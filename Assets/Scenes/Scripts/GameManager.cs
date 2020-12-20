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
        /*
        float newScaleX, newPosX, newScaleZ;
        newScaleX = curtain.transform.localScale.x;
        newScaleZ = curtain.transform.localScale.z;
        newPosX = curtain.transform.localPosition.x;
        //curtain.SetActive(false);
        while (curtain.transform.localScale.x >= 2f)
        {
            newScaleX -= 0.1f;
            newScaleZ += 0.1f;
            newPosX += 0.1f;
            curtain.transform.localScale = new Vector3(newScaleX, curtain.transform.localScale.y, newScaleZ);
            curtain.transform.localPosition = new Vector3(newPosX, curtain.transform.localPosition.y, curtain.transform.localPosition.z);
            //yield return new WaitForSeconds(0.001f);
            yield return new WaitForFixedUpdate();
        }
        */

        float t = 1;
        float newScaleX, newPosX, newScaleZ;
        newScaleX = curtain.transform.localScale.x;
        newScaleZ = curtain.transform.localScale.z;
        newPosX = curtain.transform.localPosition.x;
        while (t >= 0) 
        {
            float ease = easeInOutBack(t);

            newScaleX = Mathf.Lerp(2f, curtain.transform.localScale.x, ease);
            newScaleZ = Mathf.Lerp(10f, curtain.transform.localScale.z, ease);
            curtain.transform.localScale = new Vector3(newScaleX, curtain.transform.localScale.y, newScaleZ);
            newPosX = Mathf.Lerp(45f, curtain.transform.localPosition.x, ease);
            curtain.transform.localPosition = new Vector3(newPosX, curtain.transform.localPosition.y, curtain.transform.localPosition.z);

            t -= 0.02f;
            yield return new WaitForFixedUpdate();

        }



    }

    public void HideSolution()
    {
        StartCoroutine(CloseCurtains());
    }

    IEnumerator CloseCurtains()
    {
        /*
        float newScaleX, newPosX, newScaleZ;
        newScaleX = curtain.transform.localScale.x;
        newScaleZ = curtain.transform.localScale.z;
        newPosX = curtain.transform.localPosition.x;
        //curtain.SetActive(false);
        while (curtain.transform.localScale.x < 16f)
        {
            newScaleX += 0.1f;
            newScaleZ -= 0.1f;
            newPosX -= 0.1f;
            curtain.transform.localScale = new Vector3(newScaleX, curtain.transform.localScale.y, newScaleZ);
            curtain.transform.localPosition = new Vector3(newPosX, curtain.transform.localPosition.y, curtain.transform.localPosition.z);
            //yield return new WaitForSeconds(0.001f);
            yield return new WaitForFixedUpdate();

        }
        */

        float t = 0;
        float newScaleX, newPosX, newScaleZ;
        newScaleX = curtain.transform.localScale.x;
        newScaleZ = curtain.transform.localScale.z;
        newPosX = curtain.transform.localPosition.x;
        while (t <= 1)
        {
            float ease = easeInOutBack(t);

            newScaleX = Mathf.Lerp(curtain.transform.localScale.x, 16f, ease);
            newScaleZ = Mathf.Lerp( curtain.transform.localScale.z, 3f, ease);
            curtain.transform.localScale = new Vector3(newScaleX, curtain.transform.localScale.y, newScaleZ);
            newPosX = Mathf.Lerp(curtain.transform.localPosition.x, 33f, ease);
            curtain.transform.localPosition = new Vector3(newPosX, curtain.transform.localPosition.y, curtain.transform.localPosition.z);
            t += 0.02f;
            yield return new WaitForFixedUpdate();

        }

    }

    public float easeInOutBack(float x)
    {
        const float c1 = 1;//1.70158f;
        const float c2 = c1 * 1.525f;

        if (x < 0.5f)
        {
            return (Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2f;
        }
        else
        {
            return (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2f;
        }
    }


}
