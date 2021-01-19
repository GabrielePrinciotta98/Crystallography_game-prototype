using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PowerUpsManger
{
    private static bool zoomUnlocked;
    private static bool lambdaUnlocked;
    private static bool powerUnlocked;
    private static bool rotationSliderUnlocked;
    private static bool rotationButtonUnlocked;
    
    public static bool ZoomUnlocked
    {
        get => zoomUnlocked;
        set => zoomUnlocked = value;
    }

    public static bool LambdaUnlocked
    {
        get => lambdaUnlocked;
        set => lambdaUnlocked = value;
    }

    public static bool PowerUnlocked
    {
        get => powerUnlocked;
        set => powerUnlocked = value;
    }

    public static bool RotationSliderUnlocked
    {
        get => rotationSliderUnlocked;
        set => rotationSliderUnlocked = value;
    }

    public static bool RotationButtonUnlocked
    {
        get => rotationButtonUnlocked;
        set => rotationButtonUnlocked = value;
    }
}
