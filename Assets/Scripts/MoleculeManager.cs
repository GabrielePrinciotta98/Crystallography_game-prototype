using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MoleculeManager : MonoBehaviour
{
    [SerializeReference] private GameObject bond;
    [SerializeReference] private GameObject shadow;
    public AtomsManager AtomsManager { get; set; }
    public SolutionManager SolutionManager { get; set; }

    private Vector3[] allSolutionAtomsPositions;
    private Vector3[] allAtomsPositions;

    private GameObject[] atoms;
    private GameObject[] solutionAtoms;

    private List<GameObject> bonds = new List<GameObject>();
    private List<GameObject> bondShadows = new List<GameObject>(); 
    
    private List<int[]> solutionBonds; 
    private List<float> distancesGraph = new List<float>();
    private Vertex[] solutionMST; // i vertici appartenenti all'MST come lista di vertici
    private bool[] markedNodesSolMST;

    private List<Vertex>[] atomsMST;
    
    private readonly Vector3 pivotPos = new Vector3(22, 6.6f, 10);
    private readonly Vector3 pivotPosSol = new Vector3(22, 6.6f, -20);
    private bool active;
    private bool firstActivation = true;

    public bool Activated { get; private set; }
    //private bool activeGizmo;

     
    // Start is called before the first frame update
    void Start()
    {

        allAtomsPositions = new Vector3[AtomsManager.GetN()];
        allAtomsPositions[0] = Vector3.zero;
    }

    private void ParentingMolecule()
    {
        for (int i = 0; i < atomsMST.Length; i++)
        {
            GameObject curFather = atoms[i];

            for (int j = 0; j < atomsMST[i].Count; j++)
            {
                // aggiungi figli a ogni atomo eccetto il pivot
                if (i > 0) curFather.GetComponent<Atom>().molecularChildren.Add(atoms[atomsMST[i][j].V].GetComponent<Atom>()); 
                // aggiungi il padre a ogni atomo 
                atoms[atomsMST[i][j].V].GetComponent<Atom>().molecularParent = atoms[atomsMST[i][j].Parent];
                // aggiungi la distanza dal padre a ogni atomo
                atoms[atomsMST[i][j].V].GetComponent<Atom>().distanceToMolecularParent =
                    Vector3.Distance(atoms[atomsMST[i][j].V].transform.position,
                        atoms[atomsMST[i][j].V].GetComponent<Atom>().molecularParent.transform.position);
            }
        }
    }

    private void DisplayMoleculeBonds()
    {
        for (int i = 0; i < atomsMST.Length; i++)
        {
            GameObject start = atoms[i];
            Vector3 bondStart = start.transform.position;
            for (int j = 0; j < atomsMST[i].Count; j++)
            {
                GameObject end = atoms[atomsMST[i][j].V];
                Vector3 bondEnd = end.transform.position;
                //instanzia tubi dx
                GameObject bondTemp = Instantiate(bond, (bondStart + bondEnd) / 2, Quaternion.identity);
                bonds.Add(bondTemp);
                bondTemp.GetComponent<Bond>().Start = start;
                bondTemp.GetComponent<Bond>().End = end;
                GameObject shadowTemp = Instantiate(shadow);
                bondShadows.Add(shadowTemp);
                //instanzia ombre dx
                shadowTemp.GetComponent<BondShadow>().Start = start;
                shadowTemp.GetComponent<BondShadow>().End = end;
            }

            start = solutionAtoms[i];
            bondStart = start.transform.position;
            for (int j = 0; j < atomsMST[i].Count; j++)
            {
                GameObject end = solutionAtoms[atomsMST[i][j].V];
                Vector3 bondEnd = end.transform.position;
                //instanzia tubi soluzione
                GameObject bondTemp = Instantiate(bond, (bondStart + bondEnd) / 2, Quaternion.identity);
                bonds.Add(bondTemp);
                bondTemp.GetComponent<Bond>().Start = start;
                bondTemp.GetComponent<Bond>().End = end;
                //instanzia ombre soluzione
                GameObject shadowTemp = Instantiate(shadow);
                bondShadows.Add(shadowTemp);
                shadowTemp.GetComponent<BondShadow>().Start = start;
                shadowTemp.GetComponent<BondShadow>().End = end;
            }
        }
    }
    
    
    private void CreateSolutionMST()
    {
        solutionBonds = CreateAtomsGraph(SolutionManager.GetAtomSpawnPositions());
        solutionMST = GetMST(BondListToWeightedMatrix(solutionBonds, distancesGraph));
        markedNodesSolMST = new bool[solutionMST.Length];
        foreach (Vertex u in solutionMST)
        {
            if (u.Parent < 0) continue;
            //Debug.Log($"From father Vertex {u.Parent} to Vertex {u.V}, distance is: {u.Key}");
        }
    }
    
    private List<int[]> CreateAtomsGraph(Vector3[] atomsPositons)
    {
        List<int[]> ris = new List<int[]>();
        allSolutionAtomsPositions = new Vector3[atomsPositons.Length + 1];
        allSolutionAtomsPositions[0] = Vector3.zero; // primo posto riservato al pivot
        for (int i = 1; i < allSolutionAtomsPositions.Length; i++)
        {
            allSolutionAtomsPositions[i] = atomsPositons[i-1];
        }

        for (int i = 0; i < allSolutionAtomsPositions.Length; i++)
        {
            for (int j = 0; j < allSolutionAtomsPositions.Length; j++)
            {
                if (i==j)
                    continue;
                int[] possibleBond = {i, j};
                if (ris.Any(p => p.SequenceEqual(possibleBond))) continue;
                ris.Add(possibleBond);
                distancesGraph.Add(Vector3.Distance(allSolutionAtomsPositions[i], allSolutionAtomsPositions[j]));
            }
        }

        return ris;
    }

    private int[][] BondListToWeightedMatrix(List<int[]> bonds, List<float> distances)
    {
        int[][] ris = new int[allSolutionAtomsPositions.Length][];
        for (int k = 0; k < allSolutionAtomsPositions.Length; k++)
        {
            int[] row = new int[allSolutionAtomsPositions.Length];
            for (int i = 0; i < bonds.Count; i++)
            {
                if (bonds[i][0] != k) continue;
                row[bonds[i][1]] = (int)(distances[i] * 10);
            }

            ris[k] = row;
        }
        
        return ris;
    }
    
    private static Vertex[] GetMST(int[][] graph)
    {
        PriorityQueue<Vertex> queue = new PriorityQueue<Vertex>(true);
        int vertexCount = graph.GetLength(0);
        //listing all vertices
        Vertex[] vertices = new Vertex[vertexCount];
        for (int i = 0; i < vertexCount; i++)
            vertices[i] = new Vertex() { Key = int.MaxValue, Parent = -1, V = i };
        //setting first one's key to zero
        vertices[0].Key = 0;

        //insertingvertices
        for (int i = 0; i < vertexCount; i++)
            queue.Enqueue(vertices[i].Key, vertices[i]);

        while (queue.Count > 0)
        {
            Vertex minVertex = queue.Dequeue();
            int u = minVertex.V;
            vertices[u].IsProcessed = true;
            //alll edges from vertex u
            int[] edges = graph[minVertex.V];
            for (int v = 0; v < edges.Length; v++)
            {
                if (graph[u][v] > 0 && !vertices[v].IsProcessed && graph[u][v] < vertices[v].Key)
                {
                    vertices[v].Parent = u;
                    vertices[v].Key = graph[u][v];
                    //updating priority in queue since key is priority
                    queue.UpdatePriority(vertices[v], vertices[v].Key);
                }
            }
        }
        return vertices;
    }

    private List<Vertex>[] VertexListToAdjencyList(Vertex[] vertexList)
    {
        List<Vertex>[] ris = new List<Vertex>[AtomsManager.GetN()];
        
        //inizializza liste
        for (int i = 0; i < ris.Length; i++)
        {
            ris[i] = new List<Vertex>();
        }
        
        for (int i = 0; i < ris.Length; i++)
        {
            foreach (var vertex in vertexList)
            {
                if (vertex.Parent == i)
                {
                    ris[i].Add(vertexList[vertex.V]);
                }
            }
        }

        foreach (var father in ris)
        {
            
            foreach (var child in father)
            {
                
                Debug.Log($"Father {child.Parent}, child {child.V}, distance {child.Key}");

            }
        }
        
        return ris;
    }
    

    private void Dfs(Vertex s)
    {
        List<Vertex> ris = new List<Vertex>();
        Vertex radix = new Vertex {V = 0, Parent = -1, Key = 0};
        ris.Add(radix);
        DfsRecursive(ris, s);
        //return ris.ToArray();
    }

    private void DfsRecursive(List<Vertex> ris, Vertex v)
    {
        markedNodesSolMST[v.V] = true; //marca il nodo v
        
        // VISITA
        //Debug.Log("nodo visitato corrente: " + v.V); //stampa il nodo che sto visitando
        //Debug.Log("nodo padre corrente: " + v.Parent);
        
        if (v.Parent != -1) //skippa il pivot
            EnforceMolecularBonds(v);

        // VAI AVANTI CON LA VISITA
        foreach (var w in atomsMST[v.V])
        {
            if (!markedNodesSolMST[w.V])
            {
                ris.Add(w);
                DfsRecursive(ris, w);
            }
        }
    }

    private void EnforceMolecularBonds(Vertex v)
    {
        
        Vector3 f = AtomsManager.GetAtoms()[v.V - 1].transform.localPosition;
        Vector3 p;
        if (v.Parent == 0)
            p = Vector3.zero; // il parent è il pivot
        else
            p = AtomsManager.GetAtoms()[v.Parent - 1].transform.localPosition;

        Vector3 d = Vector3.Normalize(f - p);
        Debug.Log(v.Key);
        Vector3 newF = p + d * ((float) v.Key / 10);
        AtomsManager.GetAtoms()[v.V - 1].transform.localPosition = newF;
        allAtomsPositions[v.V] = newF;
        Debug.Log("enforce");
    }

    public void ActivateMolecule(Button molecule, Button moleculeDisabled)
    {
        //active = true;
        MoleculeActivation();
        FindObjectOfType<Detector>().SetDirty();
        //activeGizmo = true;
        Activated = true;
        GameObject.Find("ControlGizmo").GetComponent<ControlGizmo>().isSphere = true;
        molecule.gameObject.SetActive(false);
        moleculeDisabled.gameObject.SetActive(true);

    }

    public void DeactivateMolecule(Button molecule, Button moleculeDisabled)
    {
        molecule.gameObject.SetActive(true);
        moleculeDisabled.gameObject.SetActive(false);
        Activated = false;
        GameObject.Find("ControlGizmo").GetComponent<ControlGizmo>().isSphere = false;
        //TODO disabilita legami
        MoleculeDeactivation();
    }

    private void MoleculeActivation()
    {
        if (firstActivation)
        {
            atoms = new GameObject[AtomsManager.GetN()];
            atoms[0] = AtomsManager.GetPivot();

            for (int i = 0; i < AtomsManager.GetAtoms().Count; i++)
            {
                atoms[i + 1] = AtomsManager.GetAtoms()[i].gameObject;
            }

            solutionAtoms = new GameObject[AtomsManager.GetN()];
            solutionAtoms[0] = SolutionManager.GetPivot();

            for (int i = 0; i < SolutionManager.GetAtoms().Count; i++)
            {
                solutionAtoms[i + 1] = SolutionManager.GetAtoms()[i].gameObject;
            }

            CreateSolutionMST();
            atomsMST = VertexListToAdjencyList(solutionMST);
            Dfs(solutionMST[0]);
            
            DisplayMoleculeBonds();
            // IMPOSTA PARENTELE MOLECOLE
            ParentingMolecule();
            firstActivation = false;
        }
        else
        {
            Debug.Log("second");
            Dfs(solutionMST[0]);
            DisplayMoleculeBonds();
        }
    }

    private void MoleculeDeactivation()
    {
        foreach (var bond in bonds)
            Destroy(bond);
        
        foreach (var shadow in bondShadows)
            Destroy(shadow);
        
        for (int i = 0; i < markedNodesSolMST.Length; i++)
            markedNodesSolMST[i] = false;
    }
}
