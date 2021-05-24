using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageScenario
{
    [SerializeField] string StartScenario;
    [SerializeField] string EndScenario;
    [SerializeField] Goal stageGoal;
    [SerializeField] List<StageEvent> events;
}

