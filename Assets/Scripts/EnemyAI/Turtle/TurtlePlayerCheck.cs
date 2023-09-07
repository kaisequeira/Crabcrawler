using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtlePlayerCheck : MonoBehaviour
{
    public TurtleMovement script;
    public Rigidbody2D RB2D_turtle;
    public Rigidbody2D RB2D_player;
    public Transform turtleLocation;
    [SerializeField] private AudioSource splashAudio;
    public static float originalPitch;
    private bool inZone = false;

    void Awake() {
        originalPitch = splashAudio.pitch;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            inZone = true; 
        }          
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            inZone = false;
        }
    }

    void FixedUpdate() {
        if (inZone) {
            if (script.turtleFacingLeft && RB2D_player.position.x <= RB2D_turtle.position.x - 0.25f) {
                script.isAgro = true;
                splashAudio.pitch = originalPitch * 2f;
            } else if (!script.turtleFacingLeft && RB2D_player.position.x >= RB2D_turtle.position.x + 0.25f) {
                script.isAgro = true;
                splashAudio.pitch = originalPitch * 2f;
            }
        }
    }
}
