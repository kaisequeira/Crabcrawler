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
    [SerializeField] private GameObject selectButton;

    /// <summary>
    /// Activates the game over menu and disables player controls.
    /// </summary>
    public void GameComplete() {
        EventSystem.current.SetSelectedGameObject(selectButton);
        GameOv = true;
        GameOverMenu.SetActive(true);
        playerControls.enabled = false;
    }
}
