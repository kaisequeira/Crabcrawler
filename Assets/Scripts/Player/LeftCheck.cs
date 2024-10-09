using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks for ground collision on the left side of the player, using a BoxCast for detection.
/// </summary>
public class LeftCheck : MonoBehaviour
{
    public PlayerController script;
    private Collider2D LeftCollider;
    [SerializeField] bool mainMenu;
    [SerializeField] LayerMask groundLayer;

    /// <summary>
    /// Initializes the LeftCheck, disabling it if in the main menu.
    /// </summary>
    void Start() {
        if (mainMenu) return;
        script.leftCheck = false;
        LeftCollider = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Performs a BoxCast to detect ground collision on the left side in each fixed update cycle.
    /// </summary>
    void FixedUpdate() {
        if (mainMenu) return;
        script.leftCheck = Physics2D.BoxCast(LeftCollider.bounds.center, LeftCollider.bounds.size, 0f, Vector2.left, 0f, groundLayer);
    }
}
