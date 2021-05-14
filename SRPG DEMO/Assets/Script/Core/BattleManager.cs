using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BattleStat
{
    BattleStart,
    PlayerCharacterSelect, 
    PlayerActionSelect,
    PlayerItemSelect,
    PlayerSkillSelect,
    PlayerAttackSelect,
    PlayerMoveSelect,
    Wait,
    EnemyTurn,
    PlayerLoseEnd,
    PlayerWinEnd
}
public class BattleManager : MonoBehaviour
{
    public BattleStat battleState;
    public PlayerInputManager playerInputManager;
    public PathFinder pathFinder;
    EnemyAIManager enemyAIManager;

    public SrpgClass curSelectClass;
    public SrpgClass preSelectClass;
    public GameObject actionSelectGameObject;
    public GameObject attackCursorPrefab;
    public GameObject mapObjectBase;
    public Dictionary<Vector3Int, GameObject> attackCursors;
    public Stack<Command> moveCommands = new Stack<Command>();
    public Stack<Command> attackCommands = new Stack<Command>();

    public RectTransform battleUIRectTransform;
    public Goal curLevelGoal;

    public UnityAction onTurnsChange;
    public static BattleManager instence;

    public bool isWalking = false;

    private void Awake()
    {
        enemyAIManager = GetComponent<EnemyAIManager>();
        playerInputManager = GetComponent<PlayerInputManager>();
        pathFinder = GetComponent<PathFinder>();
        enemyAIManager = GetComponent<EnemyAIManager>();
        #region 单例
        if (instence == null)
        {
            instence = this;
        }
        else
        {
            if(instence != this)
            {
                Destroy(gameObject);
            }
        }
        #endregion
    }
    private void Start()
    {
        InitBatlleScence();
        onTurnsChange += CheckBattleEnd;
        ScenceManager.instance.onObjectUnregister += CheckBattleEnd;
        //Temp ↓
        battleState = BattleStat.PlayerCharacterSelect;
        //TO DO：首先检测剧情是否播完，然后进入玩家回合
    }

    public void HandleUpdate()
    {

        if(battleState == BattleStat.PlayerCharacterSelect)
        {
            HandlePlayerSelectSrpgClass();
        }else if(battleState == BattleStat.PlayerMoveSelect)
        {
            HandlePlayerSelectMovePosition();
            HandlePlayerBack();
        }else if(battleState == BattleStat.PlayerActionSelect)
        {
            HandlePlayerSelectAction();
            HandlePlayerBack();
        }else if(battleState == BattleStat.PlayerAttackSelect)
        {
            HandlePlayerSelectAttackPosition();
            HandlePlayerBack();
        }else if (battleState == BattleStat.PlayerItemSelect)
        {
            HandlePlayerItemSelect();
            HandlePlayerBack();
        }else if(battleState == BattleStat.PlayerSkillSelect)
        {
            HandlePlayerSkillSelect();
            HandlePlayerBack();
        }else if(battleState == BattleStat.EnemyTurn)
        {
            if(enemyAIManager.CheckEnemyTurnEnd() == false)
            {
                enemyAIManager.HandleUpdate();
            }
            else
            {
                battleState = BattleStat.PlayerCharacterSelect;
                StartNewPlayerTurn();
            }

        }


        HandleEnemyTurnStart();
    }

    public void HandleEnemyTurnStart()
    {
        if(battleState != BattleStat.EnemyTurn)
        {
            foreach (var playerUnit in ScenceManager.instance.playerClasses)
            {
                if (playerUnit.IsActived == false)
                    return;
            }

            battleState = BattleStat.EnemyTurn;
            //enemyAIManager.DecideState();
        }
    }

    public void StartNewPlayerTurn()
    {
        foreach(var playerClass in ScenceManager.instance.playerClasses)
        {
            playerClass.IsActived = false;
        }

        foreach(var enemyClass in ScenceManager.instance.enemyClasses)
        {
            enemyClass.IsActived = false;
        }

        foreach (var allyClass in ScenceManager.instance.allyClasses)
        {
            allyClass.IsActived = false;
        }

        foreach (var neutralClass in ScenceManager.instance.neutralClasses)
        {
            neutralClass.IsActived = false;
        }
    }

