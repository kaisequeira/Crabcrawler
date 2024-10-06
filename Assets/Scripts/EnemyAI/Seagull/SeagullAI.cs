using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class SeagullAI : MonoBehaviour
{
    public Transform target;
    public bool inRange = false;
    public bool seagullFacingLeft = true;
    public PlayerController playerController;
    public Animator animator;

    private enum IdleType {
        Stationary,
        Moving
    }

    public enum SeagullState {
        TrackingSpawn,
        Idle,
        TrackingPlayer,
    }

    [SerializeField] private float speed;
    [SerializeField] private float idleSpeed;
    [SerializeField] private float idleRange;
    [SerializeField] private float nextWaypointDistance;
    [SerializeField] private float baseRange;
    [SerializeField] private float increasedRange;
    [SerializeField] private IdleType idleType;

    Path path;
    int currentWaypoint = 0;
    public SeagullState currentState = SeagullState.Idle;
    private Vector2 initialPosition;
    private bool playerObscured = false;
    private float inWaterTimer = 0f;
    
    Seeker seeker;
    Rigidbody2D RB2D;

    void Start() {
        seeker = GetComponent<Seeker>();
        RB2D = GetComponent<Rigidbody2D>();
        initialPosition = transform.position - new Vector3(0f, 0.04f, 0f);

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath() {
        if (seeker.IsDone() && currentState != SeagullState.Idle) {
            seeker.StartPath(
                RB2D.position,
                currentState == SeagullState.TrackingPlayer
                    ? target.position : initialPosition,
                OnPathComplete);
        }
    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
        }
    }

    void Update() {
        animator.SetBool("inRange", currentState == SeagullState.TrackingPlayer);
        if (currentState == SeagullState.TrackingPlayer && !GetComponent<AudioSource>().isPlaying) {
            GetComponent<AudioSource>().Play();   
        } else if (currentState != SeagullState.TrackingPlayer && GetComponent<AudioSource>().isPlaying) {
            GetComponent<AudioSource>().Stop();
        }
    }

    void FixedUpdate() {
        if (playerController.inWater) {
            inWaterTimer += Time.fixedDeltaTime;
            if (inWaterTimer > 2f) {
                playerObscured = true;
            }
        } else {
            inWaterTimer = 0f;
            playerObscured = false;
        }

        if (!inRange && Vector2.Distance(RB2D.position, target.position) < baseRange) {
            inRange = true;
        } else if ((inRange && Vector2.Distance(RB2D.position, target.position) > increasedRange) || playerObscured) {
            inRange = false;
        }

        if (inRange && !playerObscured) {
            currentState = SeagullState.TrackingPlayer;
        } else if (currentState == SeagullState.TrackingPlayer) {
            currentState = SeagullState.TrackingSpawn;
        } else if (currentState == SeagullState.TrackingSpawn && Vector2.Distance(RB2D.position, initialPosition) < 0.05f) {
            currentState = SeagullState.Idle;
        }

        if (currentState == SeagullState.Idle) {
            IdleBehaviour();
        } else {
            TrackingBehaviour();
        }
    }

    private void TrackingBehaviour() {
        if (path == null) {
            return;
        }

        animator.SetBool("stationaryIdle", false);

        // Manual Tracking Override - Spawn
        if (currentWaypoint >= path.vectorPath.Count && currentState == SeagullState.TrackingSpawn) {
            Vector3 returnDirection = (initialPosition - RB2D.position).normalized;
            Vector2 returnforce = returnDirection * speed * Time.deltaTime;
            RB2D.AddForce(returnforce);
        }

        // Manual Tracking Override - Player
        if (currentWaypoint >= path.vectorPath.Count &&
            currentState == SeagullState.TrackingPlayer &&
            Vector2.Distance(RB2D.position, target.position) < 2f &&
            !playerController.inWater) 
        {
            Vector3 playerDirection = ((Vector2)target.position - RB2D.position).normalized;
            Vector2 playerforce = playerDirection * speed * Time.deltaTime;
            RB2D.AddForce(playerforce);
        }

        if (currentWaypoint >= path.vectorPath.Count) {
            return;
        }

        // A* Pathfinding
        Vector3 waypointDirection = ((Vector2)path.vectorPath[currentWaypoint] - RB2D.position).normalized;
        Vector2 force = waypointDirection * speed * Time.deltaTime;
        RB2D.AddForce(force);

        if (RB2D.linearVelocity.x >= 0.01f && force.x > 0f) {
            FlipRight();
        } else if (RB2D.linearVelocity.x <= -0.01 && force.x < 0f) {
            FlipLeft();
        }

        float distance = Vector2.Distance(RB2D.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint ++;
        }
    }

    private void IdleBehaviour() {
        if (idleType == IdleType.Stationary) {
            animator.SetBool("stationaryIdle", true);
            RB2D.linearVelocity = new Vector2(0f, 0f);
            return;
        }

        animator.SetBool("movingIdle", true);

        if (seagullFacingLeft) {
            if (transform.position.x - initialPosition.x <= -idleRange) {
                FlipRight();
            }
            RB2D.linearVelocity = new Vector2(-idleSpeed, 0f);
        } else {
            if (transform.position.x - initialPosition.x >= idleRange) {
                FlipLeft();
            }
            RB2D.linearVelocity = new Vector2(idleSpeed, 0f);
        }
    }

    public void FlipLeft() {
        transform.localScale = new Vector3(1f, 1f, 1f);
        seagullFacingLeft = true;
    }

    public void FlipRight() {
        transform.localScale = new Vector3(-1f, 1f, 1f);
        seagullFacingLeft = false;
    }
}
