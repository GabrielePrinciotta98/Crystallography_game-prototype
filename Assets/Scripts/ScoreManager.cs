using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    private static int score = 5000;

    public static int Score
    {
        get => score;
        set => score = value;
    }
}
