using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

/// <summary>
/// Handles the AI behavior for the seagull, including tracking the player, idle behavior, and pathfinding.
/// </summary>
public class SeagullAI : MonoBehaviour
{
    public Transform target; // Reference to the target (player).
    public bool inRange = false; // Indicates if the seagull is within tracking range of the player.
    public bool seagullFacingLeft = true; // Indicates which direction the seagull is facing.
    public PlayerController playerController; // Reference to the player's controller.
    public Animator animator; // Animator component for animations.

    private enum IdleType {
        Stationary,
        Moving
    }

    public enum SeagullState {
        TrackingSpawn,
        Idle,
        TrackingPlayer,
    }

    [SerializeField] private float speed; // Movement speed of the seagull.
    [SerializeField] private float idleSpeed; // Speed when the seagull is idling.
    [SerializeField] private float idleRange; // Range of movement when idle.
    [SerializeField] private float nextWaypointDistance; // Distance to the next waypoint for pathfinding.
    [SerializeField] private float baseRange; // Base detection range for tracking the player.
    [SerializeField] private float increasedRange; // Increased range for tracking.
    [SerializeField] private IdleType idleType; // Type of idle behavior.

    Path path; // Current path for the seagull to follow.
    int currentWaypoint = 0; // Index of the current waypoint.
    public SeagullState currentState = SeagullState.Idle; // Current state of the seagull.
    private Vector2 initialPosition; // Starting position of the seagull.
    private bool playerObscured = false; // Tracks if the player is obscured (e.g., underwater).
    private float inWaterTimer = 0f; // Timer for tracking how long the player is underwater.
    
    Seeker seeker; // Seeker component for pathfinding.
    Rigidbody2D RB2D; // Rigidbody2D component for physics.

    void Start() {
        seeker = GetComponent<Seeker>(); // Get the Seeker component.
        RB2D = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component.
        initialPosition = transform.position - new Vector3(0f, 0.04f, 0f); // Set initial position.

        InvokeRepeating("UpdatePath", 0f, 0.5f); // Periodically update the path.
    }

    /// <summary>
    /// Updates the path for the seagull based on its current state and target position.
    /// </summary>
    void UpdatePath() {
        if (seeker.IsDone() && currentState != SeagullState.Idle) {
            seeker.StartPath(
                RB2D.position,
                currentState == SeagullState.TrackingPlayer ? target.position : initialPosition,
                OnPathComplete);
        }
    }

