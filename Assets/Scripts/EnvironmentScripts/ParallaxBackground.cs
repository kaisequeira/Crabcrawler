using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages parallax scrolling for background elements based on camera movement.
/// </summary>
public class ParallaxBackground : MonoBehaviour
{
    private float length; // Length of the background sprite.
    private float startPos; // Starting position of the background.
    public GameObject mainCamera; // Reference to the main camera.
    [SerializeField] private float parallaxEffect; // Parallax effect strength.

    /// <summary>
    /// Initializes the start position and length of the background.
    /// </summary>
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x; // Calculate the length based on sprite size.
    }

    /// <summary>
    /// Adjusts the background position based on the camera's movement to create the parallax effect.
    /// </summary>
    void Update()
    {
        float temp = mainCamera.transform.position.x * (1 - parallaxEffect); 
        float distance = mainCamera.transform.position.x * parallaxEffect;
        transform.position = new Vector3(startPos + distance, transform.position.y);

        // Loop the background when moving past its boundaries.
        if (temp > startPos + length) {
            startPos += length;
        } else if (temp < startPos - length) {
            startPos -= length;
        }
    }
}
