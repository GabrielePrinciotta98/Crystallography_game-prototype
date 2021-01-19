using System;
using System.Collections.Generic;
using UnityEngine;

public static class LevelData
{
    private static List<Level> levels;

    public static List<Level> SetLevels
    {
        get => levels;
        set
        {
            levels = value;
            
            // LV 1
            levels.Add(new Level(1,1,2,false));
            Vector3[] lvPositions = {new Vector3(0, 5.5f, 0)};
            levels[0].SolPositions = lvPositions;
            
            // LV 2
            levels.Add(new Level(1, 1, 2, false));
            lvPositions = new[] {new Vector3(0, 3.5f, 2f)};
            levels[1].SolPositions = lvPositions;
            
            // LV 3
            levels.Add(new Level(1,1,2,false));
            lvPositions =  new[] {new Vector3(2f, -5f, 0)};
            levels[2].SolPositions = lvPositions;
            
            // LV 4
            levels.Add(new Level(1, 1, 3, false));
            lvPositions = new[] {new Vector3(0, 4f, 0), 
                                new Vector3(0, -4f, 0)};
            levels[3].SolPositions = lvPositions;

            // Lv 5
            levels.Add(new Level(1, 1, 3, false));
            lvPositions = new[] {new Vector3(-3f, 4f, 0), 
                                new Vector3(0, -4f, 0)};
            levels[4].SolPositions = lvPositions;
            
            // LV 6
            levels.Add(new Level(1, 1, 4, false));
            lvPositions = new[] {new Vector3(0, 4f, 0), 
                                new Vector3(0, -4f, 0),
                                new Vector3(0, 0, 2f)};
            levels[5].SolPositions = lvPositions;
            
            // LV 7
            levels.Add(new Level(1, 1, 4, false));
            lvPositions = new[] {new Vector3(1.5f, 4f, 1f), 
                new Vector3(-1f, 3f, 2.3f),
                new Vector3(2, -5, -3)};
            levels[6].SolPositions = lvPositions;
            
        }
    }
}
