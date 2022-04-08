public class Vertex
{
    public int Key { get; set; } = int.MaxValue; //peso
    public int Parent { get; set; } = -1; //indice nodo padre
    public int V { get; set; } //nodo corrente
    public bool IsProcessed { get; set; } 
}
