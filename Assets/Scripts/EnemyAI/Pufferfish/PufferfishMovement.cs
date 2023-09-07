using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufferfishMovement : MonoBehaviour
{
    public PufferfishWaterCheck waterScript;
    public PufferfishFloorCheck floorScript;
    public Rigidbody2D RB2D;
    public CircleCollider2D pufferfishCollider;
    public Animator animator;

    [SerializeField] private float FloatForceApplied;

    void FixedUpdate() {
        if (waterScript.inWater && floorScript.startFloat) {
            RB2D.AddForce(new Vector2(0f, FloatForceApplied));
        } 

        if (RB2D.velocity.y >= 0) {
            Invoke("IncreaseSize", 0.1f);
            animator.SetFloat("Velocity", RB2D.velocity.y);
            // alter size
        } else if (RB2D.velocity.y < 0) {
            DecreaseSize();
            animator.SetFloat("Velocity", RB2D.velocity.y);
            // alter size
            if (!waterScript.inWater) {
                floorScript.startFloat = false;
            }
        }

        if (RB2D.velocity.y < -0.5f && waterScript.inWater) {
            if (RB2D.velocity.y + Mathf.Abs(RB2D.velocity.y * 0.05f) > -0.5f) {
                RB2D.velocity = new Vector2(0f, -0.5f);
            } else {
                RB2D.velocity = new Vector2(0f, RB2D.velocity.y + Mathf.Abs(RB2D.velocity.y * 0.05f));
            }
        }
    }

    void IncreaseSize() {
        pufferfishCollider.radius = 0.9f;
    }

    void DecreaseSize() {
        pufferfishCollider.radius = 0.45f;  
    }
}
