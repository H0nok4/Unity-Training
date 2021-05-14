using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIManager : MonoBehaviour
{
    public static Dictionary<string, AIState> aiStateDic { get; set; } = new Dictionary<string, AIState>();
    public ScenceManager scenceManager;
    public bool isAIRunning = false;

    private void Awake()
    {
        scenceManager = GetComponent<ScenceManager>();
    }

    public void HandleUpdate()
    {
        if(isAIRunning == false)
        {
            var curSrpgClass = FindActionAIClass();
            StartCoroutine(RunAITurn(curSrpgClass));
        }

    }

    public SrpgClass FindActionAIClass()
    {
        foreach(var aiClass in scenceManager.enemyClasses)
        {
            if(aiClass.IsActived == false)
            {
                return aiClass;
            }
        }

        return null;
    }

    IEnumerator RunAITurn(SrpgClass actionAIClass)
    {
        if (actionAIClass == null) yield break;
        isAIRunning = true;
        yield return new WaitForSeconds(1f);
        actionAIClass.StateMeching.HandleUpdate();
        yield return new WaitForSeconds(1f);
        while (actionAIClass.isRunningAI)
        {
            yield return new WaitForSeconds(0.1f);
        }
        isAIRunning = false;
    }

    public bool CheckEnemyTurnEnd()
    {
        foreach(var enemyClass in scenceManager.enemyClasses)
        {
            if (enemyClass.IsActived == false)
            {
                return false;
            }
        }

        return true;
    }

}
