using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modifies player's physics properties when in water to simulate water resistance.
/// </summary>
public class WaterSlow : MonoBehaviour
{
    [SerializeField] float gravScale;
    [SerializeField] float linearDrag;
    [SerializeField] float waterSpeed;
    [SerializeField] float waterJump;

    public Rigidbody2D playerRB2D;
    public PlayerController script;
    private float tempGravScale;
    private float tempLinearDrag;
    private float tempSpeed;
    private float tempJumpForce;

    /// <summary>
    /// Initializes and stores default physics properties.
    /// </summary>
    void Start() {
        tempGravScale = playerRB2D.gravityScale;
        tempLinearDrag = playerRB2D.linearDamping;
        tempSpeed = script.speed;
        tempJumpForce = script.jumpingForce;
        script.inWater = false; // Player is not in water initially.
    }

    /// <summary>
    /// Applies water physics when the player enters water.
    /// </summary>
    /// <param name="collision">The collider of the water area.</param>
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "WaterPlayerCheck") {
            script.baseGravity = gravScale;
            playerRB2D.linearDamping = linearDrag;
            script.speed = waterSpeed;
            script.jumpingForce = waterJump;
            playerRB2D.linearVelocity = new Vector2(playerRB2D.linearVelocity.x, playerRB2D.linearVelocity.y * 0.3f);
            script.inWater = true;
        }
    }

    /// <summary>
    /// Restores default physics properties when the player exits water.
    /// </summary>
    /// <param name="collision">The collider of the water area.</param>
    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "WaterPlayerCheck") {
            script.baseGravity = tempGravScale;
            playerRB2D.linearDamping = tempLinearDrag;
            script.speed = tempSpeed;
            script.jumpingForce = tempJumpForce;
            script.inWater = false;
        }
    }
}
