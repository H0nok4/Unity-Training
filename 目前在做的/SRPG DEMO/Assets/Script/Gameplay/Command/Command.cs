using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    public virtual void Execute()
    {

    }

    public virtual void Un_Do()
    {

    }
}

public class MoveCommand:Command
{
    public SrpgClassUnit srpgClass;
    public Vector3Int prePos;
    public Vector3Int targetPos;
    public float preMoveX;
    public float preMoveY;
    public MoveCommand(SrpgClassUnit srpgClass,Vector3Int curPos,Vector3Int targetPos)
    {
        this.srpgClass = srpgClass;
        this.prePos = curPos;
        this.targetPos = targetPos;
        preMoveX = srpgClass.animator.moveX;
        preMoveY = srpgClass.animator.moveY;
    }

    public override void Execute()
    {
        //StartCoroutine(curSelectClass.StartPathMove(pathFinder.AstarCreatMovePath(curSelectClass.m_Position, cursorPosition)));
        var movePath = GameObject.Find("BattleManager").GetComponent<PathFinder>().AstarCreatMovePath(prePos, targetPos);
        srpgClass.MoveTo(movePath);
    }

    public override void Un_Do()
    {
        srpgClass.IsActived = false;
        srpgClass.m_Position = prePos;
        srpgClass.transform.position = prePos;
        srpgClass.animator.moveX = preMoveX;
        srpgClass.animator.moveY = preMoveY;
        GameObject.Find("BattleManager").GetComponent<ScenceManager>().UpdateMapObjectPosition();
    }
}

public class AttackCommand:Command
{
    public SrpgClassUnit attacker;
    public SrpgClassUnit defender;
    public DamageDetail damageDetail;
    public AttackCommand(SrpgClassUnit attacker,SrpgClassUnit defender)
    {
        this.attacker = attacker;
        this.defender = defender;
    }
    public override void Execute()
    {
        var damageDetail = defender.OnDamaged(attacker, MapManager.instance.GetSrpgTilemapData(defender.m_Position));
        this.damageDetail = damageDetail;
        ShowDamageDetail(defender.m_Position,damageDetail);
        BattleManager.instance.SetUnitActived(attacker);
    }

    public override void Un_Do()
    {

    }

    public void ShowDamageDetail(Vector3Int pos, DamageDetail damageDetail,int lifeTime = 2)
    {
        //TO DO:在被攻击的角色头上显示一个会淡出的伤害文字，根据是否暴击闪避来调整颜色大小，缓慢向上移动淡出
        Debug.Log("Show Damage Detail");
    }
}

public class ItemUseCommand : Command
{
    public SrpgUseableItem m_item;
    public SrpgClassUnit m_target;
    public ItemUseCommand(SrpgUseableItem item,SrpgClassUnit target) {
        m_item = item;
        m_target = target;
    }

    public override void Execute()
    {
        m_item.Execute(m_target);
    }

    public override void Un_Do()
    {
        
    }
}
 