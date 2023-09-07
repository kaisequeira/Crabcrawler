using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private Collider2D checkpointLocation;
    private Animator animator;
    private bool checkpointActivation = false;
    [SerializeField] private PlayerHealth playerRespawnScript;

    void Start() {
        checkpointLocation = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            animator.SetBool("Activated", true);
            if (!checkpointActivation) {
                checkpointActivation = true;
                playerRespawnScript.currentCheckSpawnpoint = checkpointLocation.transform.position;
                playerRespawnScript.CheckpointAddHeart();
                AudioManager.instance.Play("Checkpoint");
            }
        }
    }
}
