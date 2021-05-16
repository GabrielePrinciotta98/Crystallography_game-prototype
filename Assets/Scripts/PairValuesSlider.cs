using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PairValuesSlider : MonoBehaviour
{
    Slider sliderComponent;
    float old;
 
    void Start () {
        sliderComponent = GetComponent<Slider> ();
    }
     
    void Update ()
    {
        if (sliderComponent.value % 2 == 0)
        {
            if (sliderComponent.value < old)
                sliderComponent.value++;
            else
                sliderComponent.value--;
            
            old = sliderComponent.value;
        }

        if (sliderComponent.value < 0)
            sliderComponent.value = 0;
    }
}
