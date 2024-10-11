using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles the main menu behavior, including button visibility for the resume and exit buttons.
/// </summary>
public class MainMenuBehaviour : MonoBehaviour
{
    public GameObject resumeButton;
    public EventSystem eventSystem; 
    public GameObject newGameButton;

    /// <summary>
    /// Initializes the main menu. Hides the resume button if no saved data is present.
    /// </summary>
    void Start()
    {
        if (!SaveSystem.IsValidLevelData()) {
            resumeButton.SetActive(false);
            eventSystem.SetSelectedGameObject(newGameButton);
        }

        GameObject indicator = resumeButton.transform.GetChild(0).gameObject;
        indicator.SetActive(SaveSystem.LoadHardMode());
    }
}
