using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Adjusts platform collision behavior based on the player's crouching state.
/// </summary>
public class PlatformBehaviours : MonoBehaviour
{
    [SerializeField] private PlayerController playerscript; // Reference to the player controller script.
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask noPlayerMask;
    private PlatformEffector2D effector;

    /// <summary>
    /// Initializes the platform effector component.
    /// </summary>
    void Start() {
        effector = GetComponent<PlatformEffector2D>();
    }

    /// <summary>
    /// Updates platform collision based on the player's crouching state.
    /// </summary>
    void FixedUpdate() {
        if (playerscript.inCrouch && playerscript.onPlatform) {
            effector.colliderMask = noPlayerMask; // Make the platform passable when crouching.
        } else if (!playerscript.inCrouch && effector.colliderMask == noPlayerMask && !playerscript.onPlatform) {
            effector.colliderMask = playerMask; // Make the platform solid again when not crouching.
        }
    }
}
