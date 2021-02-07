using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlane : MonoBehaviour
{
    private Vector3 originPos;
    
    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 curPosition = transform.position;
        curPosition.y = originPos.y;
        curPosition.z = originPos.z;
        transform.position = curPosition;
    }
}
