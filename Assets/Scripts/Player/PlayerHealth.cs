using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start() {
        healthRenderer.sprite = healthSprite[playerLives];
    }

    public void removeHealth() {
        // deathAnimation
        isDead = true;
        animator.SetBool("Dead", isDead);
        AudioManager.instance.Play("HeartLoss");
        
        // disable rigidbody
        RB2D.isKinematic = true;
        RB2D.constraints = RigidbodyConstraints2D.FreezeAll;

        playerLives -= 1;
        if (playerLives >= 0) {
            healthRenderer.sprite = healthSprite[playerLives];
        }

        // Wait for animation
        Invoke("ReturnPlayerToGame", 1.05f);
    }

    private void ReturnPlayerToGame() {
        if (playerLives <= 0) {
            GameOverScript.GameComplete();
        } else {
            // return player to checkpoint
            RB2D.transform.position = currentCheckSpawnpoint;
            
            oxygenScript.Add();
            Invoke("ReturnPlayerMovement", 0.4f);

            // disable animation
            isDead = false;
            animator.SetBool("Dead", isDead);
        }        
    }

    private void ReturnPlayerMovement() {
        // enable rigidbody
        RB2D.isKinematic = false;
        RB2D.constraints = RigidbodyConstraints2D.None;
        RB2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void CheckpointAddHeart() {
        if (playerLives < 3 && playerLives > 0) {
            playerLives += 1;
            healthRenderer.sprite = healthSprite[playerLives];
        }        
    }
}
