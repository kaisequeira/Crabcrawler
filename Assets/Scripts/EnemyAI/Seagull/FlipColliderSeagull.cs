using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the flipping behavior of the seagull when it collides with the ground.
/// Flips the seagull's direction after a specified time while in contact with the ground.
/// </summary>
public class FlipColliderSeagull : MonoBehaviour
{
    public SeagullAI script; // Reference to the SeagullAI script for accessing its properties and methods.
    public bool inWall; // Indicates whether the seagull is in contact with the ground.
    private float timeElapsed; // Timer for flipping the seagull's direction.

    void Start() {
        inWall = false; // Initializes inWall to false.
    }

    /// <summary>
    /// Detects when the seagull enters a ground collider.
    /// </summary>
    /// <param name="collision">The collider of the object it has collided with.</param>
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ground") {
            inWall = true; // Sets inWall to true when on the ground.
        }
    }

    /// <summary>
    /// Detects when the seagull exits a ground collider.
    /// </summary>
    /// <param name="collision">The collider of the object it has collided with.</param>
    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ground") {
            inWall = false; // Sets inWall to false when leaving the ground.
        }
    }

    /// <summary>
    /// Checks the conditions to flip the seagull's direction while it is in contact with the ground.
    /// </summary>
    void FixedUpdate() {
        timeElapsed += Time.deltaTime;

        if (inWall && script.currentState == SeagullAI.SeagullState.Idle && timeElapsed > 0.5f) {
            timeElapsed = 0f;
            // Flips direction based on the seagull's current facing direction.
            if (script.seagullFacingLeft) {
                script.FlipRight();
            } else {
                script.FlipLeft();
            }
        }
    }
}
