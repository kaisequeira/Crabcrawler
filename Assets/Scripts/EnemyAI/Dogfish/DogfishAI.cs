using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DogfishAI : MonoBehaviour
{
    public bool enraged;
    public bool dogfishFacingLeft;
    public Animator animator;

    [SerializeField] private float rageSpeed;
    [SerializeField] private float idleSpeed;

    Rigidbody2D RB2D;

    void Start() {
        RB2D = GetComponent<Rigidbody2D>();
        enraged = false;
        dogfishFacingLeft = true;
    }

    void FixedUpdate() {
        if (dogfishFacingLeft) {
            RB2D.velocity = new Vector2(-(enraged ? rageSpeed : idleSpeed), 0f);
        } else {
            RB2D.velocity = new Vector2(enraged ? rageSpeed : idleSpeed, 0f);
        }
    }

    public void FlipLeft() {
        transform.localScale = new Vector3(1f, 1f, 1f);
        dogfishFacingLeft = true;
    }

    public void FlipRight() {
        transform.localScale = new Vector3(-1f, 1f, 1f);
        dogfishFacingLeft = false;
    }

}
