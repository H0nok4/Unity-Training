using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Goal
{
    public WinTarget winTarget;
    public SrpgClassUnit winClassTarget;
    public int winTurns;
    //TO DO:可占领目标

    public LoseTarget loseTarget;
    public SrpgClassUnit loseClassTarget;
    public int loseTurns;
    //TO DO:可占领目标
}

public class Goal_KillAllEnemy : Goal
{
    public Goal_KillAllEnemy()
    {
        winTarget = WinTarget.Kill_All_Enemy;
        loseTarget = LoseTarget.All_Class_Dead;
    }
}



public enum WinTarget
{
    Kill_All_Enemy,
    Kill_Target_Enemy,
    Wait_For_Turns,
    Hold_Target
}

public enum LoseTarget
{
    All_Class_Dead,
    Target_Killed,
    Wait_For_Turns,
    Target_Holded
}
