using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwapButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private Detector detector;
    
    // Start is called before the first frame update
    void Start()
    {
        detector = FindObjectOfType<Detector>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        detector.UnSwap();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        detector.Swap();
    }
}
