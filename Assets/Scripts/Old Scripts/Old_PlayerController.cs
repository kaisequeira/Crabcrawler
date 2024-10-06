using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private CapsuleCollider2D crab_collider;
    public Animator animator;

    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpForce;
    private bool onGround;
    private float moveVertical;
    private float moveHorizontal;

    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        crab_collider = GetComponent<CapsuleCollider2D>();
        onGround = true;
    }

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate() {
        if (moveHorizontal >= 0.1f) {
            rb2D.AddForce(new Vector2(moveHorizontal * moveSpeed * Time.deltaTime, 0f), ForceMode2D.Impulse);
        } else if (moveHorizontal <= -0.1f) {
            rb2D.AddForce(new Vector2(moveHorizontal * moveSpeed * Time.deltaTime, 0f), ForceMode2D.Impulse);
        }

        if (onGround == true && moveVertical >= 0.1f) {
            rb2D.AddForce(new Vector2(0f, moveVertical * jumpForce), ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        } else if (onGround == true && moveVertical <= -0.1f) {
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            crab_collider.size = new Vector2(0.16f, 0.16f);
            animator.ResetTrigger("Jump");
            animator.SetBool("Crouch", true);
        }

        if (!(moveVertical <= -0.01f)) {
            animator.SetBool("Crouch", false);
            crab_collider.size = new Vector2(0.27f, 0.16f);
        }

        animator.SetFloat("Velocity", rb2D.linearVelocity.x);
        if (rb2D.linearVelocity.y < -0.1f) {
            animator.SetBool("Falling", true);
        } else if (rb2D.linearVelocity.y >= -0.01) {
            animator.SetBool("Falling", false);
        }
    }

    // Checks for a physics trigger in this case a 2D trigger of the
    // box collider placed on the player. Uses tag data applied to the
    // ground and platforms
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Platform") {
            onGround = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Platform") {
            onGround = false;
        }
    }
}
