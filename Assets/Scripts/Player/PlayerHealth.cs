using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the player's health, handles death, respawn, and updates the health UI.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int playerLives;
    [SerializeField] public Vector2 currentCheckSpawnpoint;
    public Rigidbody2D RB2D;
    public Animator animator;
    public PlayerController script;
    public OxygenBar oxygenScript;
    public GameOver GameOverScript;
    public bool isDead;

    public Sprite[] healthSprite;
    public SpriteRenderer healthRenderer;

    /// <summary>
    /// Initializes the player's health by updating the health UI based on the number of lives.
    /// </summary>
    private void Start() {
        healthRenderer.sprite = healthSprite[playerLives];
    }

    /// <summary>
    /// Reduces player health and handles the death process, including animations and disabling movement.
    /// </summary>
    public void removeHealth() {
        isDead = true;
        animator.SetBool("Dead", isDead);
        AudioManager.instance.Play("HeartLoss");

        RB2D.isKinematic = true;
        RB2D.constraints = RigidbodyConstraints2D.FreezeAll;

        playerLives -= 1;
        if (playerLives >= 0) {
            healthRenderer.sprite = healthSprite[playerLives];
        }

        Invoke("ReturnPlayerToGame", 1.05f);
    }

    /// <summary>
    /// Respawns the player at the last checkpoint if lives remain, or triggers a game over.
    /// </summary>
    private void ReturnPlayerToGame() {
        if (playerLives <= 0) {
            GameOverScript.GameComplete();
        } else {
            RB2D.transform.position = currentCheckSpawnpoint;
            oxygenScript.Add();
            Invoke("ReturnPlayerMovement", 0.4f);

            isDead = false;
            animator.SetBool("Dead", isDead);
        }        
    }

    /// <summary>
    /// Re-enables player movement and restores Rigidbody constraints.
    /// </summary>
    private void ReturnPlayerMovement() {
        RB2D.isKinematic = false;
        RB2D.constraints = RigidbodyConstraints2D.None;
        RB2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    /// <summary>
    /// Adds an extra life when the player reaches a checkpoint, up to a maximum of 3 lives.
    /// </summary>
    public void CheckpointAddHeart() {
        if (playerLives < 3 && playerLives > 0) {
            playerLives += 1;
            healthRenderer.sprite = healthSprite[playerLives];
        }        
    }
}
