using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Smoothly follows the player's movement, maintaining an offset and adjusting the camera position.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0.25f, -10f);
    private float delayTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    
    [SerializeField] private Transform target;

    /// <summary>
    /// Updates the camera position each frame, smoothly following the target with a delay.
    /// </summary>
    void Update()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, delayTime);
    }
}
