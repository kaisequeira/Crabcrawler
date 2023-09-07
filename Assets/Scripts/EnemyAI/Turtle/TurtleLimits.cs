using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleLimits : MonoBehaviour
{
    public TurtleMovement script;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ground") {
            script.FlipTurtle();
        }
    }
}
