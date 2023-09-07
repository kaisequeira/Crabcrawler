using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InKillRangeDogfish : MonoBehaviour
{
    public PlayerHealth script;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (!script.isDead) {
                script.removeHealth();
            }   
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            // stun enemy
        }
    }
}
