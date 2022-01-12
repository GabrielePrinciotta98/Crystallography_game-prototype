using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSol : MonoBehaviour
{
    private float zoom = 4f;
    private Vector3 newPos;
    
    // Update is called once per frame
    void Update()
    {
        newPos = transform.localPosition;
        
        //[1,10] -> [-31, -25]
        newPos.x = -31f + 6f / 9f * (zoom - 1f);

        transform.localPosition = newPos;
    }
    
    public void SetZoom(float z)
    {
        zoom = z;
    }
}
