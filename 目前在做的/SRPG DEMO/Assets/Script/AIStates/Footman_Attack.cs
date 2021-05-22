using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Footman_Attack : AIState
{
    /* Attack状态下，AI会优先寻找最近的敌人攻击
     * Execute方法具体逻辑：先找到最近的一个敌人设置为target，获取该敌人的位置，然后和自己的坐标求得切比雪夫距离。
     * 然后遍历每个可以行走的位置，并且计算每个位置与敌人的曼哈顿距离，如果比原始位置近的，该位置的得分 += 距离差值
     * 在遍历位置的时候还要判断当前位置是否能够攻击到目标，如果可以，估算攻击的值（估算公式：总伤害 = 自己的攻击 * 目标防御）
     * ReciveMessager方法具体逻辑:当接收到消息"Msg_HelperMe!"的时候，将当前状态改变为帮助目标状态
     */
    private static Footman_Attack instance;
    public static Footman_Attack Instance()
    {
        if(instance == null) instance = new Footman_Attack();
        return instance;
    }


    public override void Enter(SrpgClassUnit srpgClass)
    {
         
    }

    public override IEnumerator Execute(SrpgClassUnit srpgClass)
    {
        Debug.LogWarning("Start");
        srpgClass.isRunningAI = true;
        var moveRenge =  pathFinder.CreatAIMoveRenge(srpgClass);
        Debug.LogWarning("creat AI renge");

        SrpgClassUnit target = null;
        //Start 找到最近的目标
        target = pathFinder.DijsktraFindPlayerClass(srpgClass.m_Position);
        int minDis = Math.Abs(srpgClass.m_Position.x - target.m_Position.x) + Math.Abs(srpgClass.m_Position.y - target.m_Position.y);
        Debug.Log(target.srpgclass.srpgClassName);
        //End
        Debug.LogWarning("Find nearest target");
        var aStarPath = pathFinder.AstarCreatMovePath(srpgClass.m_Position, target.m_Position);
        foreach(var movePos in aStarPath)
        {
            //如果常规的检测距离，AI有时候会呆呆得卡在墙角想打墙后的玩家，所以加入一个A*临时创建的路径到目标，A*路径经过的格子会有额外加分，然后AI就可以绕过障碍物去寻找玩家了
            if (moveRenge.ContainsKey(movePos.m_Position))
            {
                moveRenge[movePos.m_Position] += (3 + 2 * (Math.Abs(movePos.m_Position.x - srpgClass.m_Position.x) + Math.Abs(movePos.m_Position.y - srpgClass.m_Position.y)));
            }
        }
        Debug.LogWarning("Creat A*Path");
        Dictionary<Vector3Int, AttackOrder> attackOrders = new Dictionary<Vector3Int, AttackOrder>();
        Dictionary<Vector3Int, int> tempScoreDic = new Dictionary<Vector3Int, int>();
        foreach(var movePos in moveRenge)
        {
            //遍历格子，可以攻击到玩家的角色的格子有额外加分，大约可以击杀玩家的角色的格子还有额外的加分，这样子AI就可以挑出玩家残血的单位收掉
            int curDis = Math.Abs(movePos.Key.x - target.m_Position.x) + Math.Abs(movePos.Key.y - target.m_Position.y);
            tempScoreDic.Add(movePos.Key, (minDis - curDis));
            var attackOrder = DetectingCanAttackPos(movePos.Key, srpgClass.Weapon.attackRenge,srpgClass);

            if(attackOrder != null)
            {
                attackOrders.Add(movePos.Key, attackOrder);
                tempScoreDic[movePos.Key] += attackOrder.score;
            }

        }
        Debug.LogWarning("Find can AttackPos");
        foreach (var score in tempScoreDic)
        {
            moveRenge[score.Key] += score.Value;
        }

        int maxScore = 0;
        foreach(var kvp in moveRenge)
        {
            maxScore = Math.Max(kvp.Value, maxScore);
        }
        Debug.LogWarning("Calculate score");
        //找到得分最高的位置，然后看看目标位置是否有AttackOrder存在，如果有的话代表该位置需要攻击敌人，没有的话就是直接移动到该位置即可
        foreach (var kvp in moveRenge)
        {
            if(kvp.Value == maxScore)
            {
                Debug.LogWarning("attack target");
                if (attackOrders.ContainsKey(kvp.Key))
                {
                    //执行AttackOrder的命令，攻击目标
                    yield return srpgClass.StartPathMove(pathFinder.AstarCreatMovePath(srpgClass.m_Position, attackOrders[kvp.Key].pos));
                    SrpgClassUnit targetClass = scenceManager.mapObjectGameObjects[attackOrders[kvp.Key].targetPos].GetComponent<SrpgClassUnit>();
                    //攻击目标
                    if (srpgClass.isMoveingPath != true)
                    {
                        yield return new WaitForSeconds(0.5f);
                        battleManager.RunTurn(srpgClass, targetClass);
                    }

                    break;

                }
                else
                {
                    Debug.LogWarning("Move to");
                    //移动范围内没有攻击目标的命令，直接移动到目的位置。
                    yield return srpgClass.StartPathMove(pathFinder.AstarCreatMovePath(srpgClass.m_Position, kvp.Key));
                    break;
                }
            }
        }
            
        if(srpgClass.CurHealth / (float)srpgClass.maxHealth <= 0.3f)
        {
            //TO DO:血量低下，改变状态为寻找治疗者或者使用加血道具
            srpgClass.StateMeching.ChangeCurState(Footman_RunAway.Instance());
        }
        Debug.LogWarning("Running end");
        srpgClass.IsActived = true;
        srpgClass.isRunningAI = false;
    }

    public override void Exit(SrpgClassUnit srpgClass)
    {

    }

    public override void ReciveMessager(Message message)
    {
        
    }

    public override void SendMessage()
    {

    }
    public SrpgClassUnit FindNearestPlayerUnit(Vector3Int startPos)
    {
        SrpgClassUnit target = null;
        int minDis = int.MaxValue;
        foreach(var playerUnit in scenceManager.playerClasses)
        {
            var path = pathFinder.AstarCreatMovePath(startPos, playerUnit.m_Position);
            if(path[path.Count - 1].F < minDis)
            {
                minDis = path[path.Count - 1].F;
                target = playerUnit;
            }
        }

        return target;
    }


    public AttackOrder DetectingCanAttackPos(Vector3Int attackPos,int[][] attackRenge,SrpgClassUnit attacker)
    {
        //发现该位置是个可互动位置，不要走到那个位置攻击，可能会踩到传送门传送走，很憨
        if (scenceManager.interactiveObjectGameObjects.ContainsKey(attackPos))
        {
            return null;
        }
        int attackCenter = attackRenge.Length / 2;
        //List<AttackOrder> attackOrders = new List<AttackOrder>();
        PriorityQueue<AttackOrder> attackOrders = new PriorityQueue<AttackOrder>(new SortByScore());
        for(int i = 0; i < attackRenge.Length; i++)
        {
            for(int j = 0; j < attackRenge.Length; j++)
            {
                if(attackRenge[i][j] == 1)
                {
                    var targetPos = new Vector3Int(attackPos.x + (i - attackCenter), attackPos.y + (attackCenter - j), 0);
                    if (scenceManager.mapObjectGameObjects.ContainsKey(targetPos))
                    {
                        if(scenceManager.mapObjectGameObjects[targetPos].GetComponent<SrpgClassUnit>().classCamp != ClassCamp.enemy)
                        {
                            int score = 0;  
                            SrpgClassUnit defender = scenceManager.mapObjectGameObjects[targetPos].GetComponent<SrpgClassUnit>();
                            int damage = (int)(attacker.attack * (100 - defender.defense) / 100f) * (1 + ( 1 - (int)(defender.CurHealth / (float)defender.maxHealth)));
                            score += damage;
                            if (defender.CurHealth - attacker.attack <= 0)
                            {
                                score += 20;
                            }
                            AttackOrder attackOrder = new AttackOrder(attackPos, targetPos, score);
                            attackOrders.Enqueue(attackOrder);
                        }
                    }
                }
            }
        }

        if(attackOrders.Count > 0)
        {
            return attackOrders.Dequeue();
        }

        return null;
    }


}

public class Order
{
    public Vector3Int pos;
    public int score;
}

public class AttackOrder : Order
{
    public AttackOrder(Vector3Int pos,Vector3Int targetPos,int score)
    {
        this.pos = pos;
        this.targetPos = targetPos;
        this.score = score;
    }

    public Vector3Int targetPos;
}

public class SortByScore : IComparer<AttackOrder>
{
    public int Compare(AttackOrder x, AttackOrder y)
    {
        return y.score.CompareTo(x.score);
    }


}