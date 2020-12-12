using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionFakeShadow : MonoBehaviour
{
    private void Update()
    {
        //impedisci all'ombra di non scendere sotto al pavimento
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = 0.1f;
        //clampedPosition.x = Mathf.Clamp(clampedPosition.x, clampedPosition.x - 2f, clampedPosition.x + 2f);
        transform.position = clampedPosition;

    }
}
