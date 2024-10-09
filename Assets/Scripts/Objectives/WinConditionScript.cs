using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the level win condition and triggers the end screen.
/// </summary>
public class WinConditionScript : MonoBehaviour
{
    [SerializeField] private Collider2D playerCollider;
    private Collider2D winCollider;
    private Animator animator;
    public SpriteRenderer PlayerSprite;
    public Rigidbody2D RB2D;
    public LevelComplete LevelCompleteScript;
    public bool levelWon = false;

    void Start() {
        animator = GetComponent<Animator>();
        winCollider = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Checks if the player has collided with the win trigger and ends the level.
    /// </summary>
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            Destroy(PlayerSprite, 0.4f);
            playerCollider.enabled = false;
            levelWon = true;
            RB2D.transform.position = winCollider.transform.position + new Vector3(0f, -0.315f, 0f);
            RB2D.isKinematic = true;
            RB2D.constraints = RigidbodyConstraints2D.FreezeAll;
            animator.SetBool("Activated", true);
            Invoke("LevelWinSplashScreen", 1.5f);
            AudioManager.instance.Play("SandCastle");
        }
    }

    /// <summary>
    /// Displays the level complete splash screen after a short delay.
    /// </summary>
    void LevelWinSplashScreen() {
        LevelCompleteScript.LevelFinish();
    }
}
