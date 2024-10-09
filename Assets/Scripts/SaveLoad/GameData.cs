using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the data that will be saved and loaded during gameplay, like level progression.
/// </summary>
[System.Serializable]
public class GameData
{
    public int level;

    public GameData () {
        level = LevelTransition.currentLevel;
    }

    public GameData (int level) {
        this.level = level;
    }
}
