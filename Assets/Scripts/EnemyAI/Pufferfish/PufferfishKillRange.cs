using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects when the player enters the pufferfish's kill range and decreases the player's health if applicable.
/// </summary>
public class PufferfishKillRange : MonoBehaviour
{
    public PlayerHealth script;

    /// <summary>
    /// Reduces the player's health when they enter the pufferfish's kill range, if they are not already dead.
    /// </summary>
    /// <param name="collision">The collider of the object entering the trigger.</param>
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (!script.isDead) {
                script.removeHealth();
            }    
        }
    }
}
