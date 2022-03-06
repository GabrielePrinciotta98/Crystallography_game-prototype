using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AtomsManager : MonoBehaviour
{
    private LevelManager levelManager;
    private SolutionManager solutionManager;
    private Detector detector;
    public bool isCrystal; //se il livello prevede il cristallo
    public bool crystalActivated; //se è stata attivata la modalità cristallo
    
    public bool GameStart { get; set; }
    private GameObject moleculeSpace;
    [SerializeReference] GameObject atom;
    [SerializeReference] GameObject pivot;
    [SerializeReference] GameObject centralCell;
    [SerializeReference] GameObject cell;
    [SerializeReference] private GameObject[] platforms;
    [SerializeField] int N = 1;
    [SerializeField] int M = 1;
    [SerializeField] int K = 5;
    [SerializeField] int R = 4;
    [SerializeReference] GameObject pivotRep;
    [SerializeReference] GameObject atomRep;
    [SerializeReference] private Material selectedAtomRep;
    
    List<Atom> atoms = new List<Atom>();
    private List<Atom> oldAtoms;
    Vector4[] positions = new Vector4[60];


    private List<Vector3> cellsSpawnPositions = new List<Vector3>();
    private List<Vector3> atomsSpawnPositions = new List<Vector3>();
    private Vector3[] solutionSpawnPositions;
    private Atom draggingAtom;
    public List<GameObject> cells = new List<GameObject>();
    public string LevelType { get; set; }
    private int atomInPositionCrystalCounter = 0;
    private static readonly Vector3 Center = new Vector3(22, 6.6f, 10);
    public Atom centralCellAtom;
    private List<Vector3> pivotRepPositions;
    private List<GameObject> newPivots; 
    public List<GameObject> newAtoms;
    public List<int> atomRelatedCellPositions = new List<int>(); 
    

    // Start is called before the first frame update
    void Start()
    {
        moleculeSpace = GameObject.Find("MoleculeSpace");
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        detector = FindObjectOfType<Detector>();
        //Debug.Log("N: " + N);
        
        
        //POSIZIONI DI SPAWN DELLE CELLE 
        if (K > 5)
        {
            for (int x = -K; x < 2 * K; x += K)
            for (int y = -K; y < 2 * K; y += K)
            for (int z = -K; z < 2 * K; z += K)
                if (x != 0 || y != 0 || z != 0)
                    cellsSpawnPositions.Add(new Vector3(x, y, z));
        }
        else
        {
            CalculateCellsPositions();
        }
        
        
        if (isCrystal)
        {
            // POSIZIONI DI SPAWN DEGLI ATOMI
            GenerateRandomPositions(LevelType, M*N - 1);
            Instantiate(LevelType.Equals("YZ") ? platforms[0] : platforms[1], moleculeSpace.transform);
            //Center ha la posizione del pivot
            pivot = Instantiate(pivot, Center, Quaternion.identity, moleculeSpace.transform);
            for (int i=0; i < M*N - 1; i++)
                Instantiate(atom, new Vector3(Center.x + atomsSpawnPositions[i].x,
                        Center.y + atomsSpawnPositions[i].y,
                        Center.z + atomsSpawnPositions[i].z),
                    Quaternion.identity, moleculeSpace.transform);
            /*
            Instantiate(platforms[1]);
            // SPAWN CELLA CENTRALE
            centralCell = Instantiate(centralCell, centralCellSpawnPos, Quaternion.identity);
            if (K >= 5)
                centralCell.transform.localScale *= K; // scalo la dimensione del modulo centrale
            else
                centralCell.transform.localScale *= 5;

            SpawnRepeatedCells();
            */
            
        }
        else
        {
            // POSIZIONI DI SPAWN DEGLI ATOMI
            GenerateRandomPositions(LevelType, N-1);
            Instantiate(LevelType.Equals("YZ") ? platforms[0] : platforms[1], moleculeSpace.transform);
            //centralCellSpawnPos ha la posizione del pivot
            pivot = Instantiate(pivot, Center, Quaternion.identity, moleculeSpace.transform);
            for (int i=0; i<N-1; i++)
                Instantiate(atom, new Vector3(Center.x + atomsSpawnPositions[i].x,
                        Center.y + atomsSpawnPositions[i].y,
                        Center.z + atomsSpawnPositions[i].z),
                    Quaternion.identity, moleculeSpace.transform);
        }
        

    }

    private void CalculateCellsPositions()
    {
        Debug.Log("R: " + R);
        //cellsSpawnPositions = new List<Vector3>();

        if (R == 3)
        {
            for (float x = -5f; x < 10f; x += 5f)
            for (float y = -5f; y < 10f; y += 5f)
            for (float z = -5f; z < 10f; z += 5f)
                if (x != 0 || y != 0 || z != 0)
                    cellsSpawnPositions.Add(new Vector3(x, y, z));
        }

        if (R == 5)
        {
            for (float x = -10f; x < 15f; x += 5f)
            for (float y = -10f; y < 15f; y += 5f)
            for (float z = -10f; z < 15f; z += 5f)
                if (x != 0 || y != 0 || z != 0)
                    cellsSpawnPositions.Add(new Vector3(x, y, z));
        }

        if (R == 7)
        {
            for (float x = -15f; x < 20f; x += 5f)
            for (float y = -15f; y < 20f; y += 5f)
            for (float z = -15f; z < 20f; z += 5f)
                if (x != 0 || y != 0 || z != 0)
                    cellsSpawnPositions.Add(new Vector3(x, y, z));
        }
    }

    private void SpawnRepeatedCells(GameObject centralCellRef)
    {
        // SPAWN CELLE RIPETUTE
        Debug.Log(M);
        if (R == 1) return;
        for (int i = 0; i < M - 1; i++)
        {
            var cellTemp = Instantiate(cell, Center + cellsSpawnPositions[i], Quaternion.identity,
                centralCellRef.transform);
            cells.Add(cellTemp);
        }
    }


    public void FreezeAtoms()
    {
        foreach (var atom in atoms)
        {
            if (atom != draggingAtom)
                atom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            else
            {
                if (LevelType.Equals("YZ"))
                    atom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
                if (LevelType.Equals("XZ"))
                    atom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            }
        }
    }

    public void UnFreezeAtoms()
    {
        foreach (var atom in atoms)
        {
            atom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            atom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public void SetDraggingAtom(Atom atom)
    {
        draggingAtom = atom;
    }

    public Atom GetDraggingAtom()
    {
        return this.draggingAtom;
    }
    
    public void AddAtom(Atom atom)
    {
        atoms.Add(atom);
        positions[atoms.IndexOf(atom)] = atom.transform.localPosition;

    }

    public List<Atom> GetAtoms()
    {
        return atoms;
    }

    public void SetMyPosition(Atom atom)
    {
        positions[atoms.IndexOf(atom)] = atom.PositionFromPivot;
    }
    

    public Vector4[] GetPositions()
    {
        return positions;
    } 

    
    public int GetN()
    {
        return N;
    }

    public void SetN(int n)
    {
        N = n;
        //centralCell.GetComponent<CentralCell>().InstantiateAtoms();
        //Debug.Log("setN: " + GetN());
    }
    
    public void SetN(Slider slider)
    {
        N = (int)slider.value;
        
        //Debug.Log("setN: " + GetN());
    }
    
    public int GetM()
    {
        return M;
    }
    
    public void SetM(float m)
    {
        M = (int)m;
        FindObjectOfType<Detector>().SetDirty();
    }

    public int GetK()
    {
        return K;
    }

    public Vector3 GetCellForward()
    {
        return centralCell.transform.forward;
    }

    public Vector3 GetCellRight()
    {
        return centralCell.transform.right;
    }
    

    public void Rotate(float degrees)
    {
        moleculeSpace.transform.rotation = Quaternion.Euler(0, degrees, 0);
        FindObjectOfType<Detector>().SetDirty();
    }


    public int GetR()
    {
        return R;
    }

    public void SetR(float r)
    {
        R = (int)r;
        FindObjectOfType<Detector>().SetDirty();
    }

    public bool GetCrystal()
    {
        return isCrystal;
    }

    public void SetCrystal(bool flag)
    {
        isCrystal = flag;
    }
    
    private void GenerateRandomPositions(string plane, int n)
    {
        Vector3[] ris = new Vector3[n];

        for (int i = 0; i < n; i++)
        {
            bool isEqual = false;
            bool isPivot = false;
            bool isSpawn = false;

            Vector3 newPos = plane switch
            {
                "YZ" => new Vector3(0, Random.Range(-5f, 5f), Random.Range(-5f, 5f)),
                "XZ" => new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f)),
                "XYZ" => new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)),
                _ => Vector3.zero
            };
            for (int j = 0; j < i; j++)
            {
                if (Vector3.Distance(ris[j], newPos) > 1) continue;
                isEqual = true;
                break;
            }

            for (int j = 0; j < N-1; j++)
            {
                if (!(Vector3.Distance(newPos, solutionSpawnPositions[j]) < 1)) continue;
                if (!(Vector3.Distance(newPos, -solutionSpawnPositions[j]) < 1)) continue;
                isSpawn = true;
                break;
            }
            

            if (Vector3.Distance(newPos, Vector3.zero) <= 1)
                isPivot = true;
            
            if (isEqual || isPivot || isSpawn)
            {
                i--;
                continue;
            }
            ris[i] = newPos;
        }

        foreach (var v in ris)
        {
            atomsSpawnPositions.Add(v);
        } 
        
    }

    public void SetSolutionSpawnPositions(Vector3[] listPos)
    {
        solutionSpawnPositions = listPos;
    }

    public GameObject GetPivot()
    {
        return pivot;
    }

    /*
    public void UpdateCells()
    {
        foreach (var cell in cells)
        {
            cell.GetComponent<Cell>().DestroyAtoms();
            Destroy(cell);
        }

        cells = new List<GameObject>();
        CalculateCellsPositions();
        SpawnRepeatedCells();
    }
    */

    public void TriggerCrystalActivation(Button crystal, Button crystalDisabled)
    {
        
        GameStart = false; //disattivo la possibilita di trascinare gli atomi durante l'animazione
        //StartCoroutine(Whistle());
        StartCoroutine(ActivateCrystal());
        crystal.gameObject.SetActive(false);
        crystalDisabled.gameObject.SetActive(true);
    }
    
    public void TriggerCrystalDeactivation(Button crystal, Button crystalDisabled)
    {
        GameStart = false; //disattivo la possibilita di trascinare gli atomi durante l'animazione
        StartCoroutine(DeactivateCrystal());
        crystal.gameObject.SetActive(true);
        crystalDisabled.gameObject.SetActive(false);
    }
    
    

    public GameObject GetMyCell(GameObject a)
    {
        return cells[newAtoms.IndexOf(a)];
    }
    
    private IEnumerator ActivateCrystal()
    {
        solutionManager = FindObjectOfType<SolutionManager>();
        GameObject[] solutionAtoms = solutionManager.AllAtoms.ToArray(); //tutti i solution atoms senza il pivot
        
        bool[] isOccupied = new bool[atoms.Count];
        Vector3[] targetPositions = new Vector3[atoms.Count]; // array che conterra le posizioni finali
        
        
        pivotRepPositions = GetPivotRepPositions(solutionAtoms);
        Debug.Log(pivotRepPositions.Count);
        //Assegno gli atomi alle posizioni dei pivot ripetuti
        newPivots = AssignPivotPositions(isOccupied, targetPositions);

        
        //Trovo il vettore che connette il centro della scena (22, 6.6, 10) al baricentro degli atomi rimasti
        Vector3 d = FindBarycenterToCenterVector(isOccupied);
        //Assegno gli atomi alle posizioni degli atomi rossi ripetuti
        newAtoms = AssignAtomPositions(isOccupied, targetPositions, d);

        yield return StartCoroutine(MoveAllToTargetPositions(targetPositions));

        
        
        for (int i = 0; i < atoms.Count; i++)
        {
            atoms[i].transform.position = targetPositions[i];
        }
        
        //LAMPO
        yield return new WaitForSeconds(0.5f);
        SpawnCells(); //genera la lista cells
        
        
        yield return StartCoroutine(ChangeAtomsType(newPivots));
        Debug.Log("tipo cambiato");
        
        List<Cell> cellComponents = new List<Cell>();
        foreach (var c in cells)
        {
            cellComponents.Add(c.GetComponent<Cell>());
        }
        
        //Setto alcuni attributi legati al cristallo per ogni atomo
        foreach (var a in newAtoms)
        {
            a.GetComponent<Atom>().CrystalActivation(newAtoms, cellComponents);
        }
        
        //il numero di atomi nella lista Atoms deve passare da 53 a 1, l'unico atomo della cella centrale
        oldAtoms = atoms;
        atoms = new List<Atom> {centralCellAtom};
        
        //passare allo shader i parametri della modalita cristallo
        detector.TransitionHappened = true;
        detector.SetDirty();
        
        crystalActivated = true;
        GameStart = true; //riattivo il trascinamento degli atomi
    }

    


    private Vector3 FindBarycenterToCenterVector(bool[] isOccupied)
    {
        Vector3 sum = Vector3.zero;
        int nFreeAtoms = 0;
        for (int i = 0; i < atoms.Count; i++)
        {
            if (isOccupied[i]) continue;
            sum += atoms[i].transform.position;
            nFreeAtoms++;
        }
        Vector3 barycenter = sum / nFreeAtoms;
        return barycenter - Center;
    }


    private List<Vector3> GetPivotRepPositions(GameObject[] solutionAtoms)
    {
        List<Vector3> ris = new List<Vector3>();

        foreach (var a in solutionAtoms)
        {
            if (a.GetComponent<PivotAtomRepSol>())
                ris.Add(a.transform.position + new Vector3(0,0,30));
        }
        ris.Add(Center);
        return ris;
    }

    private List<GameObject> AssignPivotPositions(bool[] isOccupied, Vector3[] targetPositions)
    {
        List<GameObject> ris = new List<GameObject>();
        foreach (var pivotRepPosition in pivotRepPositions)
        {
            float minDistance = Single.MaxValue;
            int closestAtom = -1;
            if (pivotRepPosition == Center) continue;
            
            for (int j = 0; j < atoms.Count; j++)
            {
                float distance = Vector3.Distance(pivotRepPosition, atoms[j].transform.position);
                
                if (!isOccupied[j] && distance < minDistance)
                {
                    minDistance = distance;
                    closestAtom = j;
                }
            }
            isOccupied[closestAtom] = true;
            targetPositions[closestAtom] = pivotRepPosition;
            ris.Add(atoms[closestAtom].gameObject);
        }

        return ris;
    }
    
    private List<GameObject> AssignAtomPositions(bool[] isOccupied, Vector3[] targetPositions, Vector3 d)
    {
        List<GameObject> ris = new List<GameObject>();
        
        Vector3[] tempTargetPositions = new Vector3[atoms.Count - M + 1]; 
        int[] atomsIndexes =  new int[atoms.Count - M + 1];
        int j = -1;
        for (int i = 0; i < atoms.Count; i++)
        {
            if (isOccupied[i]) continue;
            j++;
            tempTargetPositions[j] = atoms[i].transform.position - (d + Vector3.one);
            atomsIndexes[j] = i;
        }

        for (var i = 0; i < pivotRepPositions.Count; i++)
        {
            var pivotRepPosition = pivotRepPositions[i];
            float minDistance = Single.MaxValue;
            int closestAtom = -1;
            for (int k = 0; k < tempTargetPositions.Length; k++)
            {
                float distance = Vector3.Distance(pivotRepPosition, tempTargetPositions[k]);
                if (!isOccupied[atomsIndexes[k]] && distance < minDistance)
                {
                    minDistance = distance;
                    closestAtom = k;
                }
            }

            if (pivotRepPosition == Center)
                centralCellAtom = atoms[atomsIndexes[closestAtom]];

            isOccupied[atomsIndexes[closestAtom]] = true;
            targetPositions[atomsIndexes[closestAtom]] = pivotRepPosition + (d + Vector3.one);
            ris.Add(atoms[atomsIndexes[closestAtom]].gameObject);
            atomRelatedCellPositions.Add(i);
        }

        return ris;
    }
    
    private IEnumerator MoveAllToTargetPositions(Vector3[] targetPositions)
    {
        for (int i = 0; i < atoms.Count; i++)
        {
            StartCoroutine(MoveToTargetPosition(atoms[i], atoms[i].transform.position, targetPositions[i], 1f));
            yield return new WaitForSeconds(0.04f);
        }

        yield return null;
    }
    
    private IEnumerator MoveToTargetPosition(Atom a, Vector3 start, Vector3 end, float duration)
    {
        for (float t = 0; t <= duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            a.transform.position = Vector3.Lerp(start, end, EaseInOutExpo(normalizedTime));
            detector.SetDirty();
            yield return null;
        }

        a.transform.position = end;
        detector.SetDirty();
        atomInPositionCrystalCounter++;
    }
    
    private IEnumerator ChangeAtomsType(List<GameObject> futurePivots)
    {
        yield return new WaitUntil(() => atomInPositionCrystalCounter == 53);
        foreach (var futurePivot in futurePivots)
        {
            ChangeToPivotAtomRep(futurePivot);
        }
    }
    
    private void ChangeToPivotAtomRep(GameObject a)
    {
        // - Tolgo script Atom
        //Destroy(a.GetComponent<Atom>());
        a.GetComponent<Atom>().enabled = false;
        a.GetComponent<Atom>().cell = cells[newPivots.IndexOf(a)];
        a.GetComponent<Collider>().enabled = false;
        // - Tolgo rigidbody
        Destroy(a.GetComponent<Rigidbody>());
        // - Cambio materiale da Red Atom a Pivot Atom Rep
        a.GetComponent<Renderer>().sharedMaterial = pivot.GetComponent<Renderer>().sharedMaterial;
    }
    
    private void SpawnCells()
    {
        // SPAWN CELLE RIPETUTE
        Debug.Log(M);
        foreach (var pos in pivotRepPositions)
        {
            var cellTemp = Instantiate(cell, pos, Quaternion.identity, moleculeSpace.transform);
            cellTemp.transform.localScale *= K;
            cellTemp.transform.rotation = moleculeSpace.transform.rotation;
            cells.Add(cellTemp);
        }
    }
    
    
    private float EaseInOutExpo(float x) {
        return x == 0
            ? 0
            : Math.Abs(x - 1) < 0.0001f
                ? 1
                : x < 0.5 ? Mathf.Pow(2, 20 * x - 10) / 2
                    : (2 - Mathf.Pow(2, -20 * x + 10)) / 2;
    }

    //------------------------------------------------------------------

    private IEnumerator DeactivateCrystal()
    {
        atoms = oldAtoms; // OLDATOMS AVRA RIFERIMENTI A ATOM DISTRUTTI, DA RISOLVERE
        
        //sposto gli atomi in una posizione random dentro la cella
        Vector3[] targetPositions = RandomPositionsInsideCell();
        yield return StartCoroutine(MoveAll(targetPositions));
        
        //resetto il tipo
        
        
        //distruggo le celle
        foreach (var c in cells)
        {
            Destroy(c);
        }
        cells.Clear();
        
        //resetto i parametri
        //atoms = oldAtoms; // OLDATOMS AVRA RIFERIMENTI A ATOM DISTRUTTI, DA RISOLVERE
        detector.TransitionHappened = false;
        detector.SetDirty();

        crystalActivated = false;
        GameStart = true;
        
        
    }

    private Vector3[] RandomPositionsInsideCell()
    {
        Vector3[] ris = new Vector3[M*N-1];
        for (int i = 0; i < oldAtoms.Count; i++)
        {
            Vector3 cellPosition = oldAtoms[i].cell.transform.position;
            ris[i] = new Vector3(Random.Range(cellPosition.x - 2, cellPosition.x + 2),
                                    Random.Range(cellPosition.y - 2, cellPosition.y + 2),
                                    Random.Range(cellPosition.z - 2, cellPosition.z + 2));

        }
        return ris;
    }

    private IEnumerator MoveAll(Vector3[] targetPositions)
    {
        for (int i = 0; i < oldAtoms.Count; i++)
        {
            StartCoroutine(MoveToTargetPosition(atoms[i], atoms[i].transform.position, targetPositions[i], 1f));
            yield return new WaitForSeconds(0.04f);
        }

        yield return null;
    }
     
    
    //------------------------------------------------------------------

    /*
    private IEnumerator Whistle()
    {
        
        solutionManager = FindObjectOfType<SolutionManager>();
        GameObject[] targetAtoms = solutionManager.AllAtoms.ToArray();

        List<Atom> centralCellAtoms = GetCentralCellAtoms(targetAtoms);
        
        //MoveAtomsInCrystalPositions(targetAtoms);
        //changeatomstype deve partire solo quando tutti gli atomi sono in posizione
        yield return StartCoroutine(ChangeAtomsType(targetAtoms, centralCellAtoms));
        
        
        //il numero di atomi nella lista Atoms deve passare da 53 a 1, l'unico atomo della cella centrale
        atoms = centralCellAtoms;
        
        //passare allo shader i parametri della modalita cristallo
        detector.TransitionHappened = true;
        detector.SetDirty();
        
        //spawn delle celle attorno agli atomi
        var centralCellTemp = Instantiate(centralCell, centralCellSpawnPos, Quaternion.identity, moleculeSpace.transform);
        centralCellTemp.transform.localScale *= K;
        SpawnRepeatedCells(centralCellTemp);
        
        crystalActivated = true;
        GameStart = true; //riattivo il trascinamento degli atomi

    }
    
    private IEnumerator ChangeAtomsType(GameObject[] targetAtoms, List<Atom> centralCellAtoms)
    {
        yield return StartCoroutine(MoveAtomsInCrystalPositions(targetAtoms));
        yield return new WaitUntil(() => atomInPositionCrystalCounter == 53);
        Debug.Log("finito gli spostamenti");
        for (var i = 0; i < atoms.Count; i++)
        {
            var a = atoms[i];
            var t = targetAtoms[i];

            if (t.GetComponent<PivotAtomRepSol>() != null)
                ChangeToPivotAtomRep(a.gameObject);

            if (t.GetComponent<AtomRepSolution>() != null)
                ChangeToAtomRep(a.gameObject, centralCellAtoms);
            
        }

        yield return null;
    }

    private IEnumerator MoveAtomsInCrystalPositions(GameObject[] targetAtoms)
    {
        for (var i = 0; i < atoms.Count; i++)
        {
            var a = atoms[i];
            var t = targetAtoms[i];

            //SPOSTA GLI ATOMI IN POSIZIONE
            Vector3 endAtomsPosition = t.transform.position + new Vector3(0, 0, 30);
            //Vector3 endAtomsPosition = new Vector3(Random.Range(20f, 24f), Random.Range(4.6f, 6.6f), Random.Range(8f, 12f));
            Vector3 endPivotsPosition = t.transform.position + new Vector3(0, 0, 30);

            if (t.GetComponent<SolutionAtom>() != null)
            {
                //a.transform.position = endAtomsPosition;
                StartCoroutine(MoveToTargetPosition(a, a.transform.position, endAtomsPosition, 2f));
            }

            if (t.GetComponent<PivotAtomRepSol>() != null)
            {
                //a.transform.position = endPivotsPosition;
                StartCoroutine(MoveToTargetPosition(a, a.transform.position, endPivotsPosition, 2f));
            }

            if (t.GetComponent<AtomRepSolution>() != null)
            {
                //a.transform.position = endAtomsPosition;
                StartCoroutine(MoveToTargetPosition(a, a.transform.position, endAtomsPosition, 2f));
            }

        }
        yield return null;

    }

    
    
    IEnumerator MoveToTargetPosition(Atom a, Vector3 start, Vector3 end, float duration)
    {
        for (float t = 0; t <= duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            a.transform.position = Vector3.Lerp(start, end, normalizedTime);
            detector.SetDirty();
            yield return null;
        }

        a.transform.position = end;
        detector.SetDirty();
        atomInPositionCrystalCounter++;
    }

    private List<Atom> GetCentralCellAtoms(GameObject[] targetAtoms)
    {
        List<Atom> centralAtoms = new List<Atom>();

        for (var i = 0; i < atoms.Count; i++)
        {
            var a = atoms[i];
            var t = targetAtoms[i];
            
            if (t.GetComponent<SolutionAtom>() != null)
                centralAtoms.Add(a);
        }

        return centralAtoms;
    }

    private void ChangeToPivotAtomRep(GameObject a)
    {
        // - Tolgo script Atom
        Destroy(a.GetComponent<Atom>());
        // - Tolgo rigidbody
        Destroy(a.GetComponent<Rigidbody>());
        // - Cambio materiale da Red Atom a Pivot Atom Rep
        a.GetComponent<Renderer>().sharedMaterial = pivotRep.GetComponent<Renderer>().sharedMaterial;
    }
    
    private void ChangeToAtomRep(GameObject a, List<Atom> centralAtoms)
    {
        // - Tolgo script Atom e metto script Atom Rep
        Destroy(a.GetComponent<Atom>());
        a.AddComponent<AtomRep>().materials = new []{atomRep.GetComponent<Renderer>().sharedMaterial, selectedAtomRep};
        
        // - Tolgo rigidbody e collider
        Destroy(a.GetComponent<Rigidbody>());
        Destroy(a.GetComponent<SphereCollider>());
        // - Cambio materiale da Red Atom a Atom Rep
        a.GetComponent<Renderer>().sharedMaterial = atomRep.GetComponent<Renderer>().sharedMaterial;
        // - assegno il padre corrispondente nella cella centrale
        a.transform.parent = centralAtoms[0].transform; //(per ora assumo un unico atomo nella cella centrale)
        a.GetComponent<AtomRep>().Parent = centralAtoms[0].GetComponent<Atom>();

    }
    */
    
}