using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelManager2 : MonoBehaviour
{
    [SerializeReference] private HUDManager hudManager;
    [SerializeReference] private AtomsManager atomsManager;
    [SerializeReference] private SolutionManager solutionManager;
    [SerializeReference] private GameObject youWonUI;
    [SerializeReference] private GameObject curtain;
    [SerializeReference] private GameObject youWonBackGroundUI;
    [SerializeReference] private GameObject addedScoreText;
    [SerializeReference] private GameObject addedBonusScoreText;
    [SerializeReference] private GameObject controlPlane;
    [SerializeReference] private GameObject dottedLine;
    [SerializeReference] private GameObject levelIndicatorBackGroundUI;
    [SerializeReference] private GameObject levelIndicatorText;
    [SerializeReference] private GameObject nextLevelButton;
    [SerializeReference] private GameObject backButton;
    private EmitterCone emitterCone;
    private EmitterConeSol emitterConeSol;
    private int frames;
    private Vector3 atomPos;
    private Vector3[] solAtomPos;
    private bool[] markedSolAtomPos;
    private int n;
    private bool over = false;
    private bool isSymmetric;
    private float haussdorfThreshold = 0.9f;

    private ScoreDisplay scoreDisplay;
    
    private float time = 300; //in secondi
    private Timer timer;
    private bool updateTime;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Scena corrente:" + (SceneManager.GetActiveScene().buildIndex-1));
        Debug.Log("Lv counter: " + LevelLoader.LevelCounter);
        Level lv = LevelData.Levels[LevelLoader.LevelCounter-1];
        //Debug.Log(lv.SolPositions);
        InstantiateAtomsManager(lv.R,lv.M,lv.N, lv.IsCrystal, lv.Plane);
        InstantiateSolutionManager(lv.R,lv.M,lv.N, lv.IsCrystal, lv.SolPositions);
        Instantiate(hudManager);

        FindObjectOfType<Detector>().SetAtomsManager(atomsManager);
        FindObjectOfType<SolutionDetector>().SetSolutionManager(solutionManager);
        
        solAtomPos = lv.SolPositions;
        markedSolAtomPos = new bool[solAtomPos.Length];
        for (int i = 0; i < markedSolAtomPos.Length; i++)
            markedSolAtomPos[i] = false;
        n = lv.N-1; // n sono gli atomi ancora da risolvere

        Instantiate(controlPlane);
        Instantiate(dottedLine);
        levelIndicatorText.GetComponent<Text>().text = lv.Description;
        nextLevelButton.GetComponent<Button>().onClick.AddListener(LevelLoader.LoadNextLevel);
        backButton.GetComponent<Button>().onClick.AddListener(LevelLoader.LoadMenu);

        emitterCone = FindObjectOfType<EmitterCone>();
        emitterConeSol = FindObjectOfType<EmitterConeSol>();

        scoreDisplay = FindObjectOfType<ScoreDisplay>();
        scoreDisplay.DisplayScore();
        
        Timer timer = FindObjectOfType<Timer>();
        Timer.Time = time;

        StartCoroutine(StartLevel());
    }

    private void InstantiateSolutionManager(int r, int m, int n, bool isCrystal, Vector3[] listPos)
    {
        solutionManager = Instantiate(solutionManager);
        solutionManager.SetCrystal(isCrystal);
        solutionManager.SetR(r);
        solutionManager.SetM(m);
        solutionManager.SetN(n);
        solutionManager.SetAtomSpawnPositions(listPos);
    }

    private void InstantiateAtomsManager(int r, int m, int n, bool isCrystal, string plane)
    {
        atomsManager = Instantiate(atomsManager);
        atomsManager.SetCrystal(isCrystal);
        atomsManager.SetR(r);
        atomsManager.SetM(m);
        atomsManager.SetN(n);
        atomsManager.Plane = plane;
    }
    

    void Update()
    {
        
        if (updateTime && !over)
        {
            time -= Time.deltaTime;
            Timer.Time = time;
        }
        if (!solutionManager.GetStop())
        {
            solAtomPos = solutionManager.Positions3.ToArray();
        }
        if (frames == 20 && !over)
        {
            //Debug.Log("sol: " + solutionManager.SumPos);
            //Debug.Log("atom: " + atomsManager.SumPos);
            //Debug.Log("distance: " + Vector4.Distance(atomsManager.SumPos, solutionManager.SumPos));
            float h = Haussdorf();
            //Debug.Log("Hausdorff: " + h);
            if (h <= haussdorfThreshold)
            {
                if (isSymmetric)
                {
                    solutionManager.ChangeToSymmetric();
                    //Debug.Log("simmetrico");
                }

                over = true;
                curtain.GetComponent<Curtain>().ShowSolution();
                //atomsManager.AnAtomIsMoving = true;
                StartCoroutine(Victory());
                ScoreDisplay.CurScore = ScoreManager.Score;
                ScoreManager.TimeBonus += (int)time;
                ScoreManager.Score += 100;
                
                print("hai vinto");
            }
            
            frames = 0;
        }
        
        frames++;
    }

    private float Haussdorf()
    {
        float h1 = 0;
        float h2 = 0;
        for (int i = 0; i < solAtomPos.Length; i++)
        {
            float shortest1 = float.MaxValue;
            float shortest2 = float.MaxValue;

            for (int j = 0; j < solAtomPos.Length; j++)
            {
                float d1 = Vector3.Distance(atomsManager.GetAtoms()[i].transform.localPosition, solAtomPos[j]);
                float d2 = Vector3.Distance(atomsManager.GetAtoms()[i].transform.localPosition, -solAtomPos[j]);
                if (d1 < shortest1)
                    shortest1 = d1;
                if (d2 < shortest2)
                    shortest2 = d2;            
            }

            if (shortest1 > h1)
                h1 = shortest1; //h1 = Hausdorff rispetto alla soluzione base
            if (shortest2 > h2)
                h2 = shortest2; //h2 = Hausdorff rispetto alla soluzione simmetrica
        }
        float H = Mathf.Min(h1,h2);
        isSymmetric = Math.Abs(H - h2) < 0.00001f;
        
        return H;
    }

    IEnumerator StartLevel()
    {

        yield return new WaitForSeconds(0.8f);
        levelIndicatorBackGroundUI.SetActive(true);
        //StartCoroutine(EnterLevelIndicator(levelIndicatorText));
        //yield return new WaitForSeconds(2);
        yield return StartCoroutine(ExitLevelIndicator(levelIndicatorText));
        
        //yield return new WaitForSeconds(1);
        levelIndicatorBackGroundUI.SetActive(false);
        atomsManager.GameStart = true;
        emitterCone.GameStart = true;
        emitterConeSol.GameStart = true;
        updateTime = true;


    }

    IEnumerator EnterLevelIndicator(GameObject levelIndicatorText)
    {
        
        float t = 0;
        float ease, newPosX;
        while (t <= 1f)
        {
            ease = EaseOutQuartic(t);
            newPosX = Mathf.Lerp(-1916, 0, ease);
            this.levelIndicatorText.transform.localPosition = new Vector3(newPosX, levelIndicatorText.transform.localPosition.y);    
            t += 0.015f;
            yield return new WaitForFixedUpdate();
        }
        //yield return new WaitForFixedUpdate();

    }

    IEnumerator ExitLevelIndicator(GameObject levelIndicatorText)
    {
        yield return EnterLevelIndicator(levelIndicatorText);
        yield return new WaitForSeconds(0.5f);
        float t = 0;
        float ease, newPosX;
        while (t <= 1f)
        {
            ease = EaseInQuartic(t);
            newPosX = Mathf.Lerp(0, 1916, ease);
            this.levelIndicatorText.transform.localPosition = new Vector3(newPosX, levelIndicatorText.transform.localPosition.y);    
            t += 0.02f;
            yield return new WaitForFixedUpdate();
        }
        //yield return new WaitForFixedUpdate();

    }
    
    IEnumerator Victory()
    {
        yield return SnapAll();
        yield return new WaitForSeconds(1.5f);
        addedScoreText.GetComponent<Text>().text = "+ " + 100;
        addedBonusScoreText.GetComponent<Text>().text = "+ " + string.Format("{0:00}", time);
        youWonBackGroundUI.SetActive(true);
        StartCoroutine(ShowYouWon(youWonUI));
        yield return new WaitForSeconds(1.5f);
        
        StartCoroutine(ShowAddedBonusScore(addedBonusScoreText));
        scoreDisplay.UpdateScore(100, (int)time);

        //atomsManager.GetAtoms()[0].GetComponent<Atom>().ChangeMaterial(0);

    }

    IEnumerator SnapAll()
    {
        if (!isSymmetric)
        {
            for (int i = 0; i < atomsManager.GetAtoms().Count; i++)
            {
                Atom atom = atomsManager.GetAtoms()[i];
                StartCoroutine(Snap(atom));
            }
        }
        else
        {
            for (int i = atomsManager.GetAtoms().Count - 1; i >= 0; i--)
            {
                Atom atom = atomsManager.GetAtoms()[i];
                StartCoroutine(Snap(atom));
                //yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
            }
        }

        yield return new WaitUntil(() => n == 0);
    }
    
    IEnumerator Snap(Atom atom)
    {
        atom.GetComponent<Collider>().enabled = false;
        atom.GetComponent<Atom>().SetSolved(true);
        atom.GetComponent<Atom>().ChangeMaterial(3);
        int j = -1;
        float minDist = float.MaxValue;
        Vector3 atomPos = atom.transform.localPosition;
        for (int i = 0; i < solAtomPos.Length; i++)
        {
            float curDist = Vector3.Distance(atomPos, solutionManager.GetPositions()[i]);
            if (!(curDist < minDist)) continue;
            minDist = curDist;
            j = i;
        }
        
        float t = 0;
        float ease, newPosX, newPosY, newPosZ;
        float posX = atomPos.x,
              posY = atomPos.y,
              posZ = atomPos.z;
        Vector3 targetPos = new Vector3(solutionManager.GetPositions()[j].x, solutionManager.GetPositions()[j].y, solutionManager.GetPositions()[j].z);
        while (t <= 1f)
        {
            ease = EaseInCubic(t);
            newPosX = Mathf.Lerp(posX, targetPos.x, ease);
            newPosY = Mathf.Lerp(posY, targetPos.y, ease);
            newPosZ = Mathf.Lerp(posZ, targetPos.z, ease);
            atom.transform.localPosition = new Vector3(newPosX, newPosY, newPosZ); 
            t += 0.01f;
            yield return new WaitForFixedUpdate();
        }
        n--;
    }

    IEnumerator ShowYouWon(GameObject youWonUI)
    {
        youWonUI.SetActive(true);
        float t = 0;
        float ease;
        float newScaleX, newScaleY;
        while (t <= 1f)
        {
            ease = EaseOutBounce(t);
            newScaleX = Mathf.Lerp(0.4f, 10f, ease);
            newScaleY = Mathf.Lerp(0.4f, 10f, ease);
            youWonUI.transform.localScale = new Vector3(newScaleX, newScaleY, 0.4f);                
            t += 0.02f;
            yield return new WaitForFixedUpdate();

        }
    }

    IEnumerator ShowAddedScore(GameObject addedScore)
    {
        float t = 0;
        float ease;
        float newScaleX, newScaleY;
        while (t <= 1f)
        {
            ease = ZoomInZoomOut(t);
            newScaleX = Mathf.Lerp(1f, 2f, ease);
            newScaleY = Mathf.Lerp(1f, 2f, ease);
            addedScore.transform.localScale = new Vector3(newScaleX, newScaleY, 1f);;                 
            t += 0.02f;
            yield return new WaitForFixedUpdate();
        }
    }
    
    IEnumerator ShowAddedBonusScore(GameObject addedBonusScore)
    {
        yield return ShowAddedScore(addedScoreText);
        float t = 0;
        float ease;
        float newScaleX, newScaleY;
        while (t <= 1f)
        {
            ease = ZoomInZoomOut(t);
            newScaleX = Mathf.Lerp(1f, 2f, ease);
            newScaleY = Mathf.Lerp(1f, 2f, ease);
            addedBonusScore.transform.localScale = new Vector3(newScaleX, newScaleY, 1f);;                 
            t += 0.02f;
            yield return new WaitForFixedUpdate();
        }
    }

    private float EaseInCubic(float x)
    {
        return x * x * x;
    }

    private float EaseOutBounce(float x) 
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (x < 1 / d1) 
        {
            return n1 * x * x;
        }

        if (x < 2 / d1) 
        {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        }

        if (x < 2.5 / d1) 
        {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        }

        return n1 * (x -= 2.625f / d1) * x + 0.984375f;
    }

    private float ZoomInZoomOut(float x)
    {
        return -4 * x * x + 4 * x;
    }

    private float EaseOutQuartic(float x)
    {
        return 1 - Mathf.Pow(1 - x, 4);
    }
    
    private float EaseInQuartic(float x)
    {
        return x * x * x * x;
    }
    
    public bool GetOver()
    {
        return over;
    }
    
}
