using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks for ground collision on the right side of the player, using a BoxCast for detection.
/// </summary>
public class RightCheck : MonoBehaviour
{
    public PlayerController script;
    private Collider2D RightCollider;
    [SerializeField] bool mainMenu;
    [SerializeField] LayerMask groundLayer;

    /// <summary>
    /// Initializes the RightCheck, disabling it if in the main menu.
    /// </summary>
    void Start() {
        if (mainMenu) return;
        script.rightCheck = false;
        RightCollider = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Performs a BoxCast to detect ground collision on the right side in each fixed update cycle.
    /// </summary>
    void FixedUpdate() {
        if (mainMenu) return;
        script.rightCheck = Physics2D.BoxCast(RightCollider.bounds.center, RightCollider.bounds.size, 0f, Vector2.right, 0f, groundLayer);
    }
}
