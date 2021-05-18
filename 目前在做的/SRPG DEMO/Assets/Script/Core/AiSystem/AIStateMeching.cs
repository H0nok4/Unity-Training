using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIStateMeching : MonoBehaviour
{
    public AIState globalState;
    public AIState curState;
    public AIState preState;
    public SrpgClass srpgClass;

    public AIStateMeching(SrpgClass srpgClass)
    {
        this.srpgClass = srpgClass;
        curState = null;
        preState = null;
        globalState = null;
    }

    public void InitStateMeching(SrpgClass srpgClass)
    {
        this.srpgClass = srpgClass;
        curState = null;
        preState = null;
        globalState = null;
    }

    public void SetCurrentState(AIState aiState)
    {
        curState = aiState;
    }

    public void HandleUpdate()
    {
        if (globalState != null)
            StartCoroutine(globalState.Execute(srpgClass));
        if (curState != null)
        {
            StartCoroutine(curState.Execute(srpgClass));
            Debug.Log("Execute Footman_Attack!");
        }
    }

    public void ChangeCurState(AIState state)
    {
        curState.Exit(srpgClass);
        preState = curState;
        curState = state;
        curState.Enter(srpgClass);
    }

    public void RevertToPreState()
    {
        ChangeCurState(preState);
    }
    

}

