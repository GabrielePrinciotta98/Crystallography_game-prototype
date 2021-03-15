using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapManager : MonoBehaviour
{

    //private Material detectorMat;
    //private Material solDetectorMat;
    //private Material temp;
    private Detector detector;
    // Start is called before the first frame update
    void Start()
    {
        detector = GameObject.Find("Detector").GetComponent<Detector>();
        //detectorMat = GameObject.Find("Detector").GetComponent<Renderer>().material;
        //solDetectorMat = GameObject.Find("SolutionDetector").GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("ready");

        if (Input.GetKey("s"))
        {
            Debug.Log("s pressed");
            detector.Swap();
        }

        if (Input.GetKey("u"))
        {
            Debug.Log("u pressed");
            detector.UnSwap();
        }
    }
/*
    public void Swap()
    {
        temp = detectorMat;
        detectorMat = solDetectorMat;
        Debug.Log(detectorMat);
    }
    
    public void UnSwap()
    {
        detectorMat = temp;
    }
*/
    
}
