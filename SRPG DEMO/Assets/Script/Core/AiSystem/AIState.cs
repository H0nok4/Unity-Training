using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState
{
    public BattleManager battleManager = GameObject.Find("GameManager").GetComponent<BattleManager>();
    public PathFinder pathFinder = GameObject.Find("GameManager").GetComponent<PathFinder>();
    public ScenceManager scenceManager = GameObject.Find("GameManager").GetComponent<ScenceManager>();

    public virtual void Enter(SrpgClass sprgClass)
    {

    }

    public virtual IEnumerator Execute(SrpgClass sprgClass)
    {
        yield break;
    }

    public virtual void Exit(SrpgClass srpgClass)
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