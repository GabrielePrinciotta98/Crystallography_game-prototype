using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HintArrow : MonoBehaviour
{
    private Renderer _rendererTail;
    private Renderer _rendererHead;
    public bool used;
    public bool activated;
    private bool atomsChosed;
    private bool targetCloseToAnotherAtom;
    public Atom chosenAtom;
    private Vector3 target;
    private static readonly Vector3 PivotPos = new Vector3(22, 6.6f, 10);
    private AtomsManager atomsManager;
    private LevelManager levelManager;
    private SolutionManager solutionManager;
    private MoleculeManager moleculeManager;
    private AudioManager audioManager;
    private GameObject hintButton;
    private bool[] markedChosenAtoms;
    private bool[] markedTargets;
    private bool firstTime = true;
    private static readonly int _Color = Shader.PropertyToID("_Color");
    private static readonly int _EmissionColor = Shader.PropertyToID("_EmissionColor");
    private int j = -1;
    private float snapThreshold;

    // Start is called before the first frame update
    void Start()
    {
        _rendererTail = transform.GetChild(0).GetComponent<Renderer>();
        _rendererTail.enabled = false;
        _rendererHead = transform.GetChild(1).GetComponent<Renderer>();
        _rendererHead.enabled = false;
        levelManager = FindObjectOfType<LevelManager>();
        hintButton = GameObject.Find("HintButton");
        snapThreshold = SnapThreshold(LevelLoader.LevelCounter);
        moleculeManager = FindObjectOfType<MoleculeManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private float SnapThreshold(int levelCounter)
    {
        switch (levelCounter)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                return 0.6f;
            case 7:
            case 8:
            case 9:
                return 0.8f;
            default:
                return 0.8f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!activated) return;
        
        if (!atomsChosed)
        {
            //SCEGLI L'ATOMO DA RISOLVERE E LA POSIZIONE TARGET
            ChooseAtomToSolve();
            ChooseTarget();
        }
        target = new Vector3(solutionManager.GetPositions()[j].x, 
            solutionManager.GetPositions()[j].y, 
            solutionManager.GetPositions()[j].z) + PivotPos;
        Debug.Log("target: " + target);
        

        /*Debug.Log("markedTarget = " + String.Join(", ",
            new List<bool>(markedTargets)
                .ConvertAll(i => i.ToString())
                .ToArray()));*/
        
        if (target == Vector3.zero) return;
        transform.position = (chosenAtom.transform.position + target) / 2;
        transform.LookAt(target);
        transform.Rotate(90, 0, 0);

        float distance = ChangeArrowAppearance();

        //Debug.Log(Vector3.Distance(target, chosenAtom.transform.position));
        if (!(distance <= snapThreshold)) return;

        int atomsToSolve = markedTargets.Count(t => t == false);
        if (atomsToSolve > 0)
        {
            audioManager.PlayOneShot("Snap");
            StartCoroutine(Snap(chosenAtom));
            chosenAtom.GetComponent<Collider>().enabled = false;
            chosenAtom.GetComponent<Atom>().SetSolved(true);
            chosenAtom.GetComponent<Atom>().ChangeMaterial(3);
            //Debug.Log("done");
                    
        }
        EnableArrowRendering(false);
        activated = false;
        atomsChosed = false;
        hintButton.GetComponent<Button>().interactable = true;
        //target = Vector3.zero;
    }
    

    private void ChooseAtomToSolve()
    {
        int range;
        do
        {
            range = Random.Range(0, atomsManager.GetAtoms().Count);
        } while (markedChosenAtoms[range] || !CheckSolvedParent(range));

        //Debug.Log("i-esimo atomo scelto: " + range);
        chosenAtom = atomsManager.GetAtoms()[range];
        chosenAtom.ChangeMaterial(4);
        markedChosenAtoms[range] = true;
        //Debug.Log("sol: " + CheckSolvedParent(range));

    }
    
    private bool CheckSolvedParent(int range)
    {
        if (!moleculeManager.Activated) return true;
        GameObject father = atomsManager.GetAtoms()[range].molecularParent;
        bool parentIsSolved = father.GetComponent<Atom>() != null && father.GetComponent<Atom>().solved;
        bool parentIsPivot = father.CompareTag("PivotAtom");
        return parentIsSolved || parentIsPivot;
    }

    private void ChooseTarget()
    {
        //float minDistance = float.MaxValue;
        //float minDistance1 = float.MaxValue;
        //float minDistance2 = float.MaxValue; 
        for (int i = 0; i < levelManager.SolAtomPos.Length; i++)
        {
            if (markedTargets[i]) continue;

            
            if (moleculeManager.Activated)
            {
                int nMolecularChildrenChosen = chosenAtom.molecularChildren.Count;
                int nMolecularChildrenTarget = solutionManager.GetAtoms()[i].molecularChildren.Count;
                bool sameNumberOfChildren = nMolecularChildrenChosen == nMolecularChildrenTarget;
                GameObject fatherChosen = chosenAtom.molecularParent;
                GameObject fatherTarget = solutionManager.GetAtoms()[i].molecularParent;
                bool parentIsPivot = fatherChosen.CompareTag("PivotAtom") && fatherTarget.CompareTag("PivotAtomSol");
                if (sameNumberOfChildren || parentIsPivot)
                {
                    if (Math.Abs(chosenAtom.distanceToMolecularParent - solutionManager.GetAtoms()[i].distanceToMolecularParent) > 0.2f)
                        continue;
                }
                else
                    continue;
            }
 
            //IL CHECK SULLA VICINANZA DI UN ALTRO ATOMO HA SENSO SOLO SENZA MOLECULE MODE
            if (!moleculeManager.Activated)
            {
                targetCloseToAnotherAtom = false;
                for (int k = 0; k < atomsManager.GetAtoms().Count; k++)
                {
                    if (atomsManager.GetAtoms()[k] == chosenAtom) continue;
                    float distance1 = Vector3.Distance(solutionManager.GetPositions()[i], atomsManager.GetPositions()[k]);
                    float distance2 = Vector3.Distance(-solutionManager.GetPositions()[i], atomsManager.GetPositions()[k]);
                    if (distance1 <= 1f || distance2 <= 1f)
                    {
                        targetCloseToAnotherAtom = true;
                        break;
                    }
                }
            
                if (targetCloseToAnotherAtom) continue;
            }
            /*
            float d1 = Vector3.Distance(solutionManager.GetPositions()[i], chosenAtom.transform.localPosition);
            float d2 = Vector3.Distance(-solutionManager.GetPositions()[i], chosenAtom.transform.localPosition);
            
            
            if (d1 < minDistance)
            {
                minDistance1 = d1;
            }
                
            if (d2 < minDistance2)
            {
                minDistance2 = d2;
            }
                
            float minDistanceTemp = minDistance1 < minDistance2 ? minDistance1 : minDistance2;

            //if (!(minDistanceTemp < minDistance)) continue;
            minDistance = minDistanceTemp;*/
            j = i;
            //Debug.Log("target changed: " + i);
        }
            
        atomsChosed = true;
        Debug.Log("j: " + j);
        markedTargets[j] = true;
        
    }

    private float ChangeArrowAppearance()
    {
        var position = chosenAtom.transform.position;
        transform.position = (position + target) / 2;
        transform.LookAt(target);
        transform.Rotate(90, 0, 0);
        float distance = Vector3.Distance(target, position);
        var localScale = transform.localScale;
        localScale = new Vector3(localScale.x, distance / 2, localScale.z);
        transform.localScale = localScale;

        EnableArrowRendering(true);

        _rendererTail.material.SetColor(_Color, Color.HSVToRGB(0.4f + -0.04f * Mathf.Clamp(distance, 0, 10), 1, 0.5f));
        _rendererTail.material.SetColor(_EmissionColor,
            Color.HSVToRGB(0.4f + -0.04f * Mathf.Clamp(distance, 0, 10), 1, 0.5f));

        _rendererHead.material.SetColor(_Color, Color.HSVToRGB(0.4f + -0.04f * Mathf.Clamp(distance, 0, 10), 1, 0.5f));
        _rendererHead.material.SetColor(_EmissionColor,
            Color.HSVToRGB(0.4f + -0.04f * Mathf.Clamp(distance, 0, 10), 1, 0.5f));
        return distance;
    }

    public void EnableArrowRendering(bool flag)
    {
        _rendererTail.enabled = flag;
        _rendererHead.enabled = flag;
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
            atomsManager = FindObjectOfType<AtomsManager>();
            solutionManager = FindObjectOfType<SolutionManager>();
            markedChosenAtoms = new bool[levelManager.SolAtomPos.Length];
            markedTargets = new bool[levelManager.SolAtomPos.Length];
            firstTime = false;
            used = true;
        }

        hintButton.GetComponent<Button>().interactable = false;

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
            ease = EaseInQuartic(t);
            newPosX = Mathf.Lerp(posX, target.x, ease);
            newPosY = Mathf.Lerp(posY, target.y, ease);
            newPosZ = Mathf.Lerp(posZ, target.z, ease);
            atom.transform.position = new Vector3(newPosX, newPosY, newPosZ); 
            t += 0.2f;
            yield return new WaitForFixedUpdate();
        }
    }
    
    private float EaseInQuartic(float x)
    {
        return x * x * x * x;
    }
}
