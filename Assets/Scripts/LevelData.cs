using System.Collections.Generic;
using UnityEngine;

public static class LevelData
{
    private static List<Level> levels;
    private const string XZ = "XZ";
    private const string YZ = "YZ";
    private const string XYZ = "XYZ";
    
    private const string Desc1 = "Livello  1\n2 atomi sul piano XY";
    private const string Desc2 = "Livello  2\n3 atomi sul piano XY";
    private const string Desc3 = "Livello  3\n4 atomi sul piano XY";
    private const string Desc4 = "Livello  4\n2 atomi sul piano XZ";
    private const string Desc5 = "Livello  5\n3 atomi sul piano XZ";
    private const string Desc6 = "Livello  6\n4 atomi sul piano XZ";
    private const string Desc7 = "Livello  7\n2 atomi sul piano XYZ";
    private const string Desc8 = "Livello  8\n3 atomi sul piano XYZ";
    private const string Desc9 = "Livello  9\n4 atomi sul piano XYZ";
    private const string Desc10 = "Livello  10\n Boss Finale";
    private const string Desc11 = "Livello 11\n6 atomi sul piano XY";

    private const string DescCry1 = "Livello  10\nCristallo";
    private const string DescCry2 = "Livello  11\nCristallo";
    private const string DescCry3 = "Livello  12\nCristallo";

    /*
    
    private static readonly Vector3[] spawnPosXY = {
        new Vector3(0,-1.5f, -1.5f),
        new Vector3(0, -1.5f, 0),
        new Vector3(0, -1.5f, 1.5f),
        new Vector3(0, 0, -1.5f),
        new Vector3(0, 0, 1.5f),
        new Vector3(0, 1.5f, -1.5f),
        new Vector3(0, 1.5f, 0),
        new Vector3(0, 1.5f, 1.5f),
        
    };
    
    private static readonly Vector3[] spawnPosXZ = {
        new Vector3(-1.5f,0, -1.5f),
        new Vector3(-1.5f, 0, 0),
        new Vector3(-1.5f, 0, 1.5f),
        new Vector3(0, 0, -1.5f),
        new Vector3(0, 0, 1.5f),
        new Vector3(1.5f, 0, -1.5f),
        new Vector3(1.5f, 0, 0),
        new Vector3(1.5f, 0, 1.5f),
        
    };
    */

    public static List<Level> Levels
    {
        get => levels;
        set
        {
            levels = value;
            
            // LV 1
            levels.Add(new Level(1,1,2,false, Desc1, YZ));

            // LV 2
            levels.Add(new Level(1, 1, 3, false, Desc2, YZ));
            // LV 3
            levels.Add(new Level(1, 1, 4, false, Desc3, YZ));
            // LV 4
            levels.Add(new Level(1,1,2,false, Desc4, XZ));
            
            // Lv 5
            levels.Add(new Level(1, 1, 3, false, Desc5, XZ));

            // LV 6
            levels.Add(new Level(1, 1, 4, false, Desc6, XZ));

            // LV 7
            levels.Add(new Level(1,1,2,false, Desc7, XYZ));

            // Lv 8
            levels.Add(new Level(1, 1, 3, false, Desc8, XYZ));
          
            // LV 9
            levels.Add(new Level(1, 1, 4, false, Desc9, XYZ));

            // LV 10 CRISTALLO LIVELLO FINALE 
            levels.Add(new Level(3,27,2,true, Desc10, XYZ));
            Vector3[] lvPositions = {new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f))};
            levels[9].SolPositions = lvPositions;

            /*
            // LV 12 CRISTALLO 5x5x5 PROVA
            levels.Add(new Level(5, 125, 2, true, Desc1, XYZ));
            lvPositions = new[] {new Vector3(-0.5f, 1.3f, 1.1f)};
            levels[10].SolPositions = lvPositions;
            
            // LV 13 CRISTALLO 7x7x7 PROVA
            levels.Add(new Level(7, 343, 2, true, Desc1, XYZ));
            lvPositions = new[] {new Vector3(-0.5f, 1.3f, 1.1f)};
            levels[11].SolPositions = lvPositions;
            */
        }
    }

    /*private static Vector3[] RandomPositions(string plane, int n)
    {
        Vector3[] ris = new Vector3[n];

        for (int i = 0; i < n; i++)
        {
            bool isEqual = false;
            bool isPivot = false;
            Vector3 newPos = plane switch
            {
                "YZ" => new Vector3(0, Random.Range(-5f, 5f), Random.Range(-5f, 5f)),
                "XZ" => new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f)),
                "XYZ" => new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)),
                _ => Vector3.zero
            };
            for (int j = 0; j < i; j++)
            {
                if (ris[j] != newPos) continue;
                isEqual = true;
                break;
            }
            
            if (Vector3.Distance(newPos, Vector3.zero) <= 1)
                isPivot = true;
            
            if (isEqual || isPivot)
            {
                i--;
                continue;
            }
            ris[i] = newPos;
        }

        return ris;
    }*/
}
