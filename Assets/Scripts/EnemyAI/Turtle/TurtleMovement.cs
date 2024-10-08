using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleMovement : MonoBehaviour
{
    [SerializeField] private float turtleSpeed;
    [SerializeField] private float turtleAgroMultiplier;
    public Rigidbody2D RB2D;
    public bool isAgro;
    public bool turtleFacingLeft;
    public Animator animator;

    void Start() {
        turtleFacingLeft = true;
        isAgro = false;
        animator.SetBool("Left", true);
        animator.SetBool("Right", false);
    }

    /// <summary>
    /// Moves the turtle based on its aggression state.
    /// </summary>
    void FixedUpdate() {
        animator.SetBool("Agro", isAgro);
        float speed = isAgro ? turtleSpeed * turtleAgroMultiplier : turtleSpeed;
        RB2D.linearVelocity = new Vector2(turtleFacingLeft ? -speed : speed, 0);
    }

    /// <summary>
    /// Flips the turtle's direction.
    /// </summary>
    public void FlipTurtle() {
        RB2D.position += new Vector2(turtleFacingLeft ? 0.01f : -0.01f, 0);
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        turtleFacingLeft = !turtleFacingLeft;
        animator.SetBool("Left", turtleFacingLeft);
        animator.SetBool("Right", !turtleFacingLeft);
        isAgro = false;
        GetComponent<AudioSource>().pitch = TurtlePlayerCheck.originalPitch;
    }
}
