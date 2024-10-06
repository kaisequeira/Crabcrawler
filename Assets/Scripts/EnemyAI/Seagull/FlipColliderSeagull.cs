using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipColliderSeagull : MonoBehaviour
{
    public SeagullAI script;
    public bool inWall;
    private float timeElapsed;

    void Start() {
        inWall = false;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ground") {
            inWall = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Ground") {
            inWall = false;
        }
    }

    void FixedUpdate() {
        timeElapsed += Time.deltaTime;

        if (inWall && script.currentState == SeagullAI.SeagullState.Idle  && timeElapsed > 0.5f) {
            timeElapsed = 0f;
            if (script.seagullFacingLeft) {
                script.FlipRight();
            } else {
                script.FlipLeft();
            }
        }
    }
}
