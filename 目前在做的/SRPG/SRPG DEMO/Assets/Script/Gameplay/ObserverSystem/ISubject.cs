using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISubject
{
    List<IObserver> observers = new List<IObserver>();

    public void AddObserver(IObserver newObserver)
    {
        observers.Add(newObserver);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void Notify(string notify_String,object[] parameter)
    {
        for(int i = 0; i < observers.Count; i++)
        {
            observers[i].OnNotify(notify_String, parameter);
        }
    }
    
}
