using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterCone : MonoBehaviour
{
    
    public void SetXZ(float xz)
    {
        xz = 6f + -5f / 29f * (xz - 1f); 
        transform.localScale = new Vector3(xz, transform.localScale.y, xz);
    } 

}
