using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    private Level lv;
    [SerializeReference] private HUDManager hudManager;
    [SerializeReference] private AtomsManager atomsManager;
    [SerializeReference] private SolutionManager solutionManager;
    [SerializeReference] private GameObject youWonUI;
    [SerializeReference] private GameObject gameWonUI;
    [SerializeReference] private GameObject curtain;
    [SerializeReference] private GameObject youWonBackGroundUI;
    [SerializeReference] private GameObject addedScoreText;
    [SerializeReference] private GameObject addedBonusScoreText;
    [SerializeReference] private GameObject hintScoreText;
    [SerializeReference] private GameObject dottedLine;
    [SerializeReference] private GameObject levelIndicatorBackGroundUI;
    [SerializeReference] private GameObject levelIndicatorText;
    [SerializeReference] private GameObject nextLevelButton;
    [SerializeReference] private GameObject backButton;
    [SerializeReference] private MoleculeManager moleculeManager; 
    [SerializeReference] private DialogueBox dialogueBox;

    //[SerializeReference] private GameObject hintArrow;
    private AudioManager audioManager;
    private EmitterCone emitterCone;
    private EmitterConeSol emitterConeSol;
    private HintArrow hintArrow;
    private Vector3[] solAtomPos;

    
    public Vector3[] SolAtomPos
    {
        get => solAtomPos;
        set => solAtomPos = value;
    }

    private bool[] markedSolAtomPos;
    private int n;
    private bool over = false;
    public bool anAtomWasDragged; 
    private bool isSymmetric;
    private float haussdorfThreshold = 0.8f;

    private ScoreDisplay scoreDisplay;
    
    private float time = 300; // 300 secondi
    private bool updateTime;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Scena corrente:" + (SceneManager.GetActiveScene().buildIndex-1));
        //Debug.Log("Lv counter: " + LevelLoader.LevelCounter);
        lv = LevelData.Levels[LevelLoader.LevelCounter-1];
        haussdorfThreshold = HaussdorfDifficulty(LevelLoader.LevelCounter);

        //Debug.Log(lv.SolPositions);
        lv.SolPositions = lv.IsCrystal ? new[] {RandomCentralCellPos()} : RandomPositions(lv.Plane, lv.N - 1);
        solAtomPos = lv.SolPositions;
        atomsManager = InstantiateAtomsManager(lv.R,lv.M,lv.N, lv.IsCrystal, lv.Plane, lv.SolPositions);
        solutionManager = InstantiateSolutionManager(lv.R,lv.M,lv.N, lv.IsCrystal, lv.Plane, lv.SolPositions); 
        InstantiateMoleculeManager(atomsManager, solutionManager);
        
        hudManager = Instantiate(hudManager);
        hudManager.dialogueBox = dialogueBox;
        FindObjectOfType<Detector>().SetAtomsManager(atomsManager);
        FindObjectOfType<Detector>().SetSolutionManager(solutionManager);
        FindObjectOfType<SolutionDetector>().SetSolutionManager(solutionManager);
        FindObjectOfType<ControlGizmo>().SetPlane(lv.Plane);
        markedSolAtomPos = new bool[solAtomPos.Length];
        for (int i = 0; i < markedSolAtomPos.Length; i++)
            markedSolAtomPos[i] = false;

        n = atomsManager.crystalActivated ? lv.M * lv.N - 1 : lv.N - 1; //n sono gli atomi ancora da risolvere

        Instantiate(dottedLine).gameObject.name = "DottedLineHoriz";
        var dottedLineVert = Instantiate(dottedLine);
        dottedLineVert.gameObject.name = "DottedLineVert";
        dottedLineVert.GetComponent<DottedLine>().Vertical = true;
        dottedLineVert.transform.Rotate(0,90,0);
        levelIndicatorText.GetComponent<Text>().text = lv.Description;
        
        audioManager = FindObjectOfType<AudioManager>();

        nextLevelButton.GetComponent<Button>().onClick.AddListener(LevelLoader.LoadNextLevel);
        nextLevelButton.GetComponent<Button>().onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
        backButton.GetComponent<Button>().onClick.AddListener(LevelLoader.LoadPrevScene);
        backButton.GetComponent<Button>().onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
        backButton.GetComponent<Button>().onClick.AddListener(delegate { audioManager.PlayInLoop("MenuTheme"); });
        backButton.GetComponent<Button>().onClick.AddListener(delegate { audioManager.Stop("GameplayTheme"); });

        hintArrow = FindObjectOfType<HintArrow>();
        emitterCone = FindObjectOfType<EmitterCone>();
        emitterConeSol = FindObjectOfType<EmitterConeSol>();

        scoreDisplay = FindObjectOfType<ScoreDisplay>();
        scoreDisplay.DisplayScore();

        dialogueBox = FindObjectOfType<DialogueBox>();
        
        Timer.Time = time;

        StartCoroutine(StartLevel());
    }

    private float HaussdorfDifficulty(int levelCounter)
    {
        switch (levelCounter)
        {
            case 1:
            case 4:
            case 7:
                return 0.8f;
            case 2:
            case 5:
            case 8:
            case 10:
                return 1f;
            case 3:
            case 6:
            case 9:
                return 1.2f;
            default:
                return 1f;
        }
    }

    private void InstantiateMoleculeManager(AtomsManager am, SolutionManager sm)
    {
        moleculeManager = Instantiate(moleculeManager);
        moleculeManager.AtomsManager = am;
        moleculeManager.SolutionManager = sm;
    }

    private SolutionManager InstantiateSolutionManager(int r, int m, int n, bool isCrystal, string plane, Vector3[] listPos)
    {
        solutionManager = Instantiate(solutionManager);
        solutionManager.SetCrystal(isCrystal);
        solutionManager.SetR(r);
        solutionManager.SetM(m);
        solutionManager.SetN(n);
        solutionManager.SetAtomSpawnPositions(listPos);
        solutionManager.Plane = plane;
        return solutionManager;
    }

    private AtomsManager InstantiateAtomsManager(int r, int m, int n, bool isCrystal, string plane, Vector3[] listPos)
    {
        atomsManager = Instantiate(atomsManager);
        atomsManager.SetCrystal(isCrystal);
        atomsManager.SetR(r);
        atomsManager.SetM(m);
        atomsManager.SetN(n);
        atomsManager.LevelType = plane;
        atomsManager.SetSolutionSpawnPositions(listPos);

        return atomsManager;
    }
    

    void Update()
    {
        bool emittersOn = emitterCone.GetPowerOn() && emitterConeSol.GetPowerOn();

        if (updateTime && !over && time > 0)
        {
            time -= Time.deltaTime;
            Timer.Time = time;
        }

        if (!emittersOn) return;

        
        bool testVictoryOn = !over && anAtomWasDragged;
        if (lv.IsCrystal)
        {
            if (testVictoryOn && atomsManager.crystalActivated)
            {
                TestVictory();
            }
        }
        else 
        {
            if (testVictoryOn)
            {
                TestVictory();
            }
        }
        
        
    }

    private void TestVictory()
    {
        float h = Haussdorf();
        
        //Debug.Log("amPos0: " + atomsManager.GetPositions()[0]);
        //Debug.Log("amPos1: " + atomsManager.GetPositions()[1]);
        
        //Debug.Log("saPos0: " + solutionManager.GetPositions()[0]);
        //Debug.Log("saPos1: " + solutionManager.GetPositions()[1]);
        //Debug.Log("Hausdorff: " + h);
        if (h <= haussdorfThreshold && atomsManager.CheckDistanceBetweenAtoms(haussdorfThreshold))
        {
            if (isSymmetric)
            {
                solutionManager.ChangeToSymmetric();
                //Debug.Log("simmetrico");
            }

            over = true;
            atomsManager.GameStart = false;
            //atomsManager.AnAtomIsMoving = true;
            StartCoroutine(Victory());
            audioManager.Play("Victory");
            ScoreDisplay.CurScore = ScoreManager.Score;
            if (!hintArrow.used)
            {
                ScoreManager.TimeBonus += Mathf.RoundToInt(time);
                ScoreManager.Score += 100;
            }
            if (FindObjectOfType<CheatCode>().AlreadyActivated != true)
                LevelsUnlocked.NumberOfLevelsUnlocked++;
            print("hai vinto");
        }
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
                float d1 = Vector3.Distance(atomsManager.GetPositions()[i], solutionManager.GetPositions()[j]);
                float d2 = Vector3.Distance(atomsManager.GetPositions()[i], -solutionManager.GetPositions()[j]);
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
        
        yield return StartCoroutine(ExitLevelIndicator(levelIndicatorText));
        
        levelIndicatorBackGroundUI.SetActive(false);
        atomsManager.GameStart = true;
        emitterCone.GameStart = true;
        emitterConeSol.GameStart = true;
        updateTime = true;
        if (LevelLoader.LevelCounter == 7)
        {
            dialogueBox.StartShiftButtonTutorial();
        }
        yield return null;

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
        hintArrow.EnableArrowRendering(false);
        hudManager.DisablePowerUps();
        yield return SnapAll();
        curtain.GetComponent<Curtain>().ShowSolution();

        yield return new WaitForSeconds(1.5f);
        
        
        youWonBackGroundUI.SetActive(true);
        if (hintArrow.used)
        {
            addedScoreText.SetActive(false);
            addedBonusScoreText.SetActive(false);
        }
        else
        {
            addedScoreText.GetComponent<Text>().text = "+ " + 100;
            addedBonusScoreText.GetComponent<Text>().text = "+ " + $"{time:00}";
            hintScoreText.SetActive(false);
        }
        
        if (atomsManager.crystalActivated)
        {
            nextLevelButton.gameObject.SetActive(false);
            gameWonUI.SetActive(true);
            youWonUI.SetActive(false);
            StartCoroutine(ShowYouWon(gameWonUI));
        }
        else
        {
            gameWonUI.SetActive(false);
            youWonUI.SetActive(true);
            StartCoroutine(ShowYouWon(youWonUI));
        }

        yield return new WaitForSeconds(1.5f);

        if (!hintArrow.used)
        {
            StartCoroutine(ShowAddedBonusScore(addedBonusScoreText));
            scoreDisplay.UpdateScore(100, Mathf.RoundToInt(time));
        }
        else
            StartCoroutine(ShowHintScore(hintScoreText));
        //atomsManager.GetAtoms()[0].GetComponent<Atom>().ChangeMaterial(0);

    }

    IEnumerator SnapAll()
    {
        audioManager.Play("Snap");
        if (moleculeManager.Activated)
        {
            for (int i = 0; i < atomsManager.GetAtoms().Count; i++)
            {
                Atom atom = atomsManager.GetAtoms()[i];
                StartCoroutine(MarkAsCorrect(atom));
            }

            bool allSnapped = true;
            do
            {
                for (int i = 0; i < atomsManager.GetAtoms().Count; i++)
                {
                    Atom atom = atomsManager.GetAtoms()[i];
                    if (atom.Snapped) continue;
                    Atom father = atom.transform.parent.GetComponent<Atom>();
                    if (father == null) // il padre è il pivot
                    {
                        yield return Snap(atom);
                        atom.Snapped = true;
                    }
                    else // il padre è un atomo
                    {
                        if (!father.Snapped) continue;
                        yield return Snap(atom);
                        atom.Snapped = true;
                    }
                }

                for (int i = 0; i < atomsManager.GetAtoms().Count; i++)
                {
                    if (!atomsManager.GetAtoms()[i].Snapped)
                    {
                        allSnapped = false;
                        break;
                    }
                    allSnapped = true;
                }

            } while (!allSnapped);
            /*
            if (!isSymmetric)
            {
                for (int i = 0; i < atomsManager.GetAtoms().Count; i++)
                {
                    Atom atom = atomsManager.GetAtoms()[i];
                    StartCoroutine(MarkAsCorrect(atom));
                }

                bool allSnapped = true;
                do
                {
                    for (int i = 0; i < atomsManager.GetAtoms().Count; i++)
                    {
                        Atom atom = atomsManager.GetAtoms()[i];
                        if (atom.Snapped) continue;
                        Atom father = atom.transform.parent.GetComponent<Atom>();
                        if (father == null) // il padre è il pivot
                        {
                            yield return Snap(atom);
                            atom.Snapped = true;
                        }
                        else // il padre è un atomo
                        {
                            if (!father.Snapped) continue;
                            yield return Snap(atom);
                            atom.Snapped = true;
                        }
                    }

                    for (int i = 0; i < atomsManager.GetAtoms().Count; i++)
                    {
                        if (!atomsManager.GetAtoms()[i].Snapped)
                        {
                            allSnapped = false;
                            break;
                        }
                        allSnapped = true;
                    }

                } while (!allSnapped);
            }
            else
            {
                for (int i = atomsManager.GetAtoms().Count - 1; i >= 0; i--)
                {
                    Atom atom = atomsManager.GetAtoms()[i];
                    StartCoroutine(MarkAsCorrect(atom));
                }

                bool allSnapped = true;

                do
                {
                    for (int i = atomsManager.GetAtoms().Count - 1; i >= 0; i--)
                    {
                        Atom atom = atomsManager.GetAtoms()[i];
                        if (atom.Snapped) continue;
                        Atom father = atom.transform.parent.GetComponent<Atom>();
                        if (father == null) // il padre è il pivot
                        {
                            yield return Snap(atom);
                            atom.Snapped = true;

                        }
                        else // il padre è un atomo
                        {
                            if (!father.Snapped) continue;
                            yield return Snap(atom);
                            atom.Snapped = true;
                        }
                    }
                    
                    for (int i = 0; i < atomsManager.GetAtoms().Count; i++)
                    {
                        if (!atomsManager.GetAtoms()[i].Snapped)
                        {
                            allSnapped = false;
                            break;
                        }
                        allSnapped = true;
                    }
                    
                } while (!allSnapped);
            }
        */
        }
        else
        {
            int numberOfAtomsToBeSnapped = atomsManager.crystalActivated ? atomsManager.newAtoms.Count : atomsManager.GetAtoms().Count;
            Debug.Log("noatbs: " + numberOfAtomsToBeSnapped);
            for (int i = 0; i < numberOfAtomsToBeSnapped; i++)
            {
                Atom atom = atomsManager.crystalActivated ? atomsManager.newAtoms[i].GetComponent<Atom>() : atomsManager.GetAtoms()[i];
                StartCoroutine(MarkAsCorrect(atom));
                if (atomsManager.crystalActivated)
                {
                    if (atom == atomsManager.centralCellAtom)
                        StartCoroutine(Snap(atom));
                }
                else
                    StartCoroutine(Snap(atom));
            }
            /*
            if (!isSymmetric)
            {
                for (int i = 0; i < numberOfAtomsToBeSnapped; i++)
                {
                    Atom atom = atomsManager.crystalActivated ? atomsManager.newAtoms[i].GetComponent<Atom>() : atomsManager.GetAtoms()[i];
                    StartCoroutine(MarkAsCorrect(atom));
                    StartCoroutine(Snap(atom));
                }
            }
            else
            {
                for (int i = numberOfAtomsToBeSnapped - 1; i >= 0; i--)
                {
                    Atom atom = atomsManager.crystalActivated ? atomsManager.newAtoms[i].GetComponent<Atom>() : atomsManager.GetAtoms()[i];
                    StartCoroutine(MarkAsCorrect(atom));
                    StartCoroutine(Snap(atom));
                }
            }
            */
        }

        yield return new WaitUntil(() => n <= 0);
    }

    IEnumerator MarkAsCorrect(Atom atom)
    {
        atom.GetComponent<Collider>().enabled = false;
        atom.GetComponent<Atom>().SetSolved(true);
        atom.GetComponent<Atom>().ChangeMaterial(3);
        yield return null;
    }
    
    IEnumerator Snap(Atom atom)
    {
        
        int j = -1;
        float minDist = float.MaxValue;
        float posX = atom.transform.localPosition.x;
        float posY = atom.transform.localPosition.y;
        float posZ = atom.transform.localPosition.z;
        

        for (int i = 0; i < solAtomPos.Length; i++)
        {
            float curDist = Vector3.Distance(atom.PositionFromPivot, solutionManager.GetPositions()[i]);
            if (!(curDist < minDist)) continue;
            minDist = curDist;
            j = i;
        }
        float t = 0;
        float ease, newPosX, newPosY, newPosZ;
        Vector3 targetPosFromPivot = new Vector3(solutionManager.GetPositions()[j].x, solutionManager.GetPositions()[j].y, solutionManager.GetPositions()[j].z);
        Vector3 targetPos;
        Atom father = atom.transform.parent.GetComponent<Atom>();
        if (father != null)
        {
            targetPos = targetPosFromPivot - father.PositionFromPivot;
        }
        else
        {
            targetPos = targetPosFromPivot;
        }
        //Debug.Log("tPos: " + targetPos);
        //Debug.Log("antirotation: " + solutionManager.antiRotation);
        targetPos = solutionManager.antiRotation * targetPos;
        
        //Debug.Log("tPosR: " + targetPos);
        Vector3 offset;
        while (t <= 1f)
        {
            ease = moleculeManager.Activated ? EaseInQuartic(t) : EaseInCubic(t);
            newPosX = Mathf.Lerp(posX, targetPos.x, ease);
            newPosY = Mathf.Lerp(posY, targetPos.y, ease);
            newPosZ = Mathf.Lerp(posZ, targetPos.z, ease);
            Vector3 newPos = new Vector3(newPosX, newPosY, newPosZ);
            offset = newPos - atom.transform.localPosition;
            
            if (atomsManager.crystalActivated)
                atom.AddOffsetToReplicas(offset);
            atom.transform.localPosition = newPos; 
            FindObjectOfType<Detector>().SetDirty();
            t += 0.01f;
            yield return new WaitForFixedUpdate();
        }

        offset = targetPos - atom.transform.localPosition;
        if (atomsManager.crystalActivated)
            atom.AddOffsetToReplicas(offset);
        atom.transform.localPosition = targetPos;
        
        FindObjectOfType<Detector>().SetDirty();
        atom.Snapped = true;
        n--;
    }

    IEnumerator ShowYouWon(GameObject youWonUI)
    {
        float t = 0;
        float ease;
        float newScaleX, newScaleY;
        while (t <= 1f)
        {
            ease = EaseOutBounce(t);
            newScaleX = Mathf.Lerp(0.4f, 4f, ease);
            newScaleY = Mathf.Lerp(0.4f, 4f, ease);
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
    
    IEnumerator ShowHintScore(GameObject hintScore)
    {
        
        float t = 0;
        float ease;
        float newScaleX, newScaleY;
        while (t <= 1f)
        {
            ease = ZoomInZoomOut(t);
            newScaleX = Mathf.Lerp(1f, 1.6f, ease);
            newScaleY = Mathf.Lerp(1f, 1.6f, ease);
            hintScore.transform.localScale = new Vector3(newScaleX, newScaleY, 1f);;                 
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
    
    public float easeInOutBack(float x)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;

        return x < 0.5
            ? (Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
            : (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
        
    }
    
    public bool GetOver()
    {
        return over;
    }
    
    private static Vector3[] RandomPositions(string plane, int n)
    {
        Vector3[] ris = new Vector3[n];

        for (int i = 0; i < n; i++)
        {
            bool isEqual = false;
            bool isPivot = false;
            Vector3 newPos = plane switch
            {
                "YZ" => new Vector3(0, Random.Range(-5f, 5f), Random.Range(-5f, 5f)),
                "XZ" => new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f)),
                "XYZ" => new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)),
                _ => Vector3.zero
            };
            for (int j = 0; j < i; j++)
            {
                if (Vector3.Distance(ris[j], newPos) > 2) continue;
                isEqual = true;
                break;
            }
            
            if (Vector3.Distance(newPos, Vector3.zero) <= 1)
                isPivot = true;
            
            if (isEqual || isPivot)
            {
                i--;
                continue;
            }
            ris[i] = newPos;
        }

        return ris;
    }
    
    private static Vector3 RandomCentralCellPos()
    {
        Vector3 ris;
        bool closeToPivot;
        do
        {
            ris = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));
            closeToPivot = Vector3.Distance(ris, Vector3.zero) <= 1;
        } while (closeToPivot);

        return ris;
    }
}
