using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    private int r; //numero di ripetizioni
    private int m; //numero di celle
    private int n; //numero di atomi (pivot incluso) 
    private bool isCrystal;
    private Vector3[] solPositions;

    public int R
    {
        get => r;
        set => r = value;
    }

    public int M
    {
        get => m;
        set => m = value;
    }

    public int N
    {
        get => n;
        set => n = value;
    }

    public bool IsCrystal
    {
        get => isCrystal;
        set => isCrystal = value;
    }

    public Vector3[] SolPositions
    {
        get => solPositions;
        set => solPositions = value;
    }


    public Level(int r, int m, int n, bool isCrystal)
    {
        this.r = r;
        this.m = m;
        this.n = n;
        this.isCrystal = isCrystal;
    }
    
    
}
