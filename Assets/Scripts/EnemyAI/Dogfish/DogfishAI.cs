using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// Controls the behavior of the Dogfish enemy, managing movement speed, direction, and enraged state.
/// </summary>
public class DogfishAI : MonoBehaviour
{
    public bool enraged; // Indicates if the Dogfish is enraged, increasing its speed.
    public bool dogfishFacingLeft; // Tracks the direction the Dogfish is facing.
    public Animator animator;

    [SerializeField] private float rageSpeed; // Speed when the Dogfish is enraged.
    [SerializeField] private float idleSpeed; // Speed when the Dogfish is idle.

    private Rigidbody2D RB2D;

    /// <summary>
    /// Initializes default values and retrieves the Rigidbody2D component.
    /// </summary>
    void Start() {
        RB2D = GetComponent<Rigidbody2D>();
        enraged = false;
        dogfishFacingLeft = true;
    }

    /// <summary>
    /// Updates the Dogfish's movement speed and direction based on its enraged state.
    /// </summary>
    void FixedUpdate() {
        if (dogfishFacingLeft) {
            RB2D.linearVelocity = new Vector2(-(enraged ? rageSpeed : idleSpeed), 0f);
        } else {
            RB2D.linearVelocity = new Vector2(enraged ? rageSpeed : idleSpeed, 0f);
        }
    }

    /// <summary>
    /// Flips the Dogfish to face left.
    /// </summary>
    public void FlipLeft() {
        transform.localScale = new Vector3(1f, 1f, 1f);
        dogfishFacingLeft = true;
    }

    /// <summary>
    /// Flips the Dogfish to face right.
    /// </summary>
    public void FlipRight() {
        transform.localScale = new Vector3(-1f, 1f, 1f);
        dogfishFacingLeft = false;
    }
}
