using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftCheck : MonoBehaviour
{
    public PlayerController script;
    private Collider2D LeftCollider;
    [SerializeField] LayerMask groundLayer;

    void Start() {
        script.leftCheck = false;
        LeftCollider = GetComponent<Collider2D>();
    }

    void FixedUpdate() {
        script.leftCheck = Physics2D.BoxCast(LeftCollider.bounds.center, LeftCollider.bounds.size, 0f, Vector2.left, 0f, groundLayer);
    }
}
