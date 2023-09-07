using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipColliderDogfish : MonoBehaviour
{
    public DogfishAI script;
    public PlayerController playerScript;
    public bool inWall;
    private float timeElapsed;
    [SerializeField] private bool disableFlipReduction;

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

        if (inWall && (!script.inRange || !playerScript.inWater) && (timeElapsed > 0.5f || disableFlipReduction)) {
            timeElapsed = 0f;
            if (script.dogfishFacingLeft) {
                script.FlipRight();
            } else {
                script.FlipLeft();
            }
        }
    }
}
