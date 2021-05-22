using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState
{
    public BattleManager battleManager = BattleManager.instance;
    public PathFinder pathFinder = GameObject.Find("BattleManager").GetComponent<PathFinder>();
    public ScenceManager scenceManager = ScenceManager.instance;

    public virtual void Enter(SrpgClassUnit sprgClass)
    {

    }

    public virtual IEnumerator Execute(SrpgClassUnit sprgClass)
    {
        yield break;
    }

    public virtual void Exit(SrpgClassUnit srpgClass)
    {

    }

    public virtual void ReciveMessager(Message message)
    {

    }

    public virtual void SendMessage()
    {

    }
}

public class Message
{
    public string messageName;//信息内容
    public string targetName;
    public int lifeTime;
    //TO DO:接受信息的目标
    //TO DO:信息处理的生命周期
}