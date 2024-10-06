using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeDogfish : MonoBehaviour
{
    public DogfishAI script;
    public Animator animator;
    public Rigidbody2D RB2D_player;
    public PlayerController playerScript;
    public Rigidbody2D RB2D_dogfish;
    private bool inZone = false;
    private float cooldownTimer = 0f;
    private const float cooldownDuration = 0.75f;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "WaterPlayerCheck" && playerScript.inWater) {
            inZone = true;
            cooldownTimer = 0f; // Reset cooldown timer if player reenters the zone
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "WaterPlayerCheck") {
            inZone = false;
            cooldownTimer = cooldownDuration; // Start cooldown timer
        }
    }

    void FixedUpdate() {
        if (inZone) {
            script.enraged = true;
            animator.SetBool("inRange", script.enraged); 
        } else if (cooldownTimer > 0) {
            cooldownTimer -= Time.fixedDeltaTime;
            if (cooldownTimer <= 0) {
                script.enraged = false;
                animator.SetBool("inRange", script.enraged);
            }
        }
    }
}