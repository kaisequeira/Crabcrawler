using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects when the player is in the "bubble blast" area in the water, enabling certain player behaviors.
/// </summary>
public class BubbleBlast : MonoBehaviour
{
    public PlayerController playerScript; // Reference to the player controller script.

    /// <summary>
    /// Activates the bubble blast behavior when the player enters the water area.
    /// </summary>
    /// <param name="collision">The collider the player has triggered.</param>
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "WaterPlayerCheck") {
            playerScript.inBubbleBlast = true; // Enable bubble blast for the player.
        }
    }

    /// <summary>
    /// Deactivates the bubble blast behavior when the player exits the water area.
    /// </summary>
    /// <param name="collision">The collider the player has triggered.</param>
    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "WaterPlayerCheck") {
            playerScript.inBubbleBlast = false; // Disable bubble blast for the player.
        }
    }
}
