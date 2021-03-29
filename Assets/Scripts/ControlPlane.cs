using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlane : MonoBehaviour
{
    private AtomsManager atomsManager;
    private Vector3 originPos;
    private Quaternion originRot;
    private Atom atom;
    private bool atomFlag;
    private Vector3 pivotPos = new Vector3(22, 6.6f, 10);
    void Start()
    {
        originPos = transform.position;
        originRot = transform.rotation;
        atomsManager = FindObjectOfType<AtomsManager>();
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
        //Quaternion curRotation = originRot;
        if (!atomsManager.Plane.Equals("XYZ"))
            curPosition.x = pivotPos.x;
        
        curPosition.y = originPos.y;
        curPosition.z = originPos.z;
        
        
        transform.position = curPosition;
        //transform.rotation = curRotation;
    }
    
    public void SetAtom(Atom atom, bool flag)
    {
        this.atom = atom;
        atomFlag = flag;
        
    }
}
