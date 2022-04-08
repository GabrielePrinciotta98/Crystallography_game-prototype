using UnityEngine;
using UnityEngine.UI;

public class MenuCrystalPattern : MonoBehaviour
{
    private RawImage crystalPattern;
    private float newX, y, h, w;
    
    void Start()
    {
        crystalPattern = GetComponent<RawImage>();
        y = crystalPattern.uvRect.y;
        h = crystalPattern.uvRect.height;
        w = crystalPattern.uvRect.width;
    }

    void Update()
    {
        newX = (crystalPattern.uvRect.x + 0.0003f) % 1;
        crystalPattern.uvRect = new Rect(newX, y, w, h);
    }
}
