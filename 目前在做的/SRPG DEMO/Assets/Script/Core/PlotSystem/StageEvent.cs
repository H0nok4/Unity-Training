using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface StageEvent
{
     bool isTriggerTurn(int curTurn);
     bool isTriggerCondition();
     void Action();
}
