using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D RB2D;
    public Animator animator;
    public CapsuleCollider2D crabCollider;
    private float horizontal;
    public float baseGravity;

    public BoxCollider2D boxCast;
    public bool playerDead;
    public bool inWater;
    public bool inBubbleBlast = false;
    public bool leftCheck;
    public bool rightCheck;
    public bool onPlatform = false;
    public bool inCrouch = false;
    private bool onGround = false;
    private float coyoteTimeCounter;
    private float bufferTimeCounter;

    [SerializeField] ParticleSystem waterParticlesOut;
    [SerializeField] ParticleSystem waterParticlesIn;
    [SerializeField] ParticleSystem playerDust;
    [SerializeField] LayerMask groundLayers;
    [SerializeField] LayerMask platformLayer;
    [SerializeField] private float bufferTime;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float waterCrouchFall;
    [SerializeField] private float waterBubbleRise;
    [SerializeField] public float speed;
    [SerializeField] public float jumpingForce;
    [SerializeField] public float fallMultiplier;

    void Update() {
        RB2D.velocity = new Vector2(horizontal * speed, RB2D.velocity.y);
    }

    void FixedUpdate() {
        // Ground check
        onGround = Physics2D.BoxCast(boxCast.bounds.center, boxCast.bounds.size, 0f, Vector2.down, 0f, groundLayers);
        onPlatform = Physics2D.BoxCast(boxCast.bounds.center, boxCast.bounds.size, 0f, Vector2.down, 0f, platformLayer);

        // Crouch behaviours reset with 1 wide check
        if (!inCrouch && !(leftCheck && rightCheck)) {
            animator.SetBool("Crouch", false);
            animator.SetBool("stillCrouch", false);
            crabCollider.size = new Vector2(0.24f, 0.16f);
        } else {
            // Crouch behaviours in water
            if (onGround || inWater) {
                RB2D.velocity = new Vector2(0f, RB2D.velocity.y);
                crabCollider.size = new Vector2(0.17f, 0.16f);
                animator.SetBool("Crouch", true);  
            }
            if (inWater && !inBubbleBlast) {
                RB2D.velocity = new Vector2(0f, waterCrouchFall);
            } else if (inWater && inBubbleBlast) {
                RB2D.AddForce(new Vector2(0f, waterBubbleRise));
            }

            // Crouch animation consistency
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Crouch_Hold") || animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Crouch")) {
                animator.SetBool("stillCrouch", true);            
            }
        }

        // Gravity fall multiplier
        if (RB2D.velocity.y < -0.01f) {
            RB2D.gravityScale = baseGravity * (fallMultiplier - 1);
        } else {
            RB2D.gravityScale = baseGravity;
        }

        // Coyote time
        if (onGround) {
            coyoteTimeCounter = coyoteTime;
        } else {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump buffering
        if (bufferTimeCounter > 0) {
            if (coyoteTimeCounter > 0) {
                // Reset size
                crabCollider.size = new Vector2(0.27f, 0.16f);
                if (!onPlatform) {
                    CreateDust();
                }
                RB2D.velocity = new Vector2(RB2D.velocity.x, jumpingForce * 0.75f);
                coyoteTimeCounter = 0;
                bufferTimeCounter = 0;
            } else {
                bufferTimeCounter -= Time.deltaTime;
            }
        }

        animator.SetFloat("Velocity", RB2D.velocity.x);
        if (RB2D.velocity.y < -0.01f) {
            animator.SetBool("Falling", true);
        } else if (RB2D.velocity.y >= -0.01f) {
            animator.SetBool("Falling", false);
        }
        if (RB2D.velocity.y > 0.1f && !inBubbleBlast) {
            animator.SetBool("Jump", true);
        } else {
            animator.SetBool("Jump", false);            
        }

        if (onGround && !onPlatform && Mathf.Abs(RB2D.velocity.x) > 0 &&
            !AudioManager.instance.isPlaying("SandSteps")) {
            AudioManager.instance.Play("SandSteps");
        }
        if (onPlatform && Mathf.Abs(RB2D.velocity.x) > 0 &&
            !AudioManager.instance.isPlaying("WoodSteps")) {
            AudioManager.instance.Play("WoodSteps");
        }
        if (inWater && !AudioManager.instance.isPlaying("BubbleSounds")) {
            CreateWaterIn();
            AudioManager.instance.Play("WaterIn");
            AudioManager.instance.inWater();
            AudioManager.instance.Play("BubbleSounds");
        } else if (!inWater && AudioManager.instance.isPlaying("BubbleSounds")) {
            CreateWaterOut();
            AudioManager.instance.Play("WaterExit");
            AudioManager.instance.inWater(); 
            AudioManager.instance.StopPlaying("BubbleSounds");           
        }
    }

    public void Jump(InputAction.CallbackContext context) {
        if (context.performed && coyoteTimeCounter > 0) {
            // Reset size
            crabCollider.size = new Vector2(0.24f, 0.16f);
            if (!onPlatform) {
                CreateDust();
            }
            RB2D.velocity = new Vector2(RB2D.velocity.x, jumpingForce);
            coyoteTimeCounter = 0;
            bufferTimeCounter = 0;
        } else if (context.performed) {
            bufferTimeCounter = bufferTime;
        }

        if (context.canceled && RB2D.velocity.y > 0f) {
            RB2D.velocity = new Vector2(RB2D.velocity.x, RB2D.velocity.y * 0.5f);
        }
    }

    public void Move(InputAction.CallbackContext context) {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Crouch(InputAction.CallbackContext context) {
        if (context.performed) {
            inCrouch = true;
        }
        if (context.canceled) {
            inCrouch = false;
        }
    }

    void CreateWaterIn() {
        waterParticlesIn.Play();
    }

    void CreateWaterOut() {
        waterParticlesOut.Play();
    }

    void CreateDust() {
        playerDust.Play();
    }
}