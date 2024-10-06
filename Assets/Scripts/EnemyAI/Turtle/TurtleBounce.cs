using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleBounce : MonoBehaviour
{
    public Rigidbody2D RB2D_player;
    [SerializeField] private float jumpVelocity;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            RB2D_player.linearVelocity = new Vector2(RB2D_player.linearVelocity.x * 0f, jumpVelocity);
            AudioManager.instance.Play("TurtleBounce");
        }
    }
}