using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects when the pufferfish touches the ground and halts its movement. 
/// Initiates the floating behavior upon ground contact.
/// </summary>
public class PufferfishFloorCheck : MonoBehaviour
{
    public bool startFloat = false; // Indicates when the pufferfish should start floating.
    public Rigidbody2D RB2D;

    /// <summary>
    /// Triggers when the pufferfish collides with the ground, stopping its movement and initiating float.
    /// </summary>
    /// <param name="collision">The collider of the object it has collided with.</param>
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ground") {
            RB2D.linearVelocity = new Vector2(0f, 0f);
            startFloat = true;
        }
    }
}
