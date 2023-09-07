using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameOver : MonoBehaviour
{
    public PlayerInput playerControls;
    public bool GameOv = false;
    public GameObject GameOverMenu;
    [SerializeField] private GameObject selectButton;

    public void GameComplete() {
        EventSystem.current.SetSelectedGameObject(selectButton);
        GameOv = true;
        GameOverMenu.SetActive(true);
        playerControls.enabled = false;
    }
}
