using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Manages level transitions and saves the current level progress.
/// </summary>
public class LevelTransition : MonoBehaviour
{
    public static int currentLevel;
    public Animator transition;
    private GameData data;
    [SerializeField] private float transitionTime;

    /// <summary>
    /// Resumes the saved level based on previous progress.
    /// </summary>
    public void ResumeLevel() {
        data = SaveSystem.LoadPlayer();
        currentLevel = data.level;
        if (currentLevel == 0) {
            StartCoroutine(LoadLevel(1));
        } else {
            StartCoroutine(LoadLevel(currentLevel));
        }
    }

    /// <summary>
    /// Reloads the current level.
    /// </summary>
    public void ReloadLevel() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    /// <summary>
    /// Loads the next level in the build settings.
    /// </summary>
    public void LoadNextLevel() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    /// <summary>
    /// Starts a new game from the first level.
    /// </summary>
    public void NewGame() {
        StartCoroutine(LoadLevel(1));
    }

    /// <summary>
    /// Loads a level by its index with a transition effect.
    /// </summary>
    IEnumerator LoadLevel(int LevelIndex) {
        currentLevel = LevelIndex;
        SaveSystem.SavePlayer();
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        if (LevelIndex < SceneManager.sceneCountInBuildSettings) {
            SceneManager.LoadScene(LevelIndex);
        } else {
            SceneManager.LoadScene(0);
            currentLevel = 0;
            SaveSystem.SavePlayer();
        }
    }

    /// <summary>
    /// Returns to the main menu.
    /// </summary>
    public void ReturnMenu() {
        StartCoroutine(ReturnMenu(0));
    }

    /// <summary>
    /// Transition back to the menu scene.
    /// </summary>
    IEnumerator ReturnMenu(int LevelIndex) {
        Time.timeScale = 1f;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(LevelIndex);
    }

    /// <summary>
    /// Exits the game or stops play mode in the editor.
    /// </summary>
    public void Quit() {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
