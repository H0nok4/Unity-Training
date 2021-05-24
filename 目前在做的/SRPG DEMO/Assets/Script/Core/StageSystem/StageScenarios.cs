using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScenario_Chapter1 : StageScenario
{
    public StageScenario_Chapter1()
    {
        StartScenario = "Scenario_Test";
        EndScenario = "";
        stageGoal = new Goal_KillAllEnemy();
        events = new List<StageEvent>() { new StageEvent_EnemyRetreat() };
    }
}