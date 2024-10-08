using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleBounce : MonoBehaviour
{
    public Rigidbody2D RB2D_player;
    [SerializeField] private float jumpVelocity; // Jump velocity for the player.

    /// <summary>
    /// Applies a bounce effect to the player when they collide with the turtle.
    /// </summary>
    /// <param name="collision">The collider of the object entering the trigger.</param>
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            RB2D_player.linearVelocity = new Vector2(RB2D_player.linearVelocity.x, jumpVelocity);
            AudioManager.instance.Play("TurtleBounce");
        }
    }
}
