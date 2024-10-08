using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects when the seagull is in contact with water and updates the animator state accordingly.
/// </summary>
public class WaterDetectorSeagull : MonoBehaviour
{
    public Animator animator; // Animator component for animations.

    /// <summary>
    /// Sets the animator state to indicate the seagull is on water when entering a water collider.
    /// </summary>
    /// <param name="collision">The collider of the object entering the trigger.</param>
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Water") {
            animator.SetBool("onWater", true); // Update animator to show the seagull is on water.
        }
    }

    /// <summary>
    /// Resets the animator state when exiting the water collider.
    /// </summary>
    /// <param name="collision">The collider of the object exiting the trigger.</param>
    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Water") {
            animator.SetBool("onWater", false); // Update animator to show the seagull is not on water anymore.
        }
    }
}
