using UnityEngine;

public static class LevelsUnlocked
{
    private static int levelsUnlocked = 1;

    public static int NumberOfLevelsUnlocked
    {
        get => levelsUnlocked;
        set
        {
            if (levelsUnlocked != LevelLoader.LevelCounter &&
                !GameObject.Find("CheatCode").GetComponent<CheatCode>().AlreadyActivated) return;
            levelsUnlocked = value;

        }
    }

    public static void LoadLevelsUnlocked(int loadedValue)
    {
        levelsUnlocked = loadedValue;
    }
}
