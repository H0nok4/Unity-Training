using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageEvent
{
    public int TriggerTurn;
    public abstract bool isTriggerCondition();
    public abstract void Action();
}
