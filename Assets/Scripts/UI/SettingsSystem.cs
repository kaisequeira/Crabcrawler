using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public class SettingsSystem : MonoBehaviour
{
    public static bool inSettings = false;
    public static bool musicPaused;
    private bool initialiseSwitch;
    public bool pauseReset;
    [SerializeField] private PauseMenu pauseScript;
    [SerializeField] private GameOver gameOvScript;
    [SerializeField] private LevelComplete levelCompScript;
    public GameObject SettingsMenu;

    [SerializeField] private bool mainMenu;

    [SerializeField] private GameObject selectButton;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject gameOverButton;
    [SerializeField] private GameObject levelOverButton;
    [SerializeField] private GameObject mainMenuButton;

    [SerializeField] private GameObject pauseBackdrop;
    [SerializeField] private GameObject gameOverBackdrop;
    [SerializeField] private GameObject levelOverBackdrop;

    public Toggle musicToggle;
    public Slider volumeSlider;
    public AudioMixer audioMixer;
    public AudioMixer musicMixer;

    void Start() {
        initialiseSwitch = true;
        musicToggle.isOn = musicPaused;
        initialiseSwitch = false;
        volumeSlider.value = 80;
        SetVolume(volumeSlider.value);
    }

    public void EnterSettings() {
        inSettings = true;
        EventSystem.current.SetSelectedGameObject(selectButton);
        SettingsMenu.SetActive(true);

        if (mainMenu) {
            return;
        } else if (pauseScript.paused) {
            pauseBackdrop.SetActive(false);           
        } else if (gameOvScript.GameOv) {
            gameOverBackdrop.SetActive(false);             
        } else if (levelCompScript.LevelComp) {
            levelOverBackdrop.SetActive(false);  
        }
    }

    public void ExitSettings() {
        inSettings = false;
        if (mainMenu) {
            SettingsMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(mainMenuButton); 
            return;
        } else if (pauseScript.paused) {
            pauseBackdrop.SetActive(true); 
            EventSystem.current.SetSelectedGameObject(pauseButton);             
        } else if (gameOvScript.GameOv) {
            gameOverBackdrop.SetActive(true);
            EventSystem.current.SetSelectedGameObject(gameOverButton);             
        } else if (levelCompScript.LevelComp) {
            levelOverBackdrop.SetActive(true);  
            EventSystem.current.SetSelectedGameObject(levelOverButton); 
        }
        SettingsMenu.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        // Ensure the input volume is clamped between 0 and 100
        volume = Mathf.Clamp(volume, 0.0001f, 100f); // Avoid using zero to prevent log(0) issues

        // Convert linear volume (0 to 100) to logarithmic scale (-80 to 0 dB)
        float logVolume = Mathf.Log10(volume / 100f) * 20f;  // Scales it to the range -80 to 0

        // Apply the volume to the audio mixer
        audioMixer.SetFloat("volume", logVolume);
        musicMixer.SetFloat("musicVolume", logVolume);
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    public void MusicEnabled() {
        if (!initialiseSwitch) {
            if (mainMenu || !FindFirstObjectByType<PauseMenu>().paused) {
                if (FindFirstObjectByType<AudioManager>().isPlaying("CrawlyCritters")) {
                    FindFirstObjectByType<AudioManager>().StopPlaying("CrawlyCritters");
                    musicPaused = true;
                } else {
                    FindFirstObjectByType<AudioManager>().Play("CrawlyCritters");
                    musicPaused = false;
                }
            } else {
                pauseReset = true;
            }
        }
    }

    public static void PauseGameAudio() {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        foreach (Sound audioSource in audioManager.sounds) {
            if (audioSource.source != null) {
                audioSource.source.Pause();
            }
        }
    }

    public static void ResumeGameAudio() {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        foreach (Sound audioSource in audioManager.sounds) {
            if (audioSource.source != null) {
                audioSource.source.UnPause();
            }
        }
    }
}