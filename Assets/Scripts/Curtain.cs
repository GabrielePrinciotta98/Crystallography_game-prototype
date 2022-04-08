using System.Collections;
using UnityEngine;

public class Curtain : MonoBehaviour
{

    public void ShowSolution()
    {
        StartCoroutine(OpenCurtains());
    }

    IEnumerator OpenCurtains()
    {
        float t = 0;
        var curtainTransform = transform;
        var localScale = curtainTransform.localScale;
        float scaleX = localScale.x;
        float scaleZ = localScale.z;
        float posX = curtainTransform.localPosition.x;
        while (t <= 1f) 
        {
            var ease = easeInOutBack(t);

            var newScaleX = Mathf.Lerp(scaleX, 3f, ease);
            var newScaleZ = Mathf.Lerp(scaleZ, 6f, ease);
            curtainTransform.localScale = new Vector3(newScaleX, transform.localScale.y, newScaleZ);
            var newPosX = Mathf.Lerp(posX, 45f, ease);
            var localPosition = curtainTransform.localPosition;
            localPosition = new Vector3(newPosX, localPosition.y, localPosition.z);
            transform.localPosition = localPosition;

            t += 0.02f;
            yield return new WaitForFixedUpdate();

        }
    }

    private float easeInOutBack(float x)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;

        return x < 0.5
            ? (Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
            : (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
        
    }
    
    
}
