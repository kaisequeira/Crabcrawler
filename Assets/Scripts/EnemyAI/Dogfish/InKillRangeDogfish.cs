using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects when the player enters the Dogfish's kill range and reduces the player's health.
/// </summary>
public class InKillRangeDogfish : MonoBehaviour
{
    public PlayerHealth script;

    /// <summary>
    /// When the player enters the trigger, removes health from the player if they are not dead.
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
