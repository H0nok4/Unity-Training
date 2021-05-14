using System;
using System.Collections.Generic;



public class PriorityQueue<T>
{
    IComparer<T> comparer;
    SortedSet<T> sSet;

    public PriorityQueue(){
        sSet = new SortedSet<T>();
    }
    public PriorityQueue(IComparer<T> comparer){
        this.comparer = (comparer == null) ? Comparer<T>.Default : comparer;
        sSet = new SortedSet<T>(comparer);
    }
    public int Count
    {
        get { return sSet.Count; }
    }

    //O(log n)
    public T Dequeue()
    {
        T item = sSet.Max;
        sSet.Remove(item);
        return item;
    }
    //O(log n)
    public void Enqueue(T _T)
    {
        sSet.Add(_T);
    }
    //O(log n)
    public T Max()
    {
        return sSet.Max;
    }
    //O(log n)
    public T Min()
    {
        return sSet.Min;
    }

    public bool Contains(T _T)
    {
        return sSet.Contains(_T);
    }

}

