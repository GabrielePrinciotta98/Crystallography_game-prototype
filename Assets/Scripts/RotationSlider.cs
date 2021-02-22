using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
        atomsManager.AnAtomIsMoving = false;
        solutionManager.AnAtomIsMoving = false;
    }
}
