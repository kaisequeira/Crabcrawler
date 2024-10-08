using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/// <summary>
/// AudioManager handles the playing, stopping, and management of audio in the game.
/// It ensures persistent audio between scenes, controls music and sound effects, and manages volume adjustments.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;

    [SerializeField] private bool inMainMenu;
    
    private AudioSource[] additionalAudio;
    

    /// <summary>
    /// FixedUpdate checks if the current scene is the main menu and destroys the AudioManager
    /// object if necessary to prevent it from lingering when not needed.
    /// </summary>
    void FixedUpdate() {
        if (!inMainMenu && SceneManager.GetActiveScene().buildIndex == 0) {
            Destroy(gameObject);            
        }
    }

    /// <summary>
    /// Awake ensures the AudioManager is a singleton and sets up audio sources for all defined sounds.
    /// It also loads and plays specific background music if the user has not muted the music.
    /// </summary>
    void Awake() {
        if (!inMainMenu) {
            if (instance == null) {
                instance = this;
            } else {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject); // Keep the AudioManager across scenes
        }

        AudioListener.pause = false;
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.Output;
        }

        // Play background music based on saved settings
        if (SaveSystem.LoadMusicMutedState() == false) {           
            Play("CrawlyCritters");
        }
        Play("OceanCrashing");
    }

    /// <summary>
    /// Plays the audio clip corresponding to the provided name.
    /// </summary>
    /// <param name="name">The name of the sound to play.</param>
    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    /// <summary>
    /// Stops the audio clip corresponding to the provided name if it is currently playing.
    /// </summary>
    /// <param name="name">The name of the sound to stop playing.</param>
    public void StopPlaying(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    /// <summary>
    /// Checks if the audio clip corresponding to the provided name is currently playing.
    /// </summary>
    /// <param name="name">The name of the sound to check.</param>
    /// <returns>True if the sound is playing, false otherwise.</returns>
    public bool isPlaying(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return true; // Treat as playing to avoid potential errors
        }
        return s.source.isPlaying;
    }

    /// <summary>
    /// Adjusts the volume of all audio sources when the player is in water.
    /// It either increases or decreases the volume based on whether water sounds are currently playing.
    /// </summary>
    public void inWater() {
        if (isPlaying("BubbleSounds")) {
            AudioSource[] AllAudioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource s in AllAudioSources) {
                s.volume = s.volume * 2f; // Double volume when underwater
            }  
        } else {
            AudioSource[] AllAudioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource s in AllAudioSources) {
                s.volume = s.volume * 0.5f; // Halve volume when not underwater
            }
        } 
    }
}
