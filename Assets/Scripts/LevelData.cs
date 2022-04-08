using System.Collections.Generic;

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
    private const string Desc7 = "Livello  7\n2 atomi nello spazio XYZ";
    private const string Desc8 = "Livello  8\n3 atomi nello spazio XYZ";
    private const string Desc9 = "Livello  9\n4 atomi nello spazio XYZ";
    private const string Desc10 = "Livello  10\n Boss Finale";



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
            
        }
    }
    
}
