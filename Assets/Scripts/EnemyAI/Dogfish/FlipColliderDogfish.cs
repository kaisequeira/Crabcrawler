using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the Dogfish's collision detection with walls and flipping its direction when a collision occurs.
/// Also manages the Dogfish's enraged state when it is stuck in a wall.
/// </summary>
public class FlipColliderDogfish : MonoBehaviour
{
    public DogfishAI script;
    public bool inWall; // Indicates if the Dogfish is colliding with a wall.
    public Animator animator;
    private float timeElapsed; // Tracks time elapsed to manage direction flipping.
    [SerializeField] private bool disableFlipReduction; // Disables time-based flip reduction if enabled.

    /// <summary>
    /// Initializes default values.
    /// </summary>
    void Start() {
        inWall = false;
    }

    /// <summary>
    /// Detects collision with the ground and sets the inWall flag to true.
    /// </summary>
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ground") {
            inWall = true;
        }
    }

    /// <summary>
    /// Detects when the Dogfish exits the collision with the ground and resets the inWall flag.
    /// </summary>
    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ground") {
            inWall = false;
        }
    }

    /// <summary>
    /// Handles direction flipping when the Dogfish is stuck in a wall for too long or the flip reduction is disabled.
    /// Also resets the enraged state if necessary.
    /// </summary>
    void FixedUpdate() {
        timeElapsed += Time.deltaTime;

        if (inWall && (timeElapsed > 0.5f || disableFlipReduction)) {
            timeElapsed = 0f;
            if (script.dogfishFacingLeft) {
                script.FlipRight();
            } else {
                script.FlipLeft();
            }
            
            if (script.enraged) {
                script.enraged = false;
                animator.SetBool("inRange", false);
            }
        }
    }
}
