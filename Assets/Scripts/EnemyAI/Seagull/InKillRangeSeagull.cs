using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects when the player enters the seagull's kill range and applies damage if the player is alive.
/// </summary>
public class InKillRangeSeagull : MonoBehaviour
{
    public PlayerHealth playerScript; // Reference to the player's health script.

    /// <summary>
    /// Reduces the player's health when they enter the seagull's kill range, if they are not dead.
    /// </summary>
    /// <param name="collision">The collider of the object entering the trigger.</param>
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (!playerScript.isDead) {
                playerScript.removeHealth(); // Applies damage to the player.
            }    
        }
    }
}
