using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the player's movement, jumping, crouching, and other interactions such as water effects and ground detection.
/// Manages player gravity, coyote time (for forgiving jumps), jump buffering, and interaction with audio cues.
/// </summary>
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
    private float jumpCooldown = 0.2f;
    private float lastJumpTime = -1f;

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

    /// <summary>
    /// Updates the player's horizontal movement based on input.
    /// </summary>
    void Update() {
        RB2D.linearVelocity = new Vector2(horizontal * speed, RB2D.linearVelocity.y);
    }

    /// <summary>
    /// Handles physics-based actions like ground detection, crouching, jump buffering, 
    /// coyote time, and gravity scaling for smoother jumping and falling.
    /// Also manages sound effects based on player movement and environment.
    /// </summary>
    void FixedUpdate() {
        onGround = Physics2D.BoxCast(boxCast.bounds.center, boxCast.bounds.size, 0f, Vector2.down, 0f, groundLayers);
        onPlatform = Physics2D.BoxCast(boxCast.bounds.center, boxCast.bounds.size, 0f, Vector2.down, 0f, platformLayer);

        // Crouch behavior and size adjustments based on ground detection or water
        if (!inCrouch && !(leftCheck && rightCheck)) {
            animator.SetBool("Crouch", false);
            animator.SetBool("stillCrouch", false);
            crabCollider.size = new Vector2(0.24f, 0.16f);
        } else {
            HandleCrouching();
        }

        // Adjust gravity based on player velocity (falling faster than rising)
        if (RB2D.linearVelocity.y < -0.01f) {
            RB2D.gravityScale = baseGravity * (fallMultiplier - 1);
        } else {
            RB2D.gravityScale = baseGravity;
        }

        HandleCoyoteTime();
        HandleJumpBuffering();

        // Set animation parameters based on velocity
        animator.SetFloat("Velocity", RB2D.linearVelocity.x);
        HandleJumpAnimations();
        HandleStepSounds();
        HandleWaterSounds();
    }

    /// <summary>
    /// Handles the jump action, considering coyote time, jump buffering, and cooldown.
    /// </summary>
    public void Jump(InputAction.CallbackContext context) {
        if (Time.time >= lastJumpTime + jumpCooldown) {
            if (context.performed && coyoteTimeCounter > 0) {
                ExecuteJump();
            } else if (context.performed) {
                bufferTimeCounter = bufferTime;
            }
        }

        // Cancels jump if released while ascending
        if (context.canceled && RB2D.linearVelocity.y > 0f) {
            RB2D.linearVelocity = new Vector2(RB2D.linearVelocity.x, RB2D.linearVelocity.y * 0.5f);
        }
    }

    /// <summary>
    /// Handles movement based on player input.
    /// </summary>
    public void Move(InputAction.CallbackContext context) {
        horizontal = context.ReadValue<Vector2>().x;
    }

    /// <summary>
    /// Toggles crouch state based on input.
    /// </summary>
    public void Crouch(InputAction.CallbackContext context) {
        if (context.performed) {
            inCrouch = true;
        }
        if (context.canceled) {
            inCrouch = false;
        }
    }

    /// <summary>
    /// Plays water particles for entering water.
    /// </summary>
    void CreateWaterIn() {
        waterParticlesIn.Play();
    }

    /// <summary>
    /// Plays water particles for exiting water.
    /// </summary>
    void CreateWaterOut() {
        waterParticlesOut.Play();
    }

    /// <summary>
    /// Creates dust particles when jumping from the ground.
    /// </summary>
    void CreateDust() {
        playerDust.Play();
    }

    /// <summary>
    /// Executes the jump by applying vertical force and resetting related timers.
    /// </summary>
    private void ExecuteJump() {
        crabCollider.size = new Vector2(0.24f, 0.16f);
        if (!onPlatform) {
            CreateDust();
        }
        RB2D.linearVelocity = new Vector2(RB2D.linearVelocity.x, jumpingForce);
        coyoteTimeCounter = 0;
        bufferTimeCounter = 0;
        lastJumpTime = Time.time;
    }

    /// <summary>
    /// Handles crouch behavior while on the ground or in water, adjusting movement and animations.
    /// </summary>
    private void HandleCrouching() {
        if (onGround || inWater) {
            RB2D.linearVelocity = new Vector2(0f, RB2D.linearVelocity.y);
            crabCollider.size = new Vector2(0.17f, 0.16f);
            animator.SetBool("Crouch", true);
        }
        if (inWater && !inBubbleBlast) {
            RB2D.linearVelocity = new Vector2(0f, waterCrouchFall);
        } else if (inWater && inBubbleBlast) {
            RB2D.AddForce(new Vector2(0f, waterBubbleRise));
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Crouch_Hold") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Crouch")) {
            animator.SetBool("stillCrouch", true);
        }
    }

    /// <summary>
    /// Manages the "coyote time" (extra jump time after leaving the ground) to make jumping more forgiving.
    /// </summary>
    private void HandleCoyoteTime() {
        if (onGround) {
            coyoteTimeCounter = coyoteTime;
        } else {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Handles jump buffering, which allows jump input to be registered slightly before the player lands.
    /// </summary>
    private void HandleJumpBuffering() {
        if (bufferTimeCounter > 0) {
            if (coyoteTimeCounter > 0) {
                crabCollider.size = new Vector2(0.27f, 0.16f);
                if (!onPlatform) {
                    CreateDust();
                }
                RB2D.linearVelocity = new Vector2(RB2D.linearVelocity.x, jumpingForce * 0.75f);
                coyoteTimeCounter = 0;
                bufferTimeCounter = 0;
            } else {
                bufferTimeCounter -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Adjusts jump and falling animations based on the player's velocity.
    /// </summary>
    private void HandleJumpAnimations() {
        if (RB2D.linearVelocity.y < -0.01f) {
            animator.SetBool("Falling", true);
        } else {
            animator.SetBool("Falling", false);
        }
        if (RB2D.linearVelocity.y > 0.1f && !inBubbleBlast) {
            animator.SetBool("Jump", true);
        } else {
            animator.SetBool("Jump", false);
        }
    }

    /// <summary>
    /// Plays appropriate sound effects based on the player's movement on different surfaces.
    /// </summary>
    private void HandleStepSounds() {
        if (onGround && !onPlatform && Mathf.Abs(RB2D.linearVelocity.x) > 0 && 
            !AudioManager.instance.isPlaying("SandSteps")) {
            AudioManager.instance.Play("SandSteps");
        }
        if (onPlatform && Mathf.Abs(RB2D.linearVelocity.x) > 0 && 
            !AudioManager.instance.isPlaying("WoodSteps")) {
            AudioManager.instance.Play("WoodSteps");
        }
    }

    /// <summary>
    /// Manages sound effects when the player enters or exits water.
    /// </summary>
    private void HandleWaterSounds() {
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
}