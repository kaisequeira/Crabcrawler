using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviours : MonoBehaviour
{
    [SerializeField] private PlayerController playerscript;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask noPlayerMask;
    private PlatformEffector2D effector;

    void Start() {
        effector = GetComponent<PlatformEffector2D>();
    }

    void FixedUpdate() {
        if (playerscript.inCrouch && playerscript.onPlatform) {
            effector.colliderMask = noPlayerMask;
        } else if (!playerscript.inCrouch && effector.colliderMask == noPlayerMask) {
            effector.colliderMask = playerMask;
        }
    }
}