    /// <summary>
    /// Callback for when the path has been computed.
    /// </summary>
    /// <param name="p">The computed path.</param>
    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p; // Set the path if there is no error.
        }
    }

    void Update() {
        animator.SetBool("inRange", currentState == SeagullState.TrackingPlayer); // Update animator state.
        if (currentState == SeagullState.TrackingPlayer && !GetComponent<AudioSource>().isPlaying) {
            GetComponent<AudioSource>().Play(); // Play sound when tracking the player.
        } else if (currentState != SeagullState.TrackingPlayer && GetComponent<AudioSource>().isPlaying) {
            GetComponent<AudioSource>().Stop(); // Stop sound when not tracking the player.
        }
    }

    void FixedUpdate() {
        // Track the player's underwater status
        if (playerController.inWater) {
            inWaterTimer += Time.fixedDeltaTime;
            if (inWaterTimer > 2f) {
                playerObscured = true; // Mark the player as obscured if underwater for too long.
            }
        } else {
            inWaterTimer = 0f; // Reset timer if not underwater.
            playerObscured = false; // Player is not obscured.
        }

        // Check if the player is within range
        if (!inRange && Vector2.Distance(RB2D.position, target.position) < baseRange) {
            inRange = true; // Set inRange to true if the player is within base range.
        } else if ((inRange && Vector2.Distance(RB2D.position, target.position) > increasedRange) || playerObscured) {
            inRange = false; // Reset inRange if out of range or player is obscured.
        }

        // Update the seagull's state based on player's range
        if (inRange && !playerObscured) {
            currentState = SeagullState.TrackingPlayer; // Change state to tracking player.
        } else if (currentState == SeagullState.TrackingPlayer) {
            currentState = SeagullState.TrackingSpawn; // Change state to tracking spawn.
        } else if (currentState == SeagullState.TrackingSpawn && Vector2.Distance(RB2D.position, initialPosition) < 0.05f) {
            currentState = SeagullState.Idle; // Change state to idle when back at initial position.
        }

        // Execute behavior based on the current state
        if (currentState == SeagullState.Idle) {
            IdleBehaviour(); // Execute idle behavior.
        } else {
            TrackingBehaviour(); // Execute tracking behavior.
        }
    }

    /// <summary>
    /// Handles the behavior of the seagull while tracking the player or spawn.
    /// </summary>
    private void TrackingBehaviour() {
        if (path == null) {
            return; // Exit if no path is available.
        }

        animator.SetBool("stationaryIdle", false); // Reset stationary idle animation.

        // Manual Tracking Override - Spawn
        if (currentWaypoint >= path.vectorPath.Count && currentState == SeagullState.TrackingSpawn) {
            Vector3 returnDirection = (initialPosition - RB2D.position).normalized; // Calculate return direction.
            Vector2 returnforce = returnDirection * speed * Time.deltaTime; // Calculate return force.
            RB2D.AddForce(returnforce); // Apply return force.
        }

        // Manual Tracking Override - Player
        if (currentWaypoint >= path.vectorPath.Count &&
            currentState == SeagullState.TrackingPlayer &&
            Vector2.Distance(RB2D.position, target.position) < 2f &&
            !playerController.inWater) 
        {
            Vector3 playerDirection = ((Vector2)target.position - RB2D.position).normalized; // Calculate direction towards player.
            Vector2 playerforce = playerDirection * speed * Time.deltaTime; // Calculate force towards player.
            RB2D.AddForce(playerforce); // Apply force towards player.
        }

        if (currentWaypoint >= path.vectorPath.Count) {
            return; // Exit if at the end of the path.
        }

        // A* Pathfinding
        Vector3 waypointDirection = ((Vector2)path.vectorPath[currentWaypoint] - RB2D.position).normalized; // Direction to the next waypoint.
        Vector2 force = waypointDirection * speed * Time.deltaTime; // Force towards the waypoint.
        RB2D.AddForce(force); // Apply force towards the waypoint.

        // Flip the seagull based on direction
        if (RB2D.linearVelocity.x >= 0.01f && force.x > 0f) {
            FlipRight();
        } else if (RB2D.linearVelocity.x <= -0.01 && force.x < 0f) {
            FlipLeft();
        }

        float distance = Vector2.Distance(RB2D.position, path.vectorPath[currentWaypoint]); // Distance to the current waypoint.

        if (distance < nextWaypointDistance) {
            currentWaypoint++; // Move to the next waypoint if close enough.
        }
    }

    /// <summary>
    /// Handles the idle behavior of the seagull.
    /// </summary>
    private void IdleBehaviour() {
        if (idleType == IdleType.Stationary) {
            animator.SetBool("stationaryIdle", true); // Set stationary idle animation.
            RB2D.linearVelocity = new Vector2(0f, 0f); // Stop movement.
            return;
        }

        animator.SetBool("movingIdle", true); // Set moving idle animation.

        if (seagullFacingLeft) {
            if (transform.position.x - initialPosition.x <= -idleRange) {
                FlipRight(); // Flip if reaching the left idle limit.
            }
            RB2D.linearVelocity = new Vector2(-idleSpeed, 0f); // Move left.
        } else {
            if (transform.position.x - initialPosition.x >= idleRange) {
                FlipLeft(); // Flip if reaching the right idle limit.
            }
            RB2D.linearVelocity = new Vector2(idleSpeed, 0f); // Move right.
        }
    }

    /// <summary>
    /// Flips the seagull to face left.
    /// </summary>
    public void FlipLeft() {
        transform.localScale = new Vector3(1f, 1f, 1f); // Flip the scale to face left.
        seagullFacingLeft = true; // Update facing direction.
    }

    /// <summary>
    /// Flips the seagull to face right.
    /// </summary>
    public void FlipRight() {
        transform.localScale = new Vector3(-1f, 1f, 1f); // Flip the scale to face right.
        seagullFacingLeft = false; // Update facing direction.
    }
}
