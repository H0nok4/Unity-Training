using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageScenario
{
    public string StartScenario;
    public string EndScenario;
    public Goal stageGoal;
    public List<StageEvent> events;
}