    private void HandlePlayerSelectAttackPosition()
    {
        //TO DO:攻击目标
        if (playerInputManager.GetPlayerLeftMouseKeyDown())
        {

            var cursorPosition = playerInputManager.GetMouseInTilemapPosition(MapManager.instance.tilemaps[1]);
            //↓目标位置有AttackCursor，并且是个SRPGClass
            if (attackCursors.ContainsKey(cursorPosition) && scenceManager.mapObjectGameObjects.ContainsKey(cursorPosition))
            {
                if( scenceManager.mapObjectGameObjects[cursorPosition].GetComponent<SrpgClass>()!=null && scenceManager.mapObjectGameObjects[cursorPosition].GetComponent<SrpgClass>().classCamp == ClassCamp.enemy)
                {
                    ClearAttackCursor();
                    //AttackCommand attackCommand = new AttackCommand(curSelectClass, scenceManager.mapObjectGameObjects[cursorPosition].GetComponent<SrpgClass>());
                    RunTurn(curSelectClass, scenceManager.mapObjectGameObjects[cursorPosition].GetComponent<SrpgClass>());
                    //TO DO：攻击逻辑，然后将当前角色设为Unactive
                    //攻击时应该面向目标
                    //RunTurn(curSelectClass, scenceManager.mapObjectGameObjects[cursorPosition].GetComponent<SrpgClass>());
                    battleState = BattleStat.PlayerCharacterSelect;
                }

            }
            else
            {
                ClearAttackCursor();
                battleState = BattleStat.PlayerActionSelect;
            }
        }
    }

