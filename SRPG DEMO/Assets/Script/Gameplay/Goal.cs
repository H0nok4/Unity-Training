using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
[CreateAssetMenu(menuName = "SRPG/Goal", fileName = "New Goal.asset")]
public class Goal : ScriptableObject
{
    public WinTarget winTarget;
    public SrpgClass winClassTarget;
    public int winTurns;
    //TO DO:可占领目标

    public LoseTarget loseTarget;
    public SrpgClass loseClassTarget;
    public int loseTurns;
    //TO DO:可占领目标
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
