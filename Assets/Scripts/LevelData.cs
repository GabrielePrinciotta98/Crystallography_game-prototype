using System;
using System.Collections.Generic;
using UnityEngine;

public static class LevelData
{
    private static List<Level> levels;
    private const string Desc1 = "Livello  1\n1 atomo su un piano";
    private const string Desc2 = "Livello  2\n1 atomo su un piano";
    private const string Desc3 = "Livello  3\n1 atomo";
    private const string Desc4 = "Livello  4\n2 atomi su un piano";
    private const string Desc5 = "Livello  5\n2 atomi";
    private const string Desc6 = "Livello  6\n3 atomi su un piano";
    private const string Desc7 = "Livello  7\n3 atomi";


    public static List<Level> Levels
    {
        get => levels;
        set
        {
            levels = value;
            
            // LV 1
            levels.Add(new Level(1,1,2,false, Desc1));
            Vector3[] lvPositions = {new Vector3(0, 5.5f, 0)};
            levels[0].SolPositions = lvPositions;
            
            // LV 2
            levels.Add(new Level(1, 1, 2, false, Desc2));
            lvPositions = new[] {new Vector3(0, 3.5f, 2f)};
            levels[1].SolPositions = lvPositions;
            
            // LV 3
            levels.Add(new Level(1,1,2,false, Desc3));
            lvPositions =  new[] {new Vector3(2f, -5f, 0)};
            levels[2].SolPositions = lvPositions;
            
            // LV 4
            levels.Add(new Level(1, 1, 3, false, Desc4));
            lvPositions = new[] {new Vector3(0, 4f, 0), 
                                new Vector3(0, -4f, 0)};
            levels[3].SolPositions = lvPositions;

            // Lv 5
            levels.Add(new Level(1, 1, 3, false, Desc5));
            lvPositions = new[] {new Vector3(-3f, 4f, 0), 
                                new Vector3(0, -4f, 0)};
            levels[4].SolPositions = lvPositions;
            
            // LV 6
            levels.Add(new Level(1, 1, 4, false, Desc6));
            lvPositions = new[] {new Vector3(0, 4f, 0), 
                                new Vector3(0, -4f, 0),
                                new Vector3(0, 0, 2f)};
            levels[5].SolPositions = lvPositions;
            
            // LV 7
            levels.Add(new Level(1, 1, 4, false, Desc7));
            lvPositions = new[] {new Vector3(1.5f, 4f, 1f), 
                new Vector3(-1f, 3f, 2.3f),
                new Vector3(2, -5, -3)};
            levels[6].SolPositions = lvPositions;
            
            // LV 8 CRISTALLO PROVA
            levels.Add(new Level(3, 27, 2, true, Desc1));
            lvPositions = new[] {new Vector3(-0.5f, 1.3f, 1.1f)};
            levels[7].SolPositions = lvPositions;
            
            // LV 9 CRISTALLO PROVA (PIù ATOMI)
            levels.Add(new Level(3, 27, 4, true, Desc1));
            lvPositions = new[] {new Vector3(-0.5f, 1.3f, 1.1f), 
                                new Vector3(0.9f, -0.6f, 0),
                                new Vector3(0.2f, 0.4f, -1.5f)};
            levels[8].SolPositions = lvPositions;
            
            // LV 10 CRISTALLO 5x5x5 PROVA
            levels.Add(new Level(5, 125, 2, true, Desc1));
            lvPositions = new[] {new Vector3(-0.5f, 1.3f, 1.1f)};
            levels[9].SolPositions = lvPositions;
            
            // LV 11 CRISTALLO 7x7x7 PROVA
            levels.Add(new Level(7, 343, 2, true, Desc1));
            lvPositions = new[] {new Vector3(-0.5f, 1.3f, 1.1f)};
            levels[10].SolPositions = lvPositions;
        }
    }
}