    public void RunTurn(SrpgClass attacker, SrpgClass defender, bool isSkill = false)
    {
        AttackCommand attackCommand = new AttackCommand(attacker, defender);
        attackCommand.Execute();
        attackCommands.Push(attackCommand);

    }
    public Vector3 WorldToUGUI(Vector3 worldPos)
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPos);//世界坐标转换为屏幕坐标
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        screenPoint -= screenSize / 2;//将屏幕坐标变换为以屏幕中心为原点
        Vector2 anchorPos = screenPoint / screenSize * battleUIRectTransform.sizeDelta;//缩放得到UGUI坐标
        return anchorPos;
    }

    public void SetCurSelectClass(Vector3Int ClassPosition)
    {
        preSelectClass = curSelectClass;
        curSelectClass = scenceManager.GetClassInVector3Int(ClassPosition);
    }

    public void InitBatlleScence()
    {
        battleState = BattleStat.BattleStart;
        scenceManager.InitClass();
        scenceManager.InitAllMapObject();
        //播放剧本
    }

    public void HandlePlayerSelectMovePosition()
    {
        if (playerInputManager.GetPlayerLeftMouseKeyDown())
        {

            var cursorPosition = playerInputManager.GetMouseInTilemapPosition(MapManager.instance.tilemaps[1]);
            if (pathFinder.MoveCursors.ContainsKey(cursorPosition))
            {
                battleState = BattleStat.Wait;
                pathFinder.ClearMoveCursors();
                MoveCommand moveCommand = new MoveCommand(curSelectClass, curSelectClass.m_Position, cursorPosition);
                moveCommand.Execute();
                moveCommands.Push(moveCommand);
                //StartCoroutine(curSelectClass.StartPathMove(pathFinder.AstarCreatMovePath(curSelectClass.m_Position, cursorPosition)));
                //临时↓
                battleState = BattleStat.PlayerActionSelect;
            }
            else
            {
                pathFinder.ClearMoveCursors();
                battleState = BattleStat.PlayerCharacterSelect;
            }
        }
    }

    public void HandlePlayerBack()
    {
        if (playerInputManager.GetPlayerRightMouseKeyDown())
        {
             if(battleState == BattleStat.PlayerMoveSelect)//取消移动选择
            {
                pathFinder.ClearMoveCursors();
                battleState = BattleStat.PlayerCharacterSelect;
            }else if(battleState == BattleStat.PlayerAttackSelect)//取消攻击选择
            {
                ClearAttackCursor();
                battleState = BattleStat.PlayerActionSelect;
            }else if(battleState == BattleStat.PlayerActionSelect)//撤销移动
            {
                if (curSelectClass.animator.isWalked) 
                    return;
                actionSelectGameObject.SetActive(false);
                var preMoveCommand = moveCommands.Pop();
                preMoveCommand.Un_Do();
                battleState = BattleStat.PlayerCharacterSelect;
            }
        }
    }

    public void HandlePlayerSelectSrpgClass()
    {
        if (playerInputManager.GetPlayerLeftMouseKeyDown())
        {
            var cursorPostion = playerInputManager.GetMouseInTilemapPosition(MapManager.instance.tilemaps[1]);
            SetCurSelectClass(cursorPostion);
            if (curSelectClass != null && curSelectClass.IsActived == false && curSelectClass.classCamp == ClassCamp.player)
            {
                battleState = BattleStat.PlayerMoveSelect;
                pathFinder.ClearMoveCursors();
                GetCurSrpgMoveRenge();
                pathFinder.DeleteAlreadyUseMoveCursors(scenceManager,curSelectClass.m_Position);
            }
            else
            {
                pathFinder.ClearMoveCursors();
            }
        }
    }

    public void HandlePlayerItemSelect()
    {

    }

    public void HandlePlayerSkillSelect()
    {

    }

    public void HandlePlayerSelectAction()
    {
        if(curSelectClass != null && curSelectClass.classCamp == ClassCamp.player)
        {
            if (!isWalking)
            {
                //TO DO:显示一个UI在这个角色旁边，反之如果没有或者不是玩家控制的则会隐藏或者只在下层UI显示这个角色的属性
                //Detail:UI的位置取决于角色的位置，并且倾向于显示在离屏幕中心点近的位置。
                //可以使用角色的位置到屏幕中心点画一条向量，向量的距离为UI离角色的距离，以向量的结束点为目标位置。
                if (actionSelectGameObject.activeSelf == false)
                    SetActionSelecterActive();


            }

        }
    } 


    public void Action_Attack()
    {
        Debug.Log("Action_Attack");
        battleState = BattleStat.PlayerAttackSelect;
        actionSelectGameObject.SetActive(false);
        CreatAttackRenge(curSelectClass.Weapon.attackRenge);
    }
    public void Action_Item()
    {
        Debug.Log("Action_Item");
        battleState = BattleStat.PlayerItemSelect;
        actionSelectGameObject.SetActive(false);
        //TO DO:显示物品菜单
    }
    public void Action_Wait()
    {
        curSelectClass.IsActived = true;
        actionSelectGameObject.SetActive(false);
        battleState = BattleStat.PlayerCharacterSelect;
    }
    public void Action_Skill()
    {
        Debug.Log("Action_Skill");
        battleState = BattleStat.PlayerSkillSelect;
        actionSelectGameObject.SetActive(false);
        //TO DO:显示技能选择栏
    }
    public void SetActionSelecterActive()
    {
        actionSelectGameObject.SetActive(true);
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 characterPos = curSelectClass.m_Position;

        Vector3 uiVector = (characterPos - 2 * Vector3.Normalize(characterPos - screenPos));
        Vector3 finalPos = WorldToUGUI(uiVector);
        actionSelectGameObject.transform.localPosition = finalPos;
    }

    public void GetCurSrpgMoveRenge()
    {
        pathFinder.CreatMoveRenge(curSelectClass);
    }

    public void CreatAttackRenge(int[][] weaponAttackRenge)
    {
        attackCursors = new Dictionary<Vector3Int, GameObject>();
        int n = weaponAttackRenge.Length;
        int center = n / 2;
        for (int i = 0; i < n; i++)
        {
            for(int j = 0; j < n; j++)
            {
                Vector3Int cursorPos = new Vector3Int(curSelectClass.m_Position.x + (i - center),curSelectClass.m_Position.y + (center - j),0);
                if(weaponAttackRenge[i][j] == 1)
                    CreatAttackCursor(cursorPos);
            }
        }
    }

    public void CreatAttackCursor(Vector3Int attackCursorPos)
    {
        var attackCursorObject = Instantiate(attackCursorPrefab, mapObjectBase.transform);
        attackCursorObject.transform.position = attackCursorPos;
        attackCursors.Add(attackCursorPos, attackCursorObject);
    }

    public void ClearAttackCursor()
    {
        if (attackCursors != null)
        {
            foreach (var kvp in attackCursors)
            {
                DestroyImmediate(kvp.Value.gameObject);
            }
            attackCursors.Clear();
        }
    }

    public void CheckBattleEnd()
    {
        //根据当前的关卡目标，判断当前的关卡战斗是否结束
        switch (curLevelGoal.winTarget)
        {
            case WinTarget.Kill_All_Enemy:
                if(scenceManager.enemyClasses.Count <= 0)
                {
                    battleState = BattleStat.PlayerWinEnd;
                    //TO DO:战斗结束，玩家胜利
                }
                break;
            case WinTarget.Kill_Target_Enemy:
                if (!scenceManager.enemyClasses.Contains(curLevelGoal.winClassTarget))
                {
                    battleState = BattleStat.PlayerWinEnd;
                }
                break;
            case WinTarget.Wait_For_Turns:
                //TO DO:记录回合数，回合变动时会调用Check函数，判断一下回合数是否等于目标
                break;
            //TO DO:可占领的MapObject还没有制作
            default:
                break;
        }

        switch (curLevelGoal.loseTarget)
        {
            case LoseTarget.All_Class_Dead:
                if(scenceManager.playerClasses.Count <= 0)
                {
                    battleState = BattleStat.PlayerLoseEnd;
                    //TO DO:战斗结束，玩家失败
                }
                break;
            case LoseTarget.Target_Killed:
                if (scenceManager.allyClasses.Contains(curLevelGoal.loseClassTarget))
                {
                    battleState = BattleStat.PlayerLoseEnd;
                }
                break;
            case LoseTarget.Wait_For_Turns:
                //TO DO:和上面一样
                break;
            default://同上
                break;
        }
         
    }

}
