using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    class Node
    {
        public int Priority { get; set; }
        public T Object { get; set; }
    }

    //object array
    private readonly List<Node> queue = new List<Node>();
    int heapSize = -1;
    private readonly bool isMinPriorityQueue;
    public int Count => queue.Count;

    /// <summary>
    /// If min queue or max queue
    /// </summary>
    /// <param name="isMinPriorityQueue"></param>
    public PriorityQueue(bool isMinPriorityQueue = false)
    {
        this.isMinPriorityQueue = isMinPriorityQueue;
    }


    /// <summary>
    /// Enqueue the object with priority
    /// </summary>
    /// <param name="priority"></param>
    /// <param name="obj"></param>
    public void Enqueue(int priority, T obj)
    {
        Node node = new Node { Priority = priority, Object = obj };
        queue.Add(node);
        heapSize++;
        //Maintaining heap
        if (isMinPriorityQueue)
            BuildHeapMin(heapSize);
        else
            BuildHeapMax(heapSize);
    }
    
    /// <summary>
    /// Dequeue the object
    /// </summary>
    /// <returns></returns>
    public T Dequeue()
    {
        if (heapSize > -1)
        {
            var returnVal = queue[0].Object;
            queue[0] = queue[heapSize];
            queue.RemoveAt(heapSize);
            heapSize--;
            //Maintaining lowest or highest at root based on min or max queue
            if (isMinPriorityQueue)
                MinHeapify(0);
            else
                MaxHeapify(0);
            return returnVal;
        }
        else
            throw new Exception("Queue is empty");
    }
    
    /// <summary>
    /// Updating the priority of specific object
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="priority"></param>
    public void UpdatePriority(T obj, int priority)
    {
        int i = 0;
        for (; i <= heapSize; i++)
        {
            Node node = queue[i];
            if (ReferenceEquals(node.Object, obj))
            {
                node.Priority = priority;
                if (isMinPriorityQueue)
                {
                    BuildHeapMin(i);
                    MinHeapify(i);
                }
                else
                {
                    BuildHeapMax(i);
                    MaxHeapify(i);
                }
            }
        }
    }
    

    /// <summary>
    /// Maintain max heap
    /// </summary>
    /// <param name="i"></param>
    private void BuildHeapMax(int i)
    {
        while (i >= 0 && queue[(i - 1) / 2].Priority < queue[i].Priority)
        {
            Swap(i, (i - 1) / 2);
            i = (i - 1) / 2;
        }
    }
    /// <summary>
    /// Maintain min heap
    /// </summary>
    /// <param name="i"></param>
    private void BuildHeapMin(int i)
    {
        while (i >= 0 && queue[(i - 1) / 2].Priority > queue[i].Priority)
        {
            Swap(i, (i - 1) / 2);
            i = (i - 1) / 2;
        }
    }

    private void Swap(int i, int j)
    {
        var temp = queue[i];
        queue[i] = queue[j];
        queue[j] = temp;
    }
    private int ChildL(int i)
    {
        return i * 2 + 1;
    }
    private int ChildR(int i)
    {
        return i * 2 + 2;
    }
    
    private void MaxHeapify(int i)
    {
        int left = ChildL(i);
        int right = ChildR(i);

        int heighst = i;

        if (left <= heapSize && queue[heighst].Priority < queue[left].Priority)
            heighst = left;
        if (right <= heapSize && queue[heighst].Priority < queue[right].Priority)
            heighst = right;

        if (heighst != i)
        {
            Swap(heighst, i);
            MaxHeapify(heighst);
        }
    }
    private void MinHeapify(int i)
    {
        int left = ChildL(i);
        int right = ChildR(i);

        int lowest = i;

        if (left <= heapSize && queue[lowest].Priority > queue[left].Priority)
            lowest = left;
        if (right <= heapSize && queue[lowest].Priority > queue[right].Priority)
            lowest = right;

        if (lowest != i)
        {
            Swap(lowest, i);
            MinHeapify(lowest);
        }
    }
}

