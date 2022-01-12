using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        Color start = new Color(mat.color.r, mat.color.g, mat.color.b, 0);
        Color end = new Color(mat.color.r, mat.color.g, mat.color.b, 30f/255f);
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
