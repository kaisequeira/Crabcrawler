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
    private float volumeStart;
    private bool result;
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
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown resolutionDropdown;
    public AudioMixer audioMixer;
    public AudioMixer musicMixer;
    public RenderPipelineAsset[] qualityLevels;

    Resolution[] resolutions;

    void Start() {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i ++) {
            string option = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRate + "hz";
            options.Add(option);

            if (resolutions[i].width == Screen.width &&
            resolutions[i].height == Screen.height) {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        initialiseSwitch = true;
        musicToggle.isOn = musicPaused;
        initialiseSwitch = false;
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        result = audioMixer.GetFloat("volume", out volumeStart);
        if (result) {
            volumeSlider.value = volumeStart;
        } else {
            volumeSlider.value = 0f;
        }
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

    public void SetVolume(float volume) {
        audioMixer.SetFloat("volume", volume);
        musicMixer.SetFloat("musicVolume", volume);
    }

    public void SetQuality(int qualityindex) {
        QualitySettings.SetQualityLevel(qualityindex);
        QualitySettings.renderPipeline = qualityLevels[qualityindex];
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    public void MusicEnabled() {
        if (!initialiseSwitch) {
            if (mainMenu || !FindObjectOfType<PauseMenu>().paused) {
                if (FindObjectOfType<AudioManager>().isPlaying("CrawlyCritters")) {
                    FindObjectOfType<AudioManager>().StopPlaying("CrawlyCritters");
                    musicPaused = true;
                } else {
                    FindObjectOfType<AudioManager>().Play("CrawlyCritters");
                    musicPaused = false;
                }
            } else {
                pauseReset = true;
            }
        }
    }

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}