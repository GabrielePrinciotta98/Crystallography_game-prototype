using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    public static int CurScore { get; set; }
    

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
        for (int i = cur+1; i < cur + a + 1; i++)
        {
            scoreText.text = i.ToString();
            CurScore = i;
            yield return new WaitForSeconds(0.000001f);
        }
    }
    
    
}
