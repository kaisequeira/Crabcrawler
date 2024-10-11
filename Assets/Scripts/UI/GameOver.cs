using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/// <summary>
/// Manages the game over state and activates the game over menu.
/// </summary>
public class GameOver : MonoBehaviour
{
    public PlayerInput playerControls;
    public bool GameOv = false;
    public GameObject GameOverMenu;
    public GameObject retryButton;
    public GameObject settingsButton;
    public RectTransform mainMenuButton;

    private bool hardMode;

    /// <summary>
    /// Starts the game over menu based on the game mode.
    /// </summary>
    public void Start() {
        hardMode = SaveSystem.LoadHardMode();

        if (hardMode) {
            retryButton.SetActive(false);
            settingsButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, -35);
            mainMenuButton.anchoredPosition = new Vector2(50, -35);
        }
    }

    /// <summary>
    /// Activates the game over menu and disables player controls.
    /// </summary>
    public void GameComplete() {
        if (hardMode) {
            LevelTransition.currentLevel = 0;
            SaveSystem.SavePlayer();
        }
        EventSystem.current.SetSelectedGameObject(hardMode ? settingsButton : retryButton);
        GameOv = true;
        GameOverMenu.SetActive(true);
        playerControls.enabled = false;
    }
}
