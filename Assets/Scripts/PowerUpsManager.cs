using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PowerUpsManger
{
    public static int boughtPowerUpsCounter = 0;
    private static bool swapUnlocked;
    private static bool moleculeUnlocked;
    private static bool zoomUnlocked;
    private static bool lambdaUnlocked;
    private static bool powerUnlocked;
    private static bool rotationUnlocked;


    public static bool SwapUnlocked 
    {
        get => swapUnlocked;
        set
        {
            swapUnlocked = value;
            boughtPowerUpsCounter++;
        }
    }

    public static bool MoleculeUnlocked
    {
        get => moleculeUnlocked;
        set
        {
            moleculeUnlocked = value;
            boughtPowerUpsCounter++;
        }
    }

    public static bool ZoomUnlocked
    {
        get => zoomUnlocked;
        set
        {
            zoomUnlocked = value;
            boughtPowerUpsCounter++;
        }
    }

    public static bool LambdaUnlocked
    {
        get => lambdaUnlocked;
        set
        {
            lambdaUnlocked = value;
            boughtPowerUpsCounter++;
        } 
    }

    public static bool PowerUnlocked
    {
        get => powerUnlocked;
        set
        {
            powerUnlocked = value;
            boughtPowerUpsCounter++;
        }
    }

    public static bool RotationUnlocked
    {
        get => rotationUnlocked;
        set
        {
            rotationUnlocked = value;
            boughtPowerUpsCounter++;
        }
    }
}
