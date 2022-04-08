using UnityEngine;
using UnityEngine.EventSystems;

public class SwapButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private Detector detector;
    
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
