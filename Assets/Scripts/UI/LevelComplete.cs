using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class LevelComplete : MonoBehaviour
{
    public PlayerInput playerControls;
    public GameObject LevelCompMenu;
    public bool LevelComp = false; 
    [SerializeField] private GameObject selectButton;

    public void LevelFinish() {
        LevelComp = true;
        EventSystem.current.SetSelectedGameObject(selectButton);
        LevelCompMenu.SetActive(true);
        playerControls.enabled = false;
    }
}
