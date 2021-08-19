using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager
{

    public StageScenario curScenario;
    public int curTurn = 0;

    public StageManager(StageScenario curScenario)
    {
        curTurn = 0;
        this.curScenario = curScenario;
    }

    #region 战斗结束触发
    //输入:无
    //效果:战斗结束时触发
    //输出:无
    public void OnBattleOver()
    {
        if (curScenario.EndScenario != string.Empty)
        {
            GameDirecter.instance.PlayScenario(curScenario.EndScenario);
        }
        Debug.Log("PlayerWin");
    }
    #endregion

    #region 战斗开始触发
    //输入:无
    //效果:战斗开始触发
    //输出:无
    public void OnBattleStart()
    {
        if(curScenario.StartScenario != string.Empty)
        {
            GameDirecter.instance.PlayScenario(curScenario.StartScenario);
        }

    }
    #endregion

    #region 回合改变的时候触发事件
    //输入:无
    //效果:当回合改变的时候，将当前回合数+1，检查是否有可以触发的事件
    //输出:无
    public void OnTurnsChange()
    {
        curTurn++;
        for(int i = curScenario.events.Count - 1; i >= 0; i--)
        {
            if(curTurn == curScenario.events[i].TriggerTurn)
            {
                if(curScenario.events[i].isTriggerCondition())
                    curScenario.events[i].Action();
            }
        }
    }
    #endregion

    #region 检测战斗是否结束
    //输入:无
    //效果:在单位死亡，回合数变动的时候检测是否达成了目标，如果是就进入输/赢阶段
    //输出:无
    public void CheckBattleEnd()
    {
        //根据当前的关卡目标，判断当前的关卡战斗是否结束
        switch (curScenario.stageGoal.winTarget)
        {
            case WinTarget.Kill_All_Enemy:
                if (ScenceManager.instance.enemyClasses.Count <= 0)
                {
                    BattleManager.instance.battleState = BattleStat.PlayerWinEnd;
                    //TO DO:战斗结束，玩家胜利
                    OnBattleOver();
                }
                break;
            case WinTarget.Kill_Target_Enemy:
                if (!ScenceManager.instance.enemyClasses.Contains(curScenario.stageGoal.winClassTarget))
                {
                    BattleManager.instance.battleState = BattleStat.PlayerWinEnd;
                }
                break;
            case WinTarget.Wait_For_Turns:

                //TO DO:记录回合数，回合变动时会调用Check函数，判断一下回合数是否等于目标
                break;
            //TO DO:可占领的MapObject还没有制作
            default:
                break;
        }

        switch (curScenario.stageGoal.loseTarget)
        {
            case LoseTarget.All_Class_Dead:
                if (ScenceManager.instance.playerClasses.Count <= 0)
                {
                    BattleManager.instance.battleState = BattleStat.PlayerLoseEnd;
                    Debug.Log("Player lose");
                    //TO DO:战斗结束，玩家失败
                }
                break;
            case LoseTarget.Target_Killed:
                if (ScenceManager.instance.allyClasses.Contains(curScenario.stageGoal.loseClassTarget))
                {
                    BattleManager.instance.battleState = BattleStat.PlayerLoseEnd;
                }

                break;
            case LoseTarget.Wait_For_Turns:

                //TO DO:和上面一样
                break;
            default://同上
                break;
        }


    }

    #endregion

}
