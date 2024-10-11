using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles the game settings, including fullscreen toggle, volume control, and entering/exiting the settings menu.
/// </summary>
public class SettingsSystem : MonoBehaviour
{
    public static bool inSettings = false;
    public static bool AudioPaused = false;
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
    [SerializeField] private GameObject enemies;

    public Toggle fullscreenToggle;
    public Toggle musicToggle;
    public Slider volumeSlider;
    public AudioMixer audioMixer;
    public AudioMixer musicMixer;

    /// <summary>
    /// Initializes settings such as fullscreen, music state, and volume.
    /// </summary>
    void Start() {
        initialiseSwitch = true;
        musicToggle.isOn = SaveSystem.LoadMusicMutedState();
        initialiseSwitch = false;
        fullscreenToggle.isOn = Screen.fullScreen;
        SetFullscreen(fullscreenToggle.isOn);
        volumeSlider.value = SaveSystem.GetVolume();
        SetVolume(volumeSlider.value);
    }

    /// <summary>
    /// Enters the settings menu and hides any backdrop depending on the current game state.
    /// </summary>
    public void EnterSettings() {
        inSettings = true;
        EventSystem.current.SetSelectedGameObject(selectButton);
        SettingsMenu.SetActive(true);
        fullscreenToggle.isOn = Screen.fullScreen;

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

    /// <summary>
    /// Exits the settings menu and restores the previous backdrop visibility based on the game state.
    /// </summary>
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

    /// <summary>
    /// Sets the game volume using a logarithmic scale.
    /// </summary>
    public void SetVolume(float volume) {
        SaveSystem.SaveVolume((int)volume);

        // Ensure volume is clamped between 0.0001 and 100 to avoid log(0) issues
        volume = Mathf.Clamp(volume, 0.0001f, 100f);
        float logVolume = Mathf.Log10(volume / 100f) * 20f;

        audioMixer.SetFloat("volume", logVolume);
        musicMixer.SetFloat("musicVolume", logVolume);
    }

    /// <summary>
    /// Toggles fullscreen mode.
    /// </summary>
    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    /// <summary>
    /// Toggles music state and resumes or pauses it as needed.
    /// </summary>
    public void MusicEnabled() {
        if (!initialiseSwitch) {
            if (mainMenu || !FindFirstObjectByType<PauseMenu>().paused) {
                if (FindFirstObjectByType<AudioManager>().isPlaying("CrawlyCritters")) {
                    FindFirstObjectByType<AudioManager>().StopPlaying("CrawlyCritters");
                    SaveSystem.SaveMusicMutedState(true);
                } else {
                    FindFirstObjectByType<AudioManager>().Play("CrawlyCritters");
                    SaveSystem.SaveMusicMutedState(false);
                }
            } else {
                pauseReset = true;
            }
        }
    }

    /// <summary>
    /// Pauses all game audio except for the menu navigation sounds.
    /// </summary>
    public static void PauseGameAudio() {
        AudioPaused = true;
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        foreach (Sound audioSource in audioManager.sounds) {
            audioSource.source?.Pause();
        }
    }

    /// <summary>
    /// Resumes all paused game audio.
    /// </summary>
    public static void ResumeGameAudio() {
        AudioPaused = false;
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        foreach (Sound audioSource in audioManager.sounds) {
            audioSource.source?.UnPause();
        }
    }
}
