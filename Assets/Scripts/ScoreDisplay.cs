using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    private void Update()
    {
        GetComponent<Text>().text = ScoreManager.Score.ToString();
    }
}
