using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeReference] private Text zoom;
    [SerializeReference] private Text power;
    [SerializeReference] private Text rotation;
    [SerializeReference] private Text repetitions;
    [SerializeReference] private Text lambda;

    
    public void SetZoomText(float zoom)
    {
        this.zoom.text = zoom.ToString();
    }

    public void SetPowerText(float pwr)
    {
        power.text = pwr.ToString();
    }

    public void SetRotationText(float rotation)
    {
        this.rotation.text = rotation.ToString();
    }

    public void SetRepsText(float reps)
    {
        repetitions.text = reps.ToString();
    }

    public void SetLambdaText(float l)
    {
        lambda.text = l.ToString();
    }

}
