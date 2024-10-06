using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufferfishFloorCheck : MonoBehaviour
{
    public bool startFloat = false;
    public Rigidbody2D RB2D;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ground") {
            RB2D.linearVelocity = new Vector2(0f, 0f);
            startFloat = true;
        }
    }
}
