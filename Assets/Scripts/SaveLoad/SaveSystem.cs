using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Manages saving and loading player data using PlayerPrefs for persistent storage.
/// </summary>
public static class SaveSystem
{
    /// <summary>
    /// Saves player data like current level using PlayerPrefs.
    /// </summary>
    public static void SavePlayer() {
        GameData data = new GameData();
        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PlayerData", jsonData);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Saves the current level progress after completion.
    /// </summary>
    public static void SaveLevelComplete() {
        GameData data = new GameData(LevelTransition.currentLevel + 1);
        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PlayerData", jsonData);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads saved player data from PlayerPrefs.
    /// </summary>
    public static GameData LoadPlayer() {
        if (PlayerPrefs.HasKey("PlayerData")) {
            string jsonData = PlayerPrefs.GetString("PlayerData");
            GameData data = JsonUtility.FromJson<GameData>(jsonData);
            return data;
        } else {
            Debug.LogError("Save file not found");
            return null;
        }
    }

    /// <summary>
    /// Checks if saved level data is valid.
    /// </summary>
    public static bool IsValidLevelData() {
        if (PlayerPrefs.HasKey("PlayerData")) {
            GameData data = LoadPlayer();
            return data != null && data.level > 1;
        }
        return false;
    }

    /// <summary>
    /// Saves the volume settings.
    /// </summary>
    public static void SaveVolume(int volume) {
        volume = Mathf.Clamp(volume, 0, 100);
        PlayerPrefs.SetInt("Volume", volume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads the volume settings.
    /// </summary>
    public static int GetVolume() {
        if (PlayerPrefs.HasKey("Volume")) {
            return PlayerPrefs.GetInt("Volume");
        }
        return 100;
    }

    /// <summary>
    /// Saves the music mute state.
    /// </summary>
    public static void SaveMusicMutedState(bool isMuted) {
        PlayerPrefs.SetInt("MusicMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads the music mute state.
    /// </summary>
    public static bool LoadMusicMutedState() {
        if (PlayerPrefs.HasKey("MusicMuted")) {
            return PlayerPrefs.GetInt("MusicMuted") == 1;
        }
        return false;
    }

    /// <summary>
    /// Sets the gamemode to hard mode.
    /// </summary>
    public static void SaveHardMode(bool isHardMode) {
        PlayerPrefs.SetInt("HardMode", isHardMode ? 1 : 0);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads the hard mode state.
    /// </summary>
    public static bool LoadHardMode() {
        if (PlayerPrefs.HasKey("HardMode")) {
            return PlayerPrefs.GetInt("HardMode") == 1;
        }
        return false;
    }

    /// <summary>
    /// Saves the player lives count.
    /// </summary>
    public static void SaveLives(int lives) {
        if (LoadHardMode()) {
            PlayerPrefs.SetInt("Lives", lives);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// Loads the player lives count.
    /// </summary>
    public static int LoadLives() {
        if (LoadHardMode() && PlayerPrefs.HasKey("Lives")) {
            return PlayerPrefs.GetInt("Lives");
        } else {
            return 3;
        }
    }
}
