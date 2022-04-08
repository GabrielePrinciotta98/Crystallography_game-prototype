using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Sprite[] hourglassesSprites;
    private Text timerValue;
    private Image hourglassImage;
    
    public static float Time { get; set; }

    void Start()
    {
        timerValue = transform.GetChild(0).GetComponent<Text>();
        hourglassImage = transform.GetChild(1).GetComponent<Image>();
    }


    void Update()
    {
        timerValue.text = $"{Time:00}";
        hourglassImage.sprite = Time switch
        {
            >= 200 => hourglassesSprites[0],
            >= 100 => hourglassesSprites[1],
            >= 0 => hourglassesSprites[2],
            _ => hourglassImage.sprite
        };
    }
}
