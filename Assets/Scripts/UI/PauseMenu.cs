using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{

    PauseInput pauseAction;
    public PlayerInput playerControls;   
    public bool paused = false;
    public GameObject pausedMenu;
    public SettingsSystem settingsScript;
    [SerializeField] private GameObject selectButton;

    public WinConditionScript WinScript;
    public GameOver GameOverScript;

    private void Awake() {
        pauseAction = new PauseInput();
    }

    private void OnEnable() {
        pauseAction.Enable();
    }

    private void OnDisable() {
        pauseAction.Disable();
    }

    private void Start() {
        pauseAction.Pause.PauseGame.performed += _ => DeterminedPause();
    }

    private void DeterminedPause() {
        if (paused) {
            ResumeGame();
        } else {
            PauseGame();
        }
    }

    public void PauseGame() {
        if (!WinScript.levelWon && !GameOverScript.GameOv) {
            Time.timeScale = 0f;
            AudioListener.pause = true;
            EventSystem.current.SetSelectedGameObject(selectButton);
            paused = true;
            pausedMenu.SetActive(true);
            playerControls.enabled = false;
        }
    }

    public void ResumeGame() {
        if (SettingsSystem.inSettings) {
            settingsScript.ExitSettings();
        }
        Time.timeScale = 1f;
        AudioListener.pause = false;
        EventSystem.current.SetSelectedGameObject(null);
        paused = false;
        pausedMenu.SetActive(false);
        playerControls.enabled = true;

        if (FindObjectOfType<SettingsSystem>().pauseReset) {
            FindObjectOfType<SettingsSystem>().pauseReset = false;
            FindObjectOfType<SettingsSystem>().MusicEnabled();            
        }

    }
}
