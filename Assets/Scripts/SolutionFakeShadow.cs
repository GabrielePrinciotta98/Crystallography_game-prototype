﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionFakeShadow : MonoBehaviour
{
    private void Start()
    {
        //impedisci all'ombra di non scendere sotto al pavimento
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = -2.4f;
        //clampedPosition.x = Mathf.Clamp(clampedPosition.x, clampedPosition.x - 2f, clampedPosition.x + 2f);
        transform.position = clampedPosition;
        //GetComponent<Renderer>().enabled = false;
    }
}
