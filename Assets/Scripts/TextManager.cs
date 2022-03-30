using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Text zoom;
    private Text power;
    private Text rotation;
    private Text lambda;
    private Text repetitions;


    public void SetZoomTextReference(Text text)
    {
        zoom = text;
    }
    
    public void SetZoomText(float zoom)
    {
        this.zoom.text = zoom.ToString("F2");
    }

    public void SetPowerTextReference(Text text)
    {
        power = text;
    }
    
    public void SetPowerText(float pwr)
    {
        power.text = Mathf.Pow(2, pwr).ToString("F2");
    }

    public void SetRotationTextReference(Text text)
    {
        rotation = text;
    }
    
    public void SetRotationText(float rotation)
    {
        this.rotation.text = rotation.ToString("F0");
    }

    public void SetLambdaTextReference(Text text)
    {
        lambda = text;
    }
    
    public void SetLambdaText(float l)
    {
        lambda.text = l.ToString("F2");
    }

    public void SetRepetitionsTextReference(Text text)
    {
        repetitions = text;
    }
    
    public void SetRepetitionsText(float r)
    {
        repetitions.text = r.ToString("F0");
    }
    
    

}
