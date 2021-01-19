using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtain : MonoBehaviour
{

    public void ShowSolution()
    {
        StartCoroutine(OpenCurtains());
        //solutionManager.ShowAtoms();

    }

    IEnumerator OpenCurtains()
    {
        float t = 0;
        float ease, newScaleX, newPosX, newScaleZ;
        float scaleX = transform.localScale.x;
        float scaleZ = transform.localScale.z;
        float posX = transform.localPosition.x;
        while (t <= 1f) 
        {
            ease = easeInOutBack(t);

            newScaleX = Mathf.Lerp(scaleX, 3f, ease);
            newScaleZ = Mathf.Lerp(scaleZ, 6f, ease);
            transform.localScale = new Vector3(newScaleX, transform.localScale.y, newScaleZ);
            newPosX = Mathf.Lerp(posX, 45f, ease);
            transform.localPosition = new Vector3(newPosX, transform.localPosition.y, transform.localPosition.z);

            t += 0.02f;
            yield return new WaitForFixedUpdate();

        }



    }

    public void HideSolution()
    {
        StartCoroutine(CloseCurtains());
    }

    IEnumerator CloseCurtains()
    {

        float t = 0;
        float ease, newScaleX, newPosX, newScaleZ;
        float scaleX = transform.localScale.x;
        float scaleZ = transform.localScale.z;
        float posX = transform.localPosition.x;
        while (t <= 1f) 
        {
            ease = easeInOutBack(t);

            newScaleX = Mathf.Lerp(scaleX, 18f, ease);
            newScaleZ = Mathf.Lerp(scaleZ, 3f, ease);
            transform.localScale = new Vector3(newScaleX, transform.localScale.y, newScaleZ);
            newPosX = Mathf.Lerp(posX, 2f, ease);
            transform.localPosition = new Vector3(newPosX, transform.localPosition.y, transform.localPosition.z);

            t += 0.02f;
            yield return new WaitForFixedUpdate();

        }

    }

    public float easeInOutBack(float x)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;

        return x < 0.5
            ? (Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
            : (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
        
    }
    
    public float EaseOutBounce(float x) 
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (x < 1 / d1) 
        {
            return n1 * x * x;
        }

        if (x < 2 / d1) 
        {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        }

        if (x < 2.5 / d1) 
        {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        }

        return n1 * (x -= 2.625f / d1) * x + 0.984375f;
    }
    
}
