using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Monitors whether the pufferfish is in water, updating its state and playing sound when exiting water.
/// </summary>
public class PufferfishWaterCheck : MonoBehaviour
{
    public bool inWater = true; // Tracks if the pufferfish is currently in water.
    public Transform pufferfishLocation;

    /// <summary>
    /// Detects when the pufferfish enters water and sets its inWater state to true.
    /// </summary>
    /// <param name="collision">The collider of the object it has collided with.</param>
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Water") {
            inWater = true;
        }
    }

    /// <summary>
    /// Detects when the pufferfish exits water, sets its inWater state to false, and plays an audio cue.
    /// </summary>
    /// <param name="collision">The collider of the object it has collided with.</param>
    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Water") {
            inWater = false;
            GetComponent<AudioSource>().Play(); // Plays a sound when leaving water.
        }
    }

    /// <summary>
    /// Updates the pufferfish's audio based on settings.
    /// </summary>
    void Update() {
        if (SettingsSystem.AudioPaused) {
            GetComponent<AudioSource>().mute = true;
        } else {
            GetComponent<AudioSource>().mute = false;
        }
    }
}
