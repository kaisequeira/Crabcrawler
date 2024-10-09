using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/// <summary>
/// Handles level completion and activates the level complete menu.
/// </summary>
public class LevelComplete : MonoBehaviour
{
    public PlayerInput playerControls;
    public GameObject LevelCompMenu;
    public bool LevelComp = false;
    [SerializeField] private GameObject selectButton;

    /// <summary>
    /// Activates the level complete menu and disables player controls.
    /// </summary>
    public void LevelFinish() {
        LevelComp = true;
        EventSystem.current.SetSelectedGameObject(selectButton);
        LevelCompMenu.SetActive(true);
        playerControls.enabled = false;
    }
}
