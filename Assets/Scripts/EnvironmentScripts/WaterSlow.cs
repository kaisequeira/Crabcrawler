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
        tempLinearDrag = playerRB2D.drag;
        tempSpeed = script.speed;
        tempJumpForce = script.jumpingForce;
        script.inWater = false;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "WaterPlayerCheck") {
            script.baseGravity = gravScale;
            playerRB2D.drag = linearDrag;
            script.speed = waterSpeed;
            script.jumpingForce = waterJump;
            playerRB2D.velocity = new Vector2(playerRB2D.velocity.x, playerRB2D.velocity.y * 0.3f);
            script.inWater = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "WaterPlayerCheck") {
            script.baseGravity = tempGravScale;
            playerRB2D.drag = tempLinearDrag;
            script.speed = tempSpeed;
            script.jumpingForce = tempJumpForce;
            script.inWater = false;
        }
    }
}

