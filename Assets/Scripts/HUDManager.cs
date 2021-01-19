using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeReference] private Slider zoom; 
    [SerializeReference] private Slider lambda; 
    [SerializeReference] private Slider power; 
    [SerializeReference] private Slider rotationSlider; 
    [SerializeReference] private Button rotateButton; 
    [SerializeReference] private Button stopRotationButton;

    void Start()
    {
        
        if (PowerUpsManger.LambdaUnlocked)
        {
            Instantiate(lambda);
        }

        if (PowerUpsManger.PowerUnlocked)
        {
            Instantiate(power);
        }

        if (PowerUpsManger.ZoomUnlocked)
        {
            Instantiate(zoom);
        }

        if (PowerUpsManger.RotationSliderUnlocked)
        {
            Instantiate(rotationSlider);
        }

        if (PowerUpsManger.RotationButtonUnlocked)
        {
            Instantiate(rotateButton);
            Instantiate(stopRotationButton);
        }
    }

}
