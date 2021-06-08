using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    void OnNotify(string notify_String,object[] parameter);
}
