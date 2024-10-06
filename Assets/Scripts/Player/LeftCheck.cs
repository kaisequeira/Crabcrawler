using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftCheck : MonoBehaviour
{
    public PlayerController script;
    private Collider2D LeftCollider;
    [SerializeField] bool mainMenu;
    [SerializeField] LayerMask groundLayer;

    void Start() {
        if (mainMenu) return;
        script.leftCheck = false;
        LeftCollider = GetComponent<Collider2D>();
    }

    void FixedUpdate() {
        if (mainMenu) return;
        script.leftCheck = Physics2D.BoxCast(LeftCollider.bounds.center, LeftCollider.bounds.size, 0f, Vector2.left, 0f, groundLayer);
    }
}
