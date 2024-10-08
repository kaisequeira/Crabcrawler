using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleLimits : MonoBehaviour
{
    public TurtleMovement script;

    /// <summary>
    /// Flips the turtle when it collides with the ground.
    /// </summary>
    /// <param name="collision">The collider of the object entering the trigger.</param>
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ground") {
            script.FlipTurtle(); // Flip the turtle when hitting the ground.
        }
    }
}
