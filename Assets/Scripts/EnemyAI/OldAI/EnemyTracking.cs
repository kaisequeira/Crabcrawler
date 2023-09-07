using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracking : MonoBehaviour
{

    public LockEnemy script;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            script.lockEnemyPosition = false;            
        }
        // disable new renderer and enable moving sprite
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            script.lockEnemyPosition = true;
            script.x_temp = transform.position.x;
            script.y_temp = transform.position.y;
            script.z_temp = transform.position.z;
            // set new renderer to current position
            // disable moving sprite and set new renderer enabled
        }
    }
}
