using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles damage dealt to the player when in contact with barnacles.
/// </summary>
public class Barnacles : MonoBehaviour
{
    public PlayerHealth script; // Reference to the player's health script.

    /// <summary>
    /// Reduces the player's health when they collide with barnacles, if not already dead.
    /// </summary>
    /// <param name="collision">The collider of the player.</param>
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (!script.isDead) {
                script.removeHealth(); // Reduce player's health.
            }    
        }
    }
}
