using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    public static int CurScore { get; set; }

    private void Start()
    {
        //scoreText = GetComponent<Text>();
    }

    /*
    private void Update()
    {
        scoreText.text = ScoreManager.Score.ToString();
    }
    */  

    public void DisplayScore()
    {
        scoreText.text = ScoreManager.Score.ToString();

    }

    public void UpdateScore(int basic, int bonus)
    {
        StartCoroutine(AddBonusScoreEffect(basic, bonus));
        
    }

    IEnumerator AddBonusScoreEffect(int a, int b)
    {
        
        yield return AddBasicScoreEffect(a);
        //Debug.Log("Bonus Score Effect: " + CurScore);

        scoreText.color = Color.red; //rosso: FF0707
        for (int i = CurScore + 1; i < CurScore + b + 1; i++)
        {
            scoreText.text = i.ToString();
            yield return new WaitForSeconds(0.000001f);
        }
        scoreText.color = Color.yellow; //giallo: F4E257
    }
    
    IEnumerator AddBasicScoreEffect(int a)
    {
        int cur = CurScore;
        //Debug.Log("Basic score effect: " + CurScore);
        for (int i = cur+1; i < cur + a + 1; i++)
        {
            scoreText.text = i.ToString();
            CurScore = i;
            yield return new WaitForSeconds(0.000001f);
        }
    }
    
    
}
