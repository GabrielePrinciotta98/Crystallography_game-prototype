using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripodPosition : MonoBehaviour
{

    private float zoom = 4f;
    private Vector3 newPos;
    
    // Update is called once per frame
    void Update()
    {
        newPos = transform.localPosition;

        //[1,10] -> [46, 41]
        newPos.x = 46f + -5f / 9f * (zoom - 1f);

        transform.localPosition = newPos;
    }
    
    public void SetZoom(float z)
    {
        zoom = z;
    }
}
