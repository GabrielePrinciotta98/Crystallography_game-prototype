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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("swap button released");
        detector.UnSwap();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("swap button pressed");
        detector.Swap();
    }
}
