using System;
using System.Collections.Generic;



public class PriorityQueue<T>
{
    IComparer<T> comparer;
    T[] heap;

    public int Count { get; private set; }

    public PriorityQueue(IComparer<T> comparer) : this(16, comparer) { }

    public PriorityQueue(int capacity, IComparer<T> comparer)
    {
        this.comparer = (comparer == null) ? Comparer<T>.Default : comparer;
        this.heap = new T[capacity];
    }

    public void Enqueue(T v)
    {
        if (Count >= heap.Length) 
            Array.Resize(ref heap, Count * 2);

        heap[Count] = v;
        ShiftUp(Count++);
    }

    public T Dequeue()
    {
        var v = Top();
        heap[0] = heap[--Count];
        if (Count > 0) ShiftDown(0);
        return v;
    }

    public T Top()
    {
        if (Count > 0) return heap[0];
        throw new InvalidOperationException("优先队列为空");
    }

    public bool Contains(T _T)
    {
        foreach(T i in heap)
        {
            if (i == null) continue;
            
            if (i.Equals(_T))
            {
                return true;
            }
        }

        return false;
    }

    void ShiftUp(int n)
    {
        var cur = heap[n];
        for (var i = n / 2; n > 0 && comparer.Compare(cur, heap[i]) > 0; n = i, i /= 2)
            heap[n] = heap[i];
        heap[n] = cur;
    }

    void ShiftDown(int n)
    {
        var cur = heap[n];
        for (var i = n * 2; i < Count; n = i, i *= 2)
        {
            if (i + 1 < Count && comparer.Compare(heap[i + 1], heap[i]) > 0) 
                i++;
            if (comparer.Compare(cur, heap[i]) >= 0) break;
            heap[n] = heap[i];
        }
        heap[n] = cur;
    }

    
}