using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurtleMovement : MonoBehaviour
{
    [SerializeField] float turtleSpeed;
    [SerializeField] float turtleAgroMultiplier;
    public Rigidbody2D RB2D;
    public bool isAgro;
    public bool turtleFacingLeft;
    private float startpos_x;
    public Animator animator;
    private bool alreadyPlaying;

    void Start() {
        turtleFacingLeft = true;
        isAgro = false;
        animator.SetBool("Left", true);
        animator.SetBool("Right", false);
    }

    void FixedUpdate() {
        animator.SetBool("Agro", isAgro);
        if (isAgro) {
            if (turtleFacingLeft) {
                RB2D.velocity = new Vector2(-turtleSpeed * turtleAgroMultiplier, 0); 
            } else {
                RB2D.velocity = new Vector2(turtleSpeed * turtleAgroMultiplier, 0); 
            }
        } else {
            if (turtleFacingLeft) {
                RB2D.velocity = new Vector2(-turtleSpeed, 0); 
            } else {
                RB2D.velocity = new Vector2(turtleSpeed, 0); 
            }            
        }              
    }

    public void FlipTurtle() {
        if (!turtleFacingLeft) {
            RB2D.position = new Vector2(RB2D.position.x - 0.01f, RB2D.position.y);
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            turtleFacingLeft = true;
            animator.SetBool("Left", true);
            animator.SetBool("Right", false);
        } else if (turtleFacingLeft) {
            RB2D.position = new Vector2(RB2D.position.x + 0.01f, RB2D.position.y);
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            turtleFacingLeft = false;
            animator.SetBool("Left", false);
            animator.SetBool("Right", true);
        }
        isAgro = false;
        GetComponent<AudioSource>().pitch = TurtlePlayerCheck.originalPitch;
    }
}
