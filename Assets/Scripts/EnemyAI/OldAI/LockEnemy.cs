using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockEnemy : MonoBehaviour
{
 
    public bool lockEnemyPosition;
    public float x_temp;
    public float y_temp;
    public float z_temp;

    void Start() {
        lockEnemyPosition = true;
        x_temp = transform.position.x;
        y_temp = transform.position.y;
        z_temp = transform.position.z;
    }

    void FixedUpdate() {
        if (lockEnemyPosition) {
            transform.position = new Vector3(x_temp, y_temp, z_temp);
        }
    }

}
