using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the Dogfish's behavior when the player is within a specified range.
/// Manages the enraged state based on the player's proximity and a cooldown timer when the player exits.
/// </summary>
public class InRangeDogfish : MonoBehaviour
{
    public DogfishAI script;
    public Animator animator;
    public Rigidbody2D RB2D_player;
    public PlayerController playerScript;
    public Rigidbody2D RB2D_dogfish;
    private bool inZone = false; // Tracks if the player is within the Dogfish's range.
    private float cooldownTimer = 0f; // Timer to control how long the Dogfish remains enraged after the player exits the range.
    private const float cooldownDuration = 0.75f; // Duration for which the Dogfish stays enraged after player exits.

    /// <summary>
    /// Detects when the player enters the Dogfish's range or water.
    /// </summary>
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "WaterPlayerCheck" && playerScript.inWater) {
            inZone = true;
            cooldownTimer = 0f; // Reset cooldown timer if player reenters the zone
        }
    }

    /// <summary>
    /// Detects when the player exits the Dogfish's range or water and starts the cooldown timer.
    /// </summary>
    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "WaterPlayerCheck") {
            inZone = false;
            cooldownTimer = cooldownDuration; // Start cooldown timer
        }
    }

    /// <summary>
    /// Updates the enraged state of the Dogfish while the player is within range or handles the cooldown after the player exits.
    /// </summary>
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
