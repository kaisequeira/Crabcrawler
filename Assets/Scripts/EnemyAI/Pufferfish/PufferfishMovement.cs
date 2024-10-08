using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the movement and size behavior of the pufferfish, including floating and size adjustments based on water conditions and velocity.
/// </summary>
public class PufferfishMovement : MonoBehaviour
{
    public PufferfishWaterCheck waterScript;
    public PufferfishFloorCheck floorScript;
    public Rigidbody2D RB2D;
    public CircleCollider2D pufferfishCollider;
    public Animator animator;

    [SerializeField] private float FloatForceApplied; // The upward force applied to make the pufferfish float.

    /// <summary>
    /// Manages the pufferfish's floating behavior and adjusts its size based on velocity and water presence.
    /// </summary>
    void FixedUpdate() {
        if (waterScript.inWater && floorScript.startFloat) {
            RB2D.AddForce(new Vector2(0f, FloatForceApplied));
        }

        if (RB2D.linearVelocity.y >= 0) {
            Invoke("IncreaseSize", 0.1f); // Delays size increase for 0.1 seconds.
            animator.SetFloat("Velocity", RB2D.linearVelocity.y);
            // alter size
        } else if (RB2D.linearVelocity.y < 0) {
            DecreaseSize();
            animator.SetFloat("Velocity", RB2D.linearVelocity.y);
            // alter size
            if (!waterScript.inWater) {
                floorScript.startFloat = false;
            }
        }

        if (RB2D.linearVelocity.y < -0.5f && waterScript.inWater) {
            if (RB2D.linearVelocity.y + Mathf.Abs(RB2D.linearVelocity.y * 0.05f) > -0.5f) {
                RB2D.linearVelocity = new Vector2(0f, -0.5f);
            } else {
                RB2D.linearVelocity = new Vector2(0f, RB2D.linearVelocity.y + Mathf.Abs(RB2D.linearVelocity.y * 0.05f));
            }
        }
    }

    /// <summary>
    /// Increases the pufferfish's collider size to simulate inflation.
    /// </summary>
    void IncreaseSize() {
        pufferfishCollider.radius = 0.9f;
    }

    /// <summary>
    /// Decreases the pufferfish's collider size to simulate deflation.
    /// </summary>
    void DecreaseSize() {
        pufferfishCollider.radius = 0.45f;  
    }
}
