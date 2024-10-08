using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer ()
    {
        GameData data = new GameData();
        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PlayerData", jsonData);
        PlayerPrefs.Save();
    }

    public static GameData LoadPlayer ()
    {
        if (PlayerPrefs.HasKey("PlayerData"))
        {
            string jsonData = PlayerPrefs.GetString("PlayerData");
            GameData data = JsonUtility.FromJson<GameData>(jsonData);
            return data;
        }
        else
        {
            Debug.LogError("Save file not found");
            return null;
        }
    }

    public static bool IsValidLevelData()
    {
        if (PlayerPrefs.HasKey("PlayerData"))
        {
            GameData data = LoadPlayer();
            return data != null && data.level > 1;
        }
        return false;
    }

    public static void SaveVolume(int volume)
    {
        volume = Mathf.Clamp(volume, 0, 100);
        PlayerPrefs.SetInt("Volume", volume);
        PlayerPrefs.Save();
    }

    public static int GetVolume()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            return PlayerPrefs.GetInt("Volume");
        }
        return 100;
    }

    public static void SaveMusicMutedState(bool isMuted)
    {
        PlayerPrefs.SetInt("MusicMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static bool LoadMusicMutedState()
    {
        if (PlayerPrefs.HasKey("MusicMuted"))
        {
            return PlayerPrefs.GetInt("MusicMuted") == 1;
        }
        return false;
    }
}
