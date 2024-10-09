using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/// <summary>
/// Manages the pause menu behavior, including pausing and resuming the game.
/// </summary>
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

    /// <summary>
    /// Initializes the pause input system.
    /// </summary>
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

    /// <summary>
    /// Determines whether to pause or resume the game.
    /// </summary>
    private void DeterminedPause() {
        if (paused) {
            ResumeGame();
        } else {
            PauseGame();
        }
    }

    /// <summary>
    /// Pauses the game and displays the pause menu if no game over or win condition has occurred.
    /// </summary>
    public void PauseGame() {
        if (!WinScript.levelWon && !GameOverScript.GameOv) {
            Time.timeScale = 0f;
            SettingsSystem.PauseGameAudio();
            EventSystem.current.SetSelectedGameObject(selectButton);
            paused = true;
            pausedMenu.SetActive(true);
            playerControls.enabled = false;
        }
    }

    /// <summary>
    /// Resumes the game and hides the pause menu.
    /// </summary>
    public void ResumeGame() {
        if (SettingsSystem.inSettings) {
            settingsScript.ExitSettings();
        }
        Time.timeScale = 1f;
        SettingsSystem.ResumeGameAudio();
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
