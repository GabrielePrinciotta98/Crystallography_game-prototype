using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSolution : MonoBehaviour
{
    private SolutionManager solutionManager;
    private Material mat;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private readonly Color startingAlpha = new Color(1, 1, 1, 0.1176471f);

    public GameObject atom;
    public GameObject pivotRep;
    public CentralCellSolution centralCell;
    private Vector3 pivotPos;
    private SolutionAtom[] centralCellAtoms;
    private Vector3[] atomSpawnPositions = new Vector3[8];
    private readonly List<GameObject> atoms = new List<GameObject>();


    void Awake()
    {
        solutionManager = FindObjectOfType<SolutionManager>();
        centralCell = FindObjectOfType<CentralCellSolution>();
        
    }
    
    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.color = new Color(1, 1, 1, 0); 
        pivotPos = transform.position;
        centralCellAtoms = centralCell.GetAtoms();
        var pivotTemp = Instantiate(pivotRep, pivotPos, Quaternion.identity, centralCell.transform);
        pivotTemp.transform.localScale /= solutionManager.GetK();
        atoms.Add(pivotTemp);
        InstantiateAtoms();
    }

    private void InstantiateAtoms()
    {
        atomSpawnPositions = solutionManager.GetAtomSpawnPositions();

        for (int i = 0; i < solutionManager.GetN()-1; i++)
        {
            if (atomSpawnPositions[i] == Vector3.zero) continue;
            var atomTemp = Instantiate(atom, pivotPos + atomSpawnPositions[i],
                Quaternion.identity, centralCellAtoms[i].transform);
            atoms.Add(atomTemp);
        }
    }

    public void ShowCell()
    {
        StartCoroutine(FlashOut(5, 1, 5f));
    }
    
    //animazione fade-in per comparire in scena
    IEnumerator Fade(Color start, Color end, float duration)
    {
        for (float t = 0; t <= duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            mat.color = Color.Lerp(start, end, normalizedTime);
            yield return null;
        }
        mat.color = end;
    }
    
    IEnumerator Flash(float intensityStart, float intensityEnd, float duration)
    {
        for (float t = 0; t <= duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            float intensity = Mathf.Lerp(intensityStart, intensityEnd, EaseOutBack(normalizedTime));
            mat.SetColor(EmissionColor, new Color(0.6f, 0.6f, 0.6f, 1) * intensity);
            yield return null;
        }
        
        mat.SetColor(EmissionColor, new Color(0.6f, 0.6f, 0.6f, 1)*intensityEnd);

    }
    IEnumerator FlashOut(float intensityStart, float intensityEnd, float duration)
    {
        yield return StartCoroutine(Flash(0, 5, 0.5f));

        for (float t = 0; t <= duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            float intensity = Mathf.Lerp(intensityStart, intensityEnd, EaseOutExpo(normalizedTime));
            mat.SetColor(EmissionColor, new Color(0.6f, 0.6f, 0.6f, 1)*intensity);
            mat.color = Color.Lerp(startingAlpha, new Color(1, 1, 1, 0), EaseOutQuad(normalizedTime));
            yield return null;
        }
        
        mat.SetColor(EmissionColor, new Color(0.6f, 0.6f, 0.6f, 1)*intensityEnd);
        mat.color = new Color(1, 1, 1, 0);


    }

    public void ChangeAlpha(Vector3 atomPosition)
    {
        float distance = Vector3.Distance(transform.position, atomPosition);
        float newAlpha = (float) (0.4f - Math.Pow(distance / 1000, 0.25f));
        Color finalColor = new Color(1, 1, 1, newAlpha);
        StartCoroutine(Fade(mat.color, finalColor, 0.1f));
    }
    
    public void ResetAlpha()
    {
        Color finalColor = new Color(1, 1, 1, 0);
        StartCoroutine(Fade(mat.color, finalColor, 0.1f));
    }
    

    private float EaseOutBack(float x) 
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;

        return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
    }
    
    private float EaseOutExpo(float x)
    {
        return Math.Abs(x - 1) < 0.0001f ? 1 : 1 - Mathf.Pow(2, -10 * x);
    } 
    
    private float EaseOutQuad(float x){
        return 1 - (1 - x) * (1 - x);
    }
}
