using System.Collections;
using UnityEngine;
using Color = UnityEngine.Color;

public class CentralCell : MonoBehaviour
{

    private Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        Color start = new Color(mat.color.r, mat.color.g, mat.color.b, 0);
        Color end = new Color(mat.color.r, mat.color.g, mat.color.b, 120f/255f);
        StartCoroutine(FadeIn(start, end, 2f));
    }
    
    //animazione fade-in per comparire in scena
    IEnumerator FadeIn(Color start, Color end, float duration)
    {
        for (float t = 0; t <= duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            mat.color = Color.Lerp(start, end, normalizedTime);
            yield return null;
        }
        mat.color = end;
    }

}


