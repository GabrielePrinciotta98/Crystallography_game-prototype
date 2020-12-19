using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeReference] AtomsManager atomsManager;
    [SerializeReference] SolutionManager solutionManager;
    [SerializeReference] GameObject curtain;
    //HashSet<Vector4> atomPositions;
    //HashSet<Vector4> solutionPositions;
    //MyEqualityComparer comparer = new MyEqualityComparer();

    // Start is called before the first frame update
    void Start()
    {
       
    }

    /*
    void Update()
    {
        

        atomPositions = new HashSet<Vector4>(atomsManager.GetPositions(), comparer);
        solutionPositions = new HashSet<Vector4>(solutionManager.GetPositions(), comparer);
        for (int i = 0; i < atomsManager.GetAtoms().Count; i++)
        {
            if (Vector4.Distance(atomsManager.GetPositions()[i], solutionManager.GetPositions()[i]) < 2)
            {
                //curtain.SetActive(false);
                //solutionManager.ShowAtoms();
                print("Hai vinto!");
            }
        }
        /*
        if (atomPositions.SetEquals(solutionPositions))
        {
            curtain.SetActive(false);
            print("Hai vinto!");
        }
        
    }
    */

    public void ShowSolution()
    {
        StartCoroutine(OpenCurtains());
        //solutionManager.ShowAtoms();

    }

    IEnumerator OpenCurtains()
    {
        float newScaleX, newPosX;
        newScaleX = curtain.transform.localScale.x;
        newPosX = curtain.transform.localPosition.x;
        //curtain.SetActive(false);
        while (curtain.transform.localScale.x >= 2f)
        {
            newScaleX -= 0.1f;
            newPosX += 0.1f;
            curtain.transform.localScale = new Vector3(newScaleX, curtain.transform.localScale.y, curtain.transform.localScale.z);
            curtain.transform.localPosition = new Vector3(newPosX, curtain.transform.localPosition.y, curtain.transform.localPosition.z);
            yield return new WaitForSeconds(0.005f);
        }
    }

    public void HideSolution()
    {
        StartCoroutine(CloseCurtains());
    }

    IEnumerator CloseCurtains()
    {
        float newScaleX, newPosX;
        newScaleX = curtain.transform.localScale.x;
        newPosX = curtain.transform.localPosition.x;
        //curtain.SetActive(false);
        while (curtain.transform.localScale.x <= 18f)
        {
            newScaleX += 0.1f;
            newPosX -= 0.1f;
            curtain.transform.localScale = new Vector3(newScaleX, curtain.transform.localScale.y, curtain.transform.localScale.z);
            curtain.transform.localPosition = new Vector3(newPosX, curtain.transform.localPosition.y, curtain.transform.localPosition.z);
            yield return new WaitForSeconds(0.005f);
        }
    }
}
