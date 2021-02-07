using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeReference] private Button zoom; 
    [SerializeReference] private Button lambda; 
    [SerializeReference] private Button power; 
    [SerializeReference] private Button rotation; 
    //[SerializeReference] private Button rotateButton; 
    //[SerializeReference] private Button stopRotationButton;

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

        if (PowerUpsManger.RotationUnlocked)
        {
            Instantiate(rotation);
        }
        /*
        if (PowerUpsManger.RotationButtonUnlocked)
        {
            Instantiate(rotateButton);
            Instantiate(stopRotationButton);
        }
        */
    }

    public void DisplaySlider(Button powerUp)
    {
        //powerUp.
    }

}
