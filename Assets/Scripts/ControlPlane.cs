using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlane : MonoBehaviour
{
    private Vector3 originPos;
    private Quaternion originRot;
    private Atom atom;
    private bool atomFlag;
    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        originRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (atomFlag)
        {
            this.transform.parent = atom.transform;
            transform.localPosition = Vector3.zero;
        }
        Vector3 curPosition = transform.position;
        Quaternion curRotation = originRot;
        curPosition.y = originPos.y;
        curPosition.z = originPos.z;
        transform.position = curPosition;
        transform.rotation = curRotation;
    }
    
    public void SetAtom(Atom atom, bool flag)
    {
        this.atom = atom;
        atomFlag = flag;

    }
}
