using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Plane = UnityEngine.Plane;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ControlGizmo : MonoBehaviour
{
    private string levelType; 
    private GameObject moleculeSpace;
    private Atom atom;
    
    private Transform frontPlane;
    private Transform backPlane;
    private Transform frontLine;
    private Transform backLine;
    private Transform frontDottedCircle;
    private Transform backDottedCircle;
    private Transform frontSphere;
    private Transform backSphere;
    private Transform frontDottedSlice;
    private Transform backDottedSlice;
    
    private Vector3 sliceNormal; //normal of the plan of the meridian 
    public bool is3D, isSphere;
    public int axis;

    public Material[] planeMaterials;
    public Material[] lineMaterials;
    public Material[] circleMaterials;
    public Material[] sliceMaterials;
    
    void Start()
    {
        if (levelType.Equals("XYZ")) is3D = true;
        
        moleculeSpace = GameObject.Find("MoleculeSpace");
        frontPlane = transform.Find("FrontPlane");
        backPlane = transform.Find("BackPlane");
        frontLine = transform.Find("FrontLine");
        backLine = transform.Find("BackLine");
        frontDottedCircle = transform.Find("FrontDottedCircle");
        backDottedCircle = transform.Find("BackDottedCircle");
        frontSphere = transform.Find("FrontSphere");
        backSphere = transform.Find("BackSphere");
        frontDottedSlice = transform.Find("FrontDottedSlice");
        backDottedSlice = transform.Find("BackDottedSlice");
    }

    public void EnableElements()
    {
        EnablePlane(!isSphere || !is3D);
        EnableLine(!isSphere && is3D);
        EnableSphere(isSphere && is3D);
        EnableSlice(isSphere && is3D);
        EnableDottedCircle(isSphere);
    }
    
    public void DisableElements()
    {
        EnableLine(false);
        EnablePlane(false);
        EnableSlice(false);
        EnableSphere(false); 
        EnableDottedCircle(false);
    }

    private void EnablePlane(bool on)
    {
        axis = levelType switch
        {
            "XZ" => 1,
            "YZ" => 0,
            _ => 1
            //_ => BestPlaneAxis()
        };

        frontPlane.GetComponent<Renderer>().enabled = on;
        backPlane.GetComponent<Renderer>().enabled = on;
    }

    private void EnableLine(bool on)
    {
        frontLine.GetComponent<Renderer>().enabled = on;
        backLine.GetComponent<Renderer>().enabled = on;
    }

    private void EnableSphere(bool on)
    {
        frontSphere.GetComponent<Renderer>().enabled = on;
        backSphere.GetComponent<Renderer>().enabled = on;
    }

    private void EnableSlice(bool on)
    {
        frontDottedSlice.GetComponent<Renderer>().enabled = on;
        backDottedSlice.GetComponent<Renderer>().enabled = on;
    }
    
    private void EnableDottedCircle(bool on)
    {
        frontDottedCircle.GetComponent<Renderer>().enabled = on;
        backDottedCircle.GetComponent<Renderer>().enabled = on;
    }


    // Update is called once per frame
    public void RefreshPositionPlane()
    {
        Vector3 p = Vector3.zero;
        p[axis] = atom.transform.localPosition[axis];
        frontPlane.transform.localPosition = p;
        backPlane.transform.localPosition = p;
        
        frontPlane.transform.localRotation = Quaternion.FromToRotation(Vector3.forward, Normal());
        backPlane.transform.localRotation = Quaternion.FromToRotation(Vector3.back, Normal());
    }

    public void RefreshPositionLine()
    {
        Vector3 p = Vector3.zero;
        p[(axis+1)%3] = atom.transform.localPosition[(axis+1)%3];
        p[(axis+2)%3] = atom.transform.localPosition[(axis+2)%3];
        frontLine.transform.localPosition = p;
        backLine.transform.localPosition = p;
        
        frontLine.transform.localRotation = Quaternion.FromToRotation(Vector3.up, Normal());
        backLine.transform.localRotation = Quaternion.FromToRotation(Vector3.up, Normal());
        backLine.transform.Rotate(0, 180, 0,Space.Self);
    }

    public void RefreshPositionSphere()
    {
        if (atom.molecularParent)
        {
            frontSphere.transform.position = atom.molecularParent.transform.position;
            backSphere.transform.position = atom.molecularParent.transform.position;
            
            float distance = Vector3.Distance(atom.transform.position, atom.molecularParent.transform.position) * 2;
            frontSphere.transform.localScale = new Vector3(distance,distance,distance);
            backSphere.transform.localScale = new Vector3(distance,distance,distance);
        }
        else
        {
            frontSphere.transform.position = moleculeSpace.transform.position;
            backSphere.transform.position = moleculeSpace.transform.position;
            
            float distance = Vector3.Distance(atom.transform.position, moleculeSpace.transform.position) * 2;
            frontSphere.transform.localScale = new Vector3(distance,distance,distance);
            backSphere.transform.localScale = new Vector3(distance,distance,distance);
        }
            
    }

    public void RefreshPositionDottedCircle()
    {
        Vector3 fatherPos = atom.molecularParent.transform.localPosition;
        Vector3 childPos = atom.transform.localPosition;
        Vector3 p = fatherPos;
        p[axis] = childPos[axis];
        frontDottedCircle.transform.localPosition = p;
        backDottedCircle.transform.localPosition = p;
        
        
        float targetDistance = atom.distanceToMolecularParent;
        float k = childPos[axis] - fatherPos[axis];
        float r = Mathf.Sqrt(Mathf.Abs(targetDistance * targetDistance - k*k)) * 2;
        frontDottedCircle.transform.localScale =
            backDottedCircle.transform.localScale = new Vector3(r, r, r);

        frontDottedCircle.transform.localRotation = Quaternion.FromToRotation(Vector3.forward, Normal());
        backDottedCircle.transform.localRotation = Quaternion.FromToRotation(Vector3.back, Normal());

    }
    
    public void RefreshPositionSlice()
    {
        Transform frontDottedSlice = this.frontDottedSlice;
        Transform backDottedSlice = this.backDottedSlice;
        
        frontDottedSlice.localRotation = Quaternion.FromToRotation(Vector3.forward, sliceNormal);
        backDottedSlice.localRotation = Quaternion.FromToRotation(Vector3.back, sliceNormal);
        float r = atom.distanceToMolecularParent * 2;
        frontDottedSlice.transform.localScale =
            backDottedSlice.transform.localScale = new Vector3(r, r, r);

        frontDottedSlice.transform.localPosition =
            backDottedSlice.transform.localPosition = atom.molecularParent.transform.localPosition;

    }


    public Vector3 Normal()
    {
        Vector3 p = Vector3.zero;
        p[axis] = 1;
        return p;
    }
    
    public Vector3 WorldNormal()
    {
        return moleculeSpace.transform.localRotation * Normal();
    }


    public void SetAtom(Atom atom)
    {
        this.atom = atom;
        RefreshPositionPlane();
        RefreshPositionLine();
        if (isSphere)
        {
            RefreshPositionDottedCircle();
            RefreshPositionSphere();
        }
    }
    
    public void SetPlane(string s)
    {
        levelType = s;
    }
    
    public int BestPlaneAxis()
    {
        Vector3 viewDir = Camera.main.transform.forward;
        Vector3 molDir = moleculeSpace.transform.forward;
        return (Mathf.Abs(Vector3.Dot(viewDir, molDir)) < 0.8f) ? 0 : 2;
    }
    
    public Vector3 PositionUnderMousePlane(out bool fail)
    {

        fail = false;
        if (!Camera.main) return transform.position;
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        Vector3 n = WorldNormal();
        if (Mathf.Abs(Vector3.Dot(n, r.direction)) < 0.1f) fail = true;
            
        Plane p = new Plane(n, atom.transform.position);
        p.Raycast(r, out var d);
        Vector3 hit = r.GetPoint(d);
        return hit;

    }

    public Vector3 PositionUnderMousePlaneAndSphere(out bool fail)
    {
        fail = false;
        Vector3 result = PositionUnderMousePlane(out fail);
        Vector3 d = result - atom.molecularParent.transform.position;
        float targetDistance = atom.distanceToMolecularParent;
        float dot = Vector3.Dot(WorldNormal(), d);
        Vector3 dn = WorldNormal() * dot; //component along plane normal
        Vector3 dp = d - dn; //component on plane  (d = dn + dp)
        float targetDp = Mathf.Sqrt(Mathf.Abs(targetDistance * targetDistance - dot * dot));
        dp = dp.normalized * targetDp;
        return atom.molecularParent.transform.position + dp + dn;
    }

    public Vector3 PositionUnderMouseLine(out bool fail)
    {
        fail = false;
        if (!Camera.main) return transform.position;
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        Vector3 n = WorldNormal();

        n = Vector3.Cross(n, Vector3.Cross(r.direction, n));
        if (n == Vector3.zero)
        {
            fail = true;
            return n;
        }

        n = n.normalized;
        
        Plane p = new Plane(n, atom.transform.position);
        p.Raycast(r, out var d);
        Vector3 hitGlobal = r.GetPoint(d);
        Vector3 hitLocal = moleculeSpace.transform.InverseTransformPoint(hitGlobal);
        Vector3 currentLocal = atom.transform.localPosition;
        currentLocal[axis] = hitLocal[axis]; 
        return moleculeSpace.transform.TransformPoint(currentLocal);

    }

    public Vector3 PositionUnderMouseLineAndSphere(out bool fail)
    {
        fail = false;
        UpdateSliceNormal();
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        Vector3 fatherPos = atom.molecularParent.transform.position;

        Plane p = new Plane(sliceNormal, fatherPos);
        p.Raycast(r, out var d);
        Vector3 hitGlobal = r.GetPoint(d);
        
        return ProjectOnSphere(hitGlobal);

    }

    private Vector3 ProjectOnSphere(Vector3 p)
    {
        Vector3 father = atom.molecularParent.transform.position;
        float r = atom.distanceToMolecularParent;
        return father + (p - father).normalized * r;
    }

    public void UpdateSliceNormal()
    {
        sliceNormal = Vector3.Cross(WorldNormal(), atom.transform.position - atom.molecularParent.transform.position);
        if (sliceNormal.sqrMagnitude == 0) sliceNormal = Vector3.Cross(WorldNormal(), Vector3.right);
        if (sliceNormal.sqrMagnitude == 0) sliceNormal = Vector3.Cross(WorldNormal(), Vector3.up);

    }

    public void HighlightPlane(bool on)
    {
        if (on)
        {
            frontPlane.GetComponent<Renderer>().sharedMaterial = planeMaterials[1];
            backPlane.GetComponent<Renderer>().sharedMaterial = planeMaterials[1];
        }
        else
        {
            frontPlane.GetComponent<Renderer>().sharedMaterial = planeMaterials[0];
            backPlane.GetComponent<Renderer>().sharedMaterial = planeMaterials[0];
        }
            
    }
    
    public void HighlightLine(bool on)
    {
        if (on)
        {
            frontLine.GetComponent<Renderer>().sharedMaterial = lineMaterials[1];
            backLine.GetComponent<Renderer>().sharedMaterial = lineMaterials[1];
        }
        else
        {
            frontLine.GetComponent<Renderer>().sharedMaterial = lineMaterials[0];
            backLine.GetComponent<Renderer>().sharedMaterial = lineMaterials[0];
        }
            
    }
    
    public void HighlightCircle(bool on)
    {
        if (on)
        {
            frontDottedCircle.GetComponent<Renderer>().sharedMaterial = circleMaterials[1];
            backDottedCircle.GetComponent<Renderer>().sharedMaterial = circleMaterials[1];
        }
        else
        {
            frontDottedCircle.GetComponent<Renderer>().sharedMaterial = circleMaterials[0];
            backDottedCircle.GetComponent<Renderer>().sharedMaterial = circleMaterials[0];
        }
            
    }
    
    public void HighlightSlice(bool on)
    {
        if (on)
        {
            frontDottedSlice.GetComponent<Renderer>().sharedMaterial = sliceMaterials[1];
            backDottedSlice.GetComponent<Renderer>().sharedMaterial = sliceMaterials[1];
        }
        else
        {
            frontDottedSlice.GetComponent<Renderer>().sharedMaterial = sliceMaterials[0];
            backDottedSlice.GetComponent<Renderer>().sharedMaterial = sliceMaterials[0];
        }
    }
}
