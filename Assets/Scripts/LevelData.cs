using System.Collections.Generic;
using UnityEngine;

public static class LevelData
{
    private static List<Level> levels;
    private const string XZ = "XZ";
    private const string YZ = "YZ";
    private const string XYZ = "XYZ";
    
    private const string Desc1 = "Livello  1\n1 atomo sul piano XY";
    private const string Desc2 = "Livello  2\n1 atomo sul piano XY";
    private const string Desc3 = "Livello  3\n1 atomo sul piano XZ";
    private const string Desc4 = "Livello  4\n2 atomi sul piano XY";
    private const string Desc5 = "Livello  5\n2 atomi sul piano XZ";
    private const string Desc6 = "Livello  6\n3 atomi sul piano XY";
    private const string Desc7 = "Livello  7\n3 atomi sul piano XZ";
    private const string Desc8 = "Livello  8\n1 atomo sul piano XYZ";
    private const string Desc9 = "Livello  9\n2 atomi sul piano XYZ";

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

    public static List<Level> Levels
    {
        get => levels;
        set
        {
            levels = value;
            
            // LV 1
            levels.Add(new Level(1,1,2,false, Desc1, YZ));
            Vector3[] lvPositions = RandomPositions(YZ, 1);
            levels[0].SolPositions = lvPositions;
            
            // LV 2
            levels.Add(new Level(1, 1, 2, false, Desc2, YZ));
            lvPositions = RandomPositions(YZ, 1);
            levels[1].SolPositions = lvPositions;
            
            // LV 3
            levels.Add(new Level(1,1,2,false, Desc3, XZ));
            lvPositions =  RandomPositions(XZ, 1);
            levels[2].SolPositions = lvPositions;
            
            // LV 4
            levels.Add(new Level(1, 1, 3, false, Desc4, YZ));
            lvPositions = RandomPositions(YZ, 2);
            levels[3].SolPositions = lvPositions;

            // Lv 5
            levels.Add(new Level(1, 1, 3, false, Desc5, XZ));
            lvPositions = RandomPositions(XZ, 2);
                                
            levels[4].SolPositions = lvPositions;
            
            // LV 6
            levels.Add(new Level(1, 1, 4, false, Desc6, YZ));
            lvPositions = RandomPositions(YZ, 3);
            levels[5].SolPositions = lvPositions;
            
            // LV 7
            levels.Add(new Level(1, 1, 4, false, Desc7, XZ));
            lvPositions = RandomPositions(XZ, 3);
            levels[6].SolPositions = lvPositions;
            
            // LV 8
            levels.Add(new Level(1,1,2,false, Desc8, XYZ));
            lvPositions = RandomPositions(XYZ, 1);
            levels[7].SolPositions = lvPositions;
            
            // Lv 9
            levels.Add(new Level(1, 1, 3, false, Desc9, XYZ));
            lvPositions = RandomPositions(XYZ, 2);
            levels[8].SolPositions = lvPositions;
            
            // LV 10 CRISTALLO PROVA
            levels.Add(new Level(3, 27, 2, true, Desc1, XYZ));
            lvPositions = new[] {new Vector3(-0.5f, 1.3f, 1.1f)};
            levels[9].SolPositions = lvPositions;
            
            // LV 11 CRISTALLO PROVA (PIù ATOMI)
            levels.Add(new Level(3, 27, 4, true, Desc1, XYZ));
            lvPositions = new[] {new Vector3(-0.5f, 1.3f, 1.1f), 
                                new Vector3(0.9f, -0.6f, 0),
                                new Vector3(0.2f, 0.4f, -1.5f)};
            levels[10].SolPositions = lvPositions;
            
            // LV 12 CRISTALLO 5x5x5 PROVA
            levels.Add(new Level(5, 125, 2, true, Desc1, XYZ));
            lvPositions = new[] {new Vector3(-0.5f, 1.3f, 1.1f)};
            levels[11].SolPositions = lvPositions;
            
            // LV 13 CRISTALLO 7x7x7 PROVA
            levels.Add(new Level(7, 343, 2, true, Desc1, XYZ));
            lvPositions = new[] {new Vector3(-0.5f, 1.3f, 1.1f)};
            levels[12].SolPositions = lvPositions;
            
        }
    }

    private static Vector3[] RandomPositions(string plane, int n)
    {
        Vector3[] ris = new Vector3[n];

        for (int i = 0; i < n; i++)
        {
            bool isEqual = false;
            bool isSpawn = false;
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

            if (plane.Equals("XZ"))
            {
                for (int j = 0; j < 8; j++)
                {
                    if (!(Vector3.Distance(newPos, spawnPosXZ[j]) < 1)) continue;
                    isSpawn = true;
                    break;
                }
            }
            else
            {
                for (int j = 0; j < 8; j++)
                {
                    if (!(Vector3.Distance(newPos, spawnPosXY[j]) < 1)) continue;
                    isSpawn = true;
                    break;
                }
            }

            if (Vector3.Distance(newPos, Vector3.zero) <= 1)
                isPivot = true;
            
            if (isEqual || isSpawn || isPivot)
            {
                i--;
                continue;
            }
            ris[i] = newPos;
        }

        return ris;
    }
}
