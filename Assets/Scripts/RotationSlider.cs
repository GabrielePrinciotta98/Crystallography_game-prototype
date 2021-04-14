using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class RotationSlider : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private AtomsManager atomsManager;
    private SolutionManager solutionManager;

    private void Start()
    {
        atomsManager = FindObjectOfType<AtomsManager>();
        solutionManager = FindObjectOfType<SolutionManager>();
    }

    //OnPointerDown is also required to receive OnPointerUp callbacks
    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    //Do this when the mouse click on this selectable UI object is released.
    public void OnPointerUp(PointerEventData eventData)
    {
        
        Debug.Log("The mouse click was released");
        float curValue = GetComponent<Slider>().value;
        // SNAP PER I MULTIPLI DI 45
        if (curValue % 45 <= 5)
        {
            curValue -= curValue % 45;
            GetComponent<Slider>().value = curValue;
            transform.GetChild(2).transform.GetChild(0).GetComponent<Image>().color = Color.blue;
        }
        if (curValue % 45 >= 40)
        {
            curValue += 45 - curValue % 45;
            GetComponent<Slider>().value = curValue;
            transform.GetChild(2).transform.GetChild(0).GetComponent<Image>().color = Color.blue;

        }
    }


    public void ChangeHandleColor45()
    {
        float curValue = GetComponent<Slider>().value;

        if (curValue % 45 <= 5 || curValue % 45 >= 40)
            transform.GetChild(2).transform.GetChild(0).GetComponent<Image>().color = Color.blue;
        else
            transform.GetChild(2).transform.GetChild(0).GetComponent<Image>().color = Color.white;
    }

    
}
