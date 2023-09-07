using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelTransition : MonoBehaviour
{
    public static int currentLevel;
    public Animator transition;
    private GameData data;
    [SerializeField] private float transitionTime;

    public void ResumeLevel() {
        data = SaveSystem.LoadPlayer();
        currentLevel = data.level;
        if (currentLevel == 0) {
            StartCoroutine(LoadLevel(1));
        } else {
            StartCoroutine(LoadLevel(currentLevel));
        }        
    }

    public void ReloadLevel() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadNextLevel() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void NewGame() {
        StartCoroutine(LoadLevel(1));
    }

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

    public void ReturnMenu() {
        StartCoroutine(ReturnMenu(0));
    }

    IEnumerator ReturnMenu(int LevelIndex) {
        Time.timeScale = 1f;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(LevelIndex);
    }

    public void Quit() {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
