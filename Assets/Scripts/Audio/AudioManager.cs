using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private AudioSource[] additionalAudio;
    public static AudioManager instance;
    [SerializeField] private bool inMainMenu;

    void FixedUpdate() {
        if (!inMainMenu && SceneManager.GetActiveScene().buildIndex == 0) {
            Destroy(gameObject);            
        }
    }

    void Awake()
    {
        if (!inMainMenu) {
            if (instance == null) {
                instance = this;
            } else {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
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

        if (SettingsSystem.musicPaused == false) {           
            Play("CrawlyCritters");
        }
        Play("OceanCrashing");
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void StopPlaying(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public bool isPlaying(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return true;
        }
        return s.source.isPlaying;
    }

    public void inWater() {
        if (isPlaying("BubbleSounds")) {
            AudioSource[] AllAudioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource s in AllAudioSources) {
                s.volume = s.volume * 2f;
            }  
        } else {
            AudioSource[] AllAudioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource s in AllAudioSources) {
                s.volume = s.volume * 0.5f;
            }
        } 
    }
}
