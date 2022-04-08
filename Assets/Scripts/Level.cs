using UnityEngine;

public class Level
{
    public int R { get; }

    public int M { get; }

    public int N { get; }

    public bool IsCrystal { get; }

    public Vector3[] SolPositions { get; set; }

    public string Description { get; }

    public string Plane { get; }

    public Level(int r, int m, int n, bool isCrystal, string desc , string plane)
    {
        R = r;
        M = m;
        N = n;
        IsCrystal = isCrystal;
        Description = desc;
        Plane = plane;
    }
    
    
}
