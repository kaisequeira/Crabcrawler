using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start() {
        tempGravScale = playerRB2D.gravityScale;
        tempLinearDrag = playerRB2D.linearDamping;
        tempSpeed = script.speed;
        tempJumpForce = script.jumpingForce;
        script.inWater = false;
    }

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

