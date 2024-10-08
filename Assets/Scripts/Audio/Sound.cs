using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Represents a sound object used in the game, containing properties for volume, pitch, looping, and audio source settings.
/// </summary>
[System.Serializable]
public class Sound
{
    /// <summary>
    /// The name of the sound for easy reference.
    /// </summary>
    public string name;

    /// <summary>
    /// The audio clip associated with this sound.
    /// </summary>
    public AudioClip clip;

    /// <summary>
    /// Volume of the audio clip, adjustable within a range of 0 to 2.
    /// </summary>
    [Range(0f, 2f)]
    public float volume;

    /// <summary>
    /// Pitch of the audio clip, adjustable within a range of 0.1 to 3.
    /// </summary>
    [Range(.1f, 3f)]
    public float pitch;

    /// <summary>
    /// Determines whether the audio clip should loop when played.
    /// </summary>
    public bool loop;

    /// <summary>
    /// The audio mixer group to which this sound is routed, allowing for grouped audio management.
    /// </summary>
    public AudioMixerGroup Output;

    /// <summary>
    /// The AudioSource component generated at runtime to play the audio clip.
    /// This is hidden in the inspector but used internally to control playback.
    /// </summary>
    [HideInInspector]
    public AudioSource source;
}
