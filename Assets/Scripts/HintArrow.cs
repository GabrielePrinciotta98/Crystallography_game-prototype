using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HintArrow : MonoBehaviour
{
    private Renderer _rendererTail;
    private Renderer _rendererHead;
    private float minDistance;
    public bool activated;
    private bool atomsChosed;
    private Atom chosenAtom;
    private Vector3 target;
    private static readonly Vector3 pivotPos = new Vector3(22, 6.6f, 10);
    private AtomsManager atomsManager;
    private LevelManager2 levelManager;
    private GameObject hintButton;
    private bool[] markedChosenAtoms;
    private bool[] markedTargets;
    private bool firstTime = true;
    private static readonly int _Color = Shader.PropertyToID("_Color");
    private static readonly int _EmissionColor = Shader.PropertyToID("_EmissionColor");

    // Start is called before the first frame update
    void Start()
    {
        _rendererTail = transform.GetChild(0).GetComponent<Renderer>();
        _rendererTail.enabled = false;
        _rendererHead = transform.GetChild(1).GetComponent<Renderer>();
        _rendererHead.enabled = false;
        levelManager = FindObjectOfType<LevelManager2>();
        hintButton = GameObject.Find("HintButton");

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("activated: " + activated);

        if (!activated) return;
        atomsManager = FindObjectOfType<AtomsManager>();


        if (!atomsChosed)
        {
            //SCEGLI L'ATOMO DA RISOLVERE E LA POSIZIONE TARGET
            //Debug.Log("atomsLength: " + (atomsManager.GetAtoms().Count));
            int range;
            do
            {
                range = Random.Range(0, atomsManager.GetAtoms().Count);
            } 
            while (markedChosenAtoms[range]);
                
            Debug.Log("i-esimo atomo scelto: " + range);
            chosenAtom = atomsManager.GetAtoms()[range];
            markedChosenAtoms[range] = true;
            float minDistance = float.MaxValue;
            float minDistance1 = float.MaxValue;
            float minDistance2 = float.MaxValue; 
            for (int i = 0; i < levelManager.SolAtomPos.Length; i++)
            {
                if (markedTargets[i]) continue;
                float d1 = Vector3.Distance(levelManager.SolAtomPos[i], chosenAtom.transform.localPosition);
                float d2 = Vector3.Distance(-levelManager.SolAtomPos[i], chosenAtom.transform.localPosition);
                float minDistanceTemp;
                if (d1 < minDistance1)
                {
                    //target = pivotPos + levelManager.SolAtomPos[i];
                    minDistance1 = d1;
                }

                if (d2 < minDistance2)
                {
                    minDistance2 = d2;
                }

                minDistanceTemp = minDistance1 < minDistance2 ? minDistance1 : minDistance2;

                if (!(minDistanceTemp < minDistance)) continue;
                minDistance = minDistanceTemp;
                target = pivotPos + levelManager.SolAtomPos[i];
                markedTargets[i] = true;
                Debug.Log("target changed: " + i);
            }

            Debug.Log("target: " + target);

            //this.transform.parent = chosenAtom.transform;
            this.transform.position = (chosenAtom.transform.position + target) / 2;
            this.transform.LookAt(target);
            //transform.Translate(Vector3.up);

            //transform.Rotate(90, 0, 0 );
            atomsChosed = true;
        }

        if (target == Vector3.zero) return;
        this.transform.position = (chosenAtom.transform.position + target) / 2;
        this.transform.LookAt(target);
        transform.Rotate(90, 0, 0);
        /*
                transform.localScale = new Vector3(transform.localScale.x,
                    Mathf.Abs(target.y - chosenAtom.transform.position.y) * 2f,
                    transform.localScale.z);
                */


        float distance = Vector3.Distance(target, chosenAtom.transform.position);
        transform.localScale = new Vector3(
            transform.localScale.x,
            distance / 2,
            transform.localScale.z 
        );
                
                
                
        _rendererTail.enabled = true;
        _rendererHead.enabled = true;
                
        _rendererTail.material.SetColor(_Color, Color.HSVToRGB(0.4f + -0.04f*Mathf.Clamp(distance, 0, 10), 1, 0.5f));
        _rendererTail.material.SetColor(_EmissionColor, Color.HSVToRGB(0.4f + -0.04f*Mathf.Clamp(distance, 0, 10), 1, 0.5f));

        _rendererHead.material.SetColor(_Color, Color.HSVToRGB(0.4f + -0.04f*Mathf.Clamp(distance, 0, 10), 1, 0.5f));
        _rendererHead.material.SetColor(_EmissionColor, Color.HSVToRGB(0.4f + -0.04f*Mathf.Clamp(distance, 0, 10), 1, 0.5f));

        //Debug.Log(Vector3.Distance(target, chosenAtom.transform.position));
        if (!(distance <= 0.3)) return;
        StartCoroutine(Snap(chosenAtom));
        chosenAtom.GetComponent<Collider>().enabled = false;
        chosenAtom.GetComponent<Atom>().SetSolved(true);
        chosenAtom.GetComponent<Atom>().ChangeMaterial(3);
        atomsManager.SetDraggingAtom(null);
        Debug.Log("done");
                    
        _rendererTail.enabled = false;
        _rendererHead.enabled = false;
        activated = false;
        atomsChosed = false;
        hintButton.GetComponent<Button>().interactable = true;
        //target = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        //Debug.Log("atomsChosed: " + atomsChosed);
        if (atomsChosed)
        {
            Gizmos.color = Color.green;
            //Gizmos.DrawWireSphere(chosenAtom.transform.position, 1f);
            //Gizmos.DrawLine(chosenAtom.transform.position, target);
            //Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(target, 1f);

        }
            
    }


    public void Activate()
    {
        activated = true;
        if (firstTime)
        {
            markedChosenAtoms = new bool[levelManager.SolAtomPos.Length];
            markedTargets = new bool[levelManager.SolAtomPos.Length];
            firstTime = false;
        }

        //hintButton.GetComponent<Button>().interactable = false;

        Debug.Log("activate");
    }
    
    IEnumerator Snap(Atom atom)
    {
        float t = 0;
        float ease, newPosX, newPosY, newPosZ;
        float posX = atom.transform.position.x,
            posY = atom.transform.position.y,
            posZ = atom.transform.position.z;
        while (t <= 1f)
        {
            ease = EaseInCubic(t);
            newPosX = Mathf.Lerp(posX, target.x, ease);
            newPosY = Mathf.Lerp(posY, target.y, ease);
            newPosZ = Mathf.Lerp(posZ, target.z, ease);
            atom.transform.position = new Vector3(newPosX, newPosY, newPosZ); 
            t += 0.2f;
            yield return new WaitForFixedUpdate();
        }
    }
    
    private float EaseInCubic(float x)
    {
        return x * x * x;
    }
}
