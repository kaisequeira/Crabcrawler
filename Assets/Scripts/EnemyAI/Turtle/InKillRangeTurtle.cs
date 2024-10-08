using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InKillRangeTurtle : MonoBehaviour
{
    public PlayerHealth script;

    /// <summary>
    /// Checks if the player enters the kill range and removes health.
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
