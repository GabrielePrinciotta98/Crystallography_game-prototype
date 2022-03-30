using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Atom : MonoBehaviour
{
    private GameObject magneticField;
    public Material[] materials;
    private ControlGizmo controlGizmo;
    private DottedLine dottedLineHoriz;
    private DottedLine dottedLineVert;
    private Renderer dottedLineHorizRenderer;
    private Renderer dottedLineVertRenderer;
    private Material dottedLineVertMaterial;
    private Vector3 controlPlanePosition;
    private Renderer _renderer;
    private AtomsManager atomsManager;
    private MoleculeManager moleculeManager;
    private LevelManager levelManager;
    private Collider _collider;
    private Detector detector;
    public Vector3 PositionFromPivot { get; set; }

    private Vector3 lastMousePos;
    private bool selected;

    public GameObject molecularParent;
    public List<Atom> molecularChildren;
    public List<GameObject> crystalReplicas;
    public GameObject cell;
    private List<Cell> cells;
    
    public float distanceToMolecularParent;
    public bool Snapped { get; set; }  
    public bool solved;
    private bool dragged;

    private HintArrow hintArrow;
    private static readonly int _EmissionColor = Shader.PropertyToID("_EmissionColor");


    private void Start()
    {
        magneticField = transform.GetChild(1).gameObject;
        Physics.IgnoreCollision(GetComponent<SphereCollider>(), magneticField.GetComponent<SphereCollider>(), true);
        detector = FindObjectOfType<Detector>();
        hintArrow = FindObjectOfType<HintArrow>();
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = true;
        ChangeMaterial(0);

        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        
        atomsManager = FindObjectOfType<AtomsManager>();
        moleculeManager = FindObjectOfType<MoleculeManager>();
        levelManager = FindObjectOfType<LevelManager>();

        
        controlGizmo = FindObjectOfType<ControlGizmo>();
        controlGizmo.DisableElements();

        dottedLineHoriz = GameObject.Find("DottedLineHoriz").GetComponent<DottedLine>();
        dottedLineHorizRenderer = dottedLineHoriz.GetComponent<Renderer>();
        dottedLineHorizRenderer.enabled = false;

        dottedLineVert = GameObject.Find("DottedLineVert").GetComponent<DottedLine>();
        dottedLineVertRenderer = dottedLineVert.GetComponent<Renderer>();
        dottedLineVertMaterial = dottedLineVertRenderer.material;
        dottedLineVertMaterial.SetColor(_EmissionColor, Color.red);

        dottedLineVertRenderer.enabled = false;

        atomsManager.AddAtom(this); 
    }
    
    private void Update()
    {
        _collider.enabled = atomsManager.GameStart;
        
        //INVIO DELLA POSIZIONE CORRENTE ALL'ATOMS MANAGER
        
        PositionFromPivot = transform.position - new Vector3(22, 6.6f, 10); // IL LIVELLO DIVENTA IRRISOLVIBILE SE RUOTO 
        //PositionFromPivot = transform.localPosition - Vector3.zero; 
        //Debug.Log($"{PositionFromPivot}= {transform.localPosition} - {Vector3.zero}");
        //Debug.Log(transform.localPosition);

        if (atomsManager.crystalActivated)
        {
            if (atomsManager.centralCellAtom == this)
                atomsManager.SetMyPosition(this);
            EnforceInsideCell();
        }
        else
        {
            atomsManager.SetMyPosition(this);
            EnforceInsideWorkspace();
        }
    }
    

    private void OnMouseOver()
    {
        if (dragged) return;
        bool hintArrowActivated = hintArrow.activated && this == hintArrow.chosenAtom;
        ChangeMaterial(hintArrowActivated ? 6 : 2); 
    }

    private void OnMouseExit()
    {
        if (solved)
        {
            ChangeMaterial(3);
            return;
        }
        bool hintArrowActivated = hintArrow.activated && this == hintArrow.chosenAtom;
        ChangeMaterial(hintArrowActivated ? 4 : 0);
    }

    private void OnMouseDown()
    {
        if (solved) return;
        atomsManager.SetDraggingAtom(this);
        //magneticField.GetComponent<SphereCollider>().isTrigger = true;
        //magneticField.GetComponent<Rigidbody>().isKinematic = false;

        //blocca la posizione di tutti gli atomi eccetto quello che si sta trascinando
        atomsManager.FreezeAtoms();
        
        controlGizmo.EnableElements();
        
        controlGizmo.SetAtom(this);

        if (atomsManager.crystalActivated)
            foreach (var c in cells)
                c.ChangeAlpha(transform.position);

        bool hintArrowActivated = hintArrow.activated && this == hintArrow.chosenAtom;
        ChangeMaterial(hintArrowActivated ? 6 : 2);
        SetSelected(true);
    }

    private bool ModifierActive()
    {
        bool result = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift );
        result = result && atomsManager.LevelType.Equals("XYZ");
        return result;
    }
    
    private void OnMouseDrag()
    {
        if (solved) return;
        bool hintArrowActivated = hintArrow.activated && this == hintArrow.chosenAtom;
        ChangeMaterial(hintArrowActivated ? 5 : 1);
        dragged = true;
        levelManager.anAtomWasDragged = true;
        Vector3 newPos;
        
        
        bool fail;
        if (ModifierActive())
        {
            if (moleculeManager.Activated)
            {
                newPos = controlGizmo.PositionUnderMouseLineAndSphere(out fail);
                controlGizmo.HighlightSlice(true);
                controlGizmo.HighlightCircle(false);
            }
            else
            {
                newPos = controlGizmo.PositionUnderMouseLine(out fail);
                controlGizmo.HighlightLine(true);
                controlGizmo.HighlightPlane(false);
            }
        }
        else
        {
            if (moleculeManager.Activated)
            {
                newPos = controlGizmo.PositionUnderMousePlaneAndSphere(out fail);
                controlGizmo.HighlightSlice(false);
                controlGizmo.HighlightCircle(true);
            }
            else
            {
                newPos = controlGizmo.PositionUnderMousePlane(out fail);
                controlGizmo.HighlightLine(false);
                controlGizmo.HighlightPlane(true);
            }
        }
        
        if (fail) return;
        
        Vector3 offset = newPos - transform.position;
        transform.position = newPos;

        
        if (atomsManager.crystalActivated)
        {
            AddOffsetToReplicas(offset);
            EnforceInsideCell();
        }
        
        
        if (moleculeManager.Activated)
        {
            AddOffsetToChildren(offset);
            controlGizmo.RefreshPositionDottedCircle();
            controlGizmo.RefreshPositionSlice();
        }
            

        EnforceInsideWorkspace();
        controlGizmo.RefreshPositionLine();
        controlGizmo.RefreshPositionPlane();
        
        detector.SetDirty();
    }
    
    
    private void OnMouseUp()
    {
        SetSelected(false);
        dragged = false;
        dottedLineVert.SetAtom(null, false);
        dottedLineVertRenderer.enabled = false;
        dottedLineHorizRenderer.enabled = false;

        controlGizmo.DisableElements();

        //magneticField.GetComponent<SphereCollider>().isTrigger = false;
        //magneticField.GetComponent<Rigidbody>().isKinematic = true;
        //SBLOCCA LA POSIZIONE DI TUTTI GLI ATOMI PERCHè SI HA SMESSO DI TRASCINARE
        atomsManager.UnFreezeAtoms();
        
        if (!solved)
        {
            bool hintArrowActivated = hintArrow.activated && this == hintArrow.chosenAtom;
            ChangeMaterial(hintArrowActivated ? 4 : 0);
        }
        
        if (!atomsManager.crystalActivated) return;
        foreach (var c in cells)
            c.ResetAlpha();
    }
    
    //fa muovere l'intero sotto-albero di un atomo
    private void AddOffsetToChildren(Vector3 offset)
    {
        foreach (var child in molecularChildren)
        {
            child.transform.position = child.transform.position + offset;
            child.AddOffsetToChildren(offset);
        }
    }
    
    public void AddOffsetToReplicas(Vector3 offset)
    {
        foreach (var replica in crystalReplicas)
        {
            if (replica != gameObject)
                replica.transform.position += offset;
        }
    }
    

    private void EnforceInsideWorkspace()
    {
        Vector3 pos = transform.localPosition;
        pos = new Vector3(Mathf.Clamp(pos.x, -8, 8),
            Mathf.Clamp(pos.y, -8, 8),
            Mathf.Clamp(pos.z, -8, 8));
        transform.localPosition = pos;
    }

    public void CrystalActivation(List<GameObject> newCrystalAtoms, List<Cell> cellComponents)
    {
        crystalReplicas = newCrystalAtoms;
        cell = atomsManager.GetMyCell(gameObject);
        cells = cellComponents;
    }
    
    private void EnforceInsideCell()
    {
        Vector3 cellPosition = cell.transform.localPosition;
        Vector3 pos = transform.localPosition;
        pos = new Vector3(Mathf.Clamp(pos.x, cellPosition.x - 2, cellPosition.x + 2),
            Mathf.Clamp(pos.y, cellPosition.y - 2, cellPosition.y + 2),
            Mathf.Clamp(pos.z, cellPosition.z - 2, cellPosition.z + 2));
        transform.localPosition = pos;
    }
    
    
    public void SetSelected(bool flag)
    {
        selected = flag;
    }

    public bool GetSelected()
    {
        return selected;
    }
    

    public void ChangeMaterial(int i)
    {
        _renderer.sharedMaterial = materials[i];
    }

    public void SetSolved(bool flag)
    {
        solved = flag;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (magneticField.CompareTag(other.tag) && CompareTag("MagneticField"))
            Debug.Log("collisione");
    }
}