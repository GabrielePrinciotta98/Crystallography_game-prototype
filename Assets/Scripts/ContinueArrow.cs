using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueArrow : MonoBehaviour
{
    private const float MAXHeight = -80;
    private const float MINHeight = -90;

    private RectTransform rectTransform;
    private float x;
    private float curY;
    private float z;

    private bool descending = true;
    private const float Movement = 0.2f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        var position = rectTransform.localPosition;
        x = position.x;
        z = position.z;
    }

    // Update is called once per frame
    void Update()
    {
        curY = rectTransform.localPosition.y;

        if (descending)
        {
            if (curY <= MINHeight)
                descending = false;
            else
                rectTransform.localPosition = new Vector3(x, curY - Movement, z);
        }
        else
        {
            if (curY >= MAXHeight)
                descending = true;
            else
                rectTransform.localPosition = new Vector3(x, curY + Movement, z);
        }
    }
}
