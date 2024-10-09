using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles the main menu behavior, including button visibility for the resume and exit buttons.
/// </summary>
public class MainMenuBehaviour : MonoBehaviour
{
    public GameObject resumeButton;
    public GameObject exitButton;
    public EventSystem eventSystem; 
    public GameObject newGameButton;

    /// <summary>
    /// Initializes the main menu. Hides the resume button if no saved data is present and disables the exit button on WebGL.
    /// </summary>
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
