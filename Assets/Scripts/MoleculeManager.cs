using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoleculeManager : MonoBehaviour
{
    public AtomsManager AtomsManager { get; set; }
    public SolutionManager SolutionManager { get; set; }

    private Vector3[] allSolutionAtomsPositions;
    private List<int[]> solutionBonds; 
    private List<float> distancesGraph = new List<float>();
    private Vertex[] solutionMST; // i vertici appartenenti all'MST come lista di vertici
    private bool[] markedNodesSolMST;

    private List<Vertex>[] atomsMST;
    
    private readonly Vector3 pivotPos = new Vector3(22, 6.6f, 10);
    private readonly Vector3 pivotPosSol = new Vector3(22, 6.6f, -20);
    private bool active;
    private bool activeGizmo;

    private Atom[] allAtoms;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("molecule active: " + active);
        if (!active) return;
        
        CreateSolutionMST();
        atomsMST = VertexListToAdjencyList(solutionMST);
        atomsMST = VertexListToAdjencyList(Dfs(solutionMST[0])); //MST degli atomi gameplay come lista di adiacenza 
        
        
        active = false;
    }

    private void CreateSolutionMST()
    {
        solutionBonds = CreateAtomsGraph(SolutionManager.GetAtomSpawnPositions());
        solutionMST = GetMST(BondListToWeightedMatrix(solutionBonds, distancesGraph));
        markedNodesSolMST = new bool[solutionMST.Length];
        int totalWeight = 0;
        foreach (Vertex u in solutionMST)
        {
            if (u.Parent < 0) continue;
            Debug.Log($"From father Vertex {u.Parent} to Vertex {u.V}, distance is: {u.Key}");
            totalWeight += u.Key;
        }
    }

    private void OnDrawGizmos()
    {
        //Debug.Log("bonds length: " + bonds.Count);
        //Debug.Log("distancesGraph length: " + distancesGraph.Count);
        Debug.Log("active gizmo: " + activeGizmo);
        if (!activeGizmo) return;
        
        Debug.Log("vertices length: " + solutionMST.Length);

        foreach (var u in solutionMST)
        {
            if (u.Parent < 0) continue;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(allSolutionAtomsPositions[u.Parent] + pivotPosSol, 
                allSolutionAtomsPositions[u.V] + pivotPosSol);
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
        //allSolutionAtomsPositions[atomsPositons.Length] = Vector3.zero; //l'ultimo posto dell'array è del pivot, dovrebbe essere il primo?

        for (int i = 0; i < allSolutionAtomsPositions.Length; i++)
        {
            for (int j = 0; j < allSolutionAtomsPositions.Length; j++)
            {
                if (i==j)
                    continue;
                int[] possibleBond = {i, j};
                //int[] possibleBondReverse = {j, i};
                if (ris.Any(p => p.SequenceEqual(possibleBond))) continue;
                ris.Add(possibleBond);
                Debug.Log("added: " + possibleBond[0] + ", " + possibleBond[1]);
                distancesGraph.Add(Vector3.Distance(allSolutionAtomsPositions[i], allSolutionAtomsPositions[j]));
            }
        }

        foreach (var v in ris)
        {
            Debug.Log(v);
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

        string print = "matrice: ";
        for (int i = 0; i < allSolutionAtomsPositions.Length; i++)
        {
            for (int j = 0; j < allSolutionAtomsPositions.Length; j++)
            {
                print += ris[i][j];
                print += " ";
            }

            print += "\n";
        }
        Debug.Log(print);
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
                
                Debug.Log($"Father {child.Parent}, child {child.V}");

            }
        }
        
        return ris;
    }
    

    private Vertex[] Dfs(Vertex s)
    {
        List<Vertex> ris = new List<Vertex>();
        Vertex radix = new Vertex {V = 0, Parent = -1, Key = 0};
        ris.Add(radix);
        DfsRecursive(ris, s);
        return ris.ToArray();
    }

    private void DfsRecursive(List<Vertex> ris, Vertex v)
    {
        markedNodesSolMST[v.V] = true; //marca il nodo v
        Debug.Log("nodo visitato corrente: " + v.V); //stampa il nodo che sto visitando
        Debug.Log("nodo padre corrente: " + v.Parent);
        
        if (v.Parent != -1) //skippa il pivot
        {
            Vector3 f = AtomsManager.GetAtoms()[v.V].transform.localPosition;
            Vector3 p = AtomsManager.GetAtoms()[v.Parent].transform.localPosition;
            Vector3 d = Vector3.Normalize(f - p);
            Vector3 newF = p + d * v.Key;
            AtomsManager.GetAtoms()[v.V].transform.localPosition = newF;
        }
        
        //Debug.Log("ciao");
        
        foreach (var w in atomsMST[v.V])
        {
            if (!markedNodesSolMST[w.V])
            {
                ris.Add(w);
                DfsRecursive(ris, w);
            }
        }
    }
    
    public void ActiveMolecule()
    {
        active = true;
        activeGizmo = true;
    }
}
