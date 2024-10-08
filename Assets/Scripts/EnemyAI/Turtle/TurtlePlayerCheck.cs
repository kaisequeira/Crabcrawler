using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtlePlayerCheck : MonoBehaviour
{
    public TurtleMovement script;
    public Rigidbody2D RB2D_turtle;
    public Rigidbody2D RB2D_player;
    [SerializeField] private Transform turtleLocation;
    [SerializeField] private AudioSource splashAudio;
    public static float originalPitch;
    private bool inZone = false;

    void Awake() {
        originalPitch = splashAudio.pitch;
    }

    /// <summary>
    /// Checks if the player enters the turtle's detection zone.
    /// </summary>
    /// <param name="collision">The collider of the object entering the trigger.</param>
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            inZone = true; 
        }          
    }

    /// <summary>
    /// Checks if the player exits the turtle's detection zone.
    /// </summary>
    /// <param name="collision">The collider of the object exiting the trigger.</param>
    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            inZone = false;
        }
    }

    /// <summary>
    /// Updates the turtle's aggression state based on the player's position.
    /// </summary>
    void FixedUpdate() {
        if (inZone) {
            if ((script.turtleFacingLeft && RB2D_player.position.x <= RB2D_turtle.position.x - 0.25f) ||
                (!script.turtleFacingLeft && RB2D_player.position.x >= RB2D_turtle.position.x + 0.25f)) {
                script.isAgro = true; // Set the turtle to aggressive.
                splashAudio.pitch = originalPitch * 2f; // Change audio pitch when aggressive.
            }
        }
    }
}
