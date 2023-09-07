using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeDogfish : MonoBehaviour
{
    public DogfishAI script;
    public Animator animator;
    public CapsuleCollider2D dogfishCollider;
    public PlayerController playerScript;

    void Start() {
        playerScript = FindObjectOfType<PlayerController>();
    }

    void FixedUpdate() {
        if (script.inRange && playerScript.inWater) {
            dogfishCollider.size = new Vector2(0.35f, 0.12f);
        } else if (!script.inRange || !playerScript.inWater) {
            dogfishCollider.size = new Vector2(0.35f, 0.28f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            script.inRange = true;
            animator.SetBool("inRange", script.inRange);
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            script.inRange = false;
            animator.SetBool("inRange", script.inRange); 
        }
    }
}
