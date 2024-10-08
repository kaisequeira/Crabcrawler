using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuBehaviour : MonoBehaviour
{
    public GameObject resumeButton;
    public GameObject exitButton;
    public EventSystem eventSystem; 
    public GameObject newGameButton;

    void Start()
    {
        if (!SaveSystem.IsValidLevelData()) {
            resumeButton.SetActive(false);
            eventSystem.SetSelectedGameObject(newGameButton);
        }

        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            exitButton.SetActive(false);
        }
    }
}
