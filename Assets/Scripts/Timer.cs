using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Sprite[] hourglassesSprites;
    private Text timerValue;
    private Image hourglassImage;
    
    public static float Time { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        timerValue = transform.GetChild(0).GetComponent<Text>();
        hourglassImage = transform.GetChild(1).GetComponent<Image>();
    }


    // Update is called once per frame
    void Update()
    {
        timerValue.text = string.Format("{0:00}", Time);
        if (Time >= 200)
        {
            hourglassImage.sprite = hourglassesSprites[0];
        }
        else if (Time >= 100)
        {
            hourglassImage.sprite = hourglassesSprites[1];
        }
        else if (Time >= 0)
        {
            hourglassImage.sprite = hourglassesSprites[2];
        }
        
    }
}
