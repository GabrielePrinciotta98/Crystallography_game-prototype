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
    [SerializeReference] private Slider rotationSlider;
    private int frames;
    private Vector3 atomPos;
    private Vector3[] solAtomPos;
    private bool[] markedSolAtomPos;
    private int n;
    private bool over = false;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(hudManager);
        //Debug.Log("Scena corrente:" + (SceneManager.GetActiveScene().buildIndex-1));
        Debug.Log("Lv counter: " + LevelLoader.LevelCounter);
        Level lv = LevelData.SetLevels[LevelLoader.LevelCounter-1];
        
        InstantiateAtomsManager(lv.R,lv.M,lv.N, lv.IsCrystal);
        InstantiateSolutionManager(lv.R,lv.M,lv.N, lv.IsCrystal, lv.SolPositions);
        
        FindObjectOfType<Detector>().SetAtomsManager(atomsManager);
        FindObjectOfType<SolutionDetector>().SetSolutionManager(solutionManager);
        rotationSlider.onValueChanged.AddListener(delegate {atomsManager.Rotate(1f);});
        rotationSlider.onValueChanged.AddListener(delegate {solutionManager.Rotate(1f);});
        solAtomPos = lv.SolPositions;
        markedSolAtomPos = new bool[solAtomPos.Length];
        for (int i = 0; i < markedSolAtomPos.Length; i++)
            markedSolAtomPos[i] = false;
        n = lv.N-1; // n sono gli atomi ancora da risolvere
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

                if (Vector3.Distance(atom.transform.localPosition, solAtomPos[i]) <= 1f && !markedSolAtomPos[i])
                {
                    j = i;
                    markedSolAtomPos[i] = true;
                    atomSolved = true;
                    break;
                }
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
            Debug.Log(n);

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

    public float EaseInCubic(float x)
    {
        return x * x * x;
    }
    
    public float EaseOutBounce(float x) 
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

    public float ZoomInZoomOut(float x)
    {
        return -4 * x * x + 4 * x;
    }
    
    
    public bool GetOver()
    {
        return over;
    }
    
}
