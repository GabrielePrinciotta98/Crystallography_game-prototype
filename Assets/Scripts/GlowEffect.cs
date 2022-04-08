using UnityEngine;
using UnityEngine.UI;

public class GlowEffect : MonoBehaviour
{
    private Image glowImage;
    private float h, a;
    private bool ascending;
    private Color newColor;
    
    void Start()
    {
        glowImage = GetComponent<Image>();
    }

    void Update()
    {
        a = glowImage.color.a;
        
        if (ascending)
            a += 0.01f;
        else
            a -= 0.01f;

        if (a >= 1)
            ascending = false;
        if (a <= 0)
            ascending = true;
        
        h += 0.001f;
        newColor = Color.HSVToRGB(h % 1f, 1f, 1f);
        newColor.a = a;
        glowImage.color = newColor;

    }
}
