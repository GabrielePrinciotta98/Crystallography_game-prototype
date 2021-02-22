using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    [SerializeReference] private HUDManager hudManager;
    [SerializeReference] private AtomsManager atomsManager;
    [SerializeReference] private SolutionManager solutionManager;
    [SerializeReference] private GameObject youWonUI;
    [SerializeReference] private GameObject curtain;
    [SerializeReference] private GameObject youWonBackGroundUI;
    [SerializeReference] private GameObject addedScoreText;
    [SerializeReference] private GameObject controlPlane;
    [SerializeReference] private GameObject dottedLine;
    [SerializeReference] private GameObject levelIndicatorBackGroundUI;
    [SerializeReference] private GameObject levelIndicatorText;
    [SerializeReference] private GameObject nextLevelButton;
    [SerializeReference] private GameObject backButton;

    private int frames;
    private Vector3 atomPos;
    private Vector3[] solAtomPos;
    private bool[] markedSolAtomPos;
    private int n;
    private bool over = false;
   

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Scena corrente:" + (SceneManager.GetActiveScene().buildIndex-1));
        Debug.Log("Lv counter: " + LevelLoader.LevelCounter);
        Level lv = LevelData.Levels[LevelLoader.LevelCounter-1];
        //Debug.Log(lv.SolPositions);
        InstantiateAtomsManager(lv.R,lv.M,lv.N, lv.IsCrystal);
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

    private void InstantiateAtomsManager(int r, int m, int n, bool isCrystal)
    {
        atomsManager = Instantiate(atomsManager);
        atomsManager.SetCrystal(isCrystal);
        atomsManager.SetR(r);
        atomsManager.SetM(m);
        atomsManager.SetN(n);
    }
    
    

    // Update is called once per frame
    void Update()
    {
        Atom atom = atomsManager.GetDraggingAtom();
        int j = -1; // j è l'indice della posizione risolta
        if (frames % 10 == 0 && !over && atom != null)
        {

            bool atomSolved = false;
            for (int i = 0; i < atomsManager.GetAtoms().Count; i++)
            {
                if (!(Vector3.Distance(atom.transform.localPosition, solAtomPos[i]) <= 1f) || markedSolAtomPos[i]) 
                    continue;
                j = i;
                markedSolAtomPos[i] = true;
                atomSolved = true;
                break;
            }
            
            
            if (atomSolved)
            {
                //atomsManager.GetAtoms()[0].transform.localPosition = solutionManager.GetAtoms()[0].transform.localPosition; //snap
                StartCoroutine(Snap(atom, j));
                atom.GetComponent<Collider>().enabled = false;
                atom.GetComponent<Atom>().SetSolved(true);
                atom.GetComponent<Atom>().ChangeMaterial(3);
                atomsManager.SetDraggingAtom(null);
                n--;
            }
            //Debug.Log(n);

            if (n == 0) 
            {
                curtain.GetComponent<Curtain>().ShowSolution();
                StartCoroutine(Victory());
                ScoreManager.Score += 100;    
                print("hai vinto");
                //LevelLoader.LevelCounter++;
                over = true;
            }
            frames = 0;
        }

        frames++;
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
        yield return new WaitForSeconds(1.5f);
        youWonBackGroundUI.SetActive(true);
        StartCoroutine(ShowYouWon(youWonUI));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(ShowAddedScore(addedScoreText));
        //atomsManager.GetAtoms()[0].GetComponent<Atom>().ChangeMaterial(0);

    }
    
    IEnumerator Snap(Atom atom, int j)
    {
        float t = 0;
        float ease, newPosX, newPosY, newPosZ;
        float posX = atom.transform.localPosition.x,
              posY = atom.transform.localPosition.y,
              posZ = atom.transform.localPosition.z;
        while (t <= 1f)
        {
            ease = EaseInCubic(t);
            newPosX = Mathf.Lerp(posX, solAtomPos[j].x, ease);
            newPosY = Mathf.Lerp(posY, solAtomPos[j].y, ease);
            newPosZ = Mathf.Lerp(posZ, solAtomPos[j].z, ease);
            atom.transform.localPosition = new Vector3(newPosX, newPosY, newPosZ); 
            t += 0.2f;
            yield return new WaitForFixedUpdate();
        }
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
