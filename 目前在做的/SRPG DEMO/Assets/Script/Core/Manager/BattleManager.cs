using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum BattleStat
{
    BattleStart,
    PlayerCharacterSelect, 
    PlayerActionSelect,
    PlayerItemSelect,
    PlayerItemTargetSelect,
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
    #region 成员变量
    public BattleStat battleState;
    public PlayerInputManager playerInputManager;
    public PathFinder pathFinder;
    EnemyAIManager enemyAIManager;

    public SrpgUseableItem curHoldItem;
    public SrpgClassUnit curSelectClass;
    public SrpgClassUnit preSelectClass;
    public GameObject actionSelectGameObject;
    public GameObject itemSelectGameObject;
    public GameObject attackCursorPrefab;
    public GameObject mapObjectBase;
    #region UI
    public BattleCursorClassUI battleClassUI;
    public BattleTileUI battleTileUI;
    #endregion
    public Dictionary<Vector3Int, GameObject> attackCursors;
    public Dictionary<Vector3Int, GameObject> itemUseCursors;
    public List<SrpgClassUnit> waitUnRegisterSrpgClass = new List<SrpgClassUnit>();
    public Stack<Command> moveCommands = new Stack<Command>();
    public Stack<Command> attackCommands = new Stack<Command>();

    public RectTransform battleUIRectTransform;
    public StageManager stageManager;

    public UnityAction onTurnsChange;
    public static BattleManager instance;

    public bool isWalking = false;
    #endregion

    private void Awake()
    {
        enemyAIManager = GetComponent<EnemyAIManager>();
        playerInputManager = GetComponent<PlayerInputManager>();
        pathFinder = GetComponent<PathFinder>();
        enemyAIManager = GetComponent<EnemyAIManager>();
        stageManager = GetComponent<StageManager>();

        #region 单例
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }
        #endregion
    }
    private void Start()
    {
        InitBatlleScence();
        ScenceManager.instance.onObjectUnregister += stageManager.CheckBattleEnd;
        //Temp ↓
        battleState = BattleStat.PlayerCharacterSelect;
        //TO DO：首先检测剧情是否播完，然后进入玩家回合
    }

    public void HandleUpdate()
    {

        if(battleState == BattleStat.PlayerCharacterSelect)
        {
            HandlePlayerSelectSrpgClass();
            HandlePlayerOpenSrpgClassDetail();
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

            HandlePlayerBack();
        }else if(battleState == BattleStat.PlayerItemTargetSelect)
        {
            HandlePlayerSelectItemUsePosition();
            HandlePlayerBack();
        }
        else if(battleState == BattleStat.PlayerSkillSelect)
        {
            HandlePlayerSkillSelect();
            HandlePlayerBack();
        }else if(battleState == BattleStat.EnemyTurn)
        {
            if(enemyAIManager.CheckEnemyTurnEnd() == false)
            {
                enemyAIManager.HandleUpdate();
            }
            else if(battleState != BattleStat.PlayerWinEnd && battleState != BattleStat.PlayerLoseEnd)
            {
                battleState = BattleStat.PlayerCharacterSelect;
                StartNewPlayerTurn();
            }
            else
            {
                stageManager.CheckBattleEnd();
            }

        }else if(battleState == BattleStat.PlayerWinEnd)
        {

        }else if(battleState == BattleStat.PlayerLoseEnd)
        {

        }

        UpdateBattleUI();
        HandleEnemyTurnStart();
    }

    #region 判断是否开启AI回合
    //输入:无
    //效果:如果玩家的角色都行动过，则开始AI回合
    //输出:无
    public void HandleEnemyTurnStart()
    {
        if(battleState != BattleStat.EnemyTurn && battleState != BattleStat.PlayerLoseEnd)
        {
            foreach (var playerUnit in ScenceManager.instance.playerClasses)
            {
                if (playerUnit.IsActived == false)
                    return;
            }

            battleState = BattleStat.EnemyTurn;
            
        }
    }
    #endregion

    #region 开始新的回合
    //输入:无
    //效果:开启一个新的回合，有关回合数的方法会在这里调用和改变
    //输出:无
    public void StartNewPlayerTurn()
    {
        stageManager.OnTurnsChange();
        for(int i = ScenceManager.instance.playerClasses.Count - 1; i >= 0; i--)
        {
            var playerClass = ScenceManager.instance.playerClasses[i];
            playerClass.buffManager.ReduceBuffDuretionTurn();
            playerClass.IsActived = false;
            OnStartNewTurn(playerClass);
            playerClass.buffManager.RemoveBuff();
        }

        for (int i = ScenceManager.instance.enemyClasses.Count - 1; i >= 0; i--)
        {
            var enemyClass = ScenceManager.instance.enemyClasses[i];
            enemyClass.IsActived = false;
            OnStartNewTurn(enemyClass);
            enemyClass.buffManager.RemoveBuff();
        }

        for (int i = ScenceManager.instance.allyClasses.Count - 1; i >= 0; i--)
        {
            var allyClass = ScenceManager.instance.allyClasses[i];
            allyClass.buffManager.ReduceBuffDuretionTurn();
            allyClass.IsActived = false;
            OnStartNewTurn(allyClass);
            allyClass.buffManager.RemoveBuff();
        }

        for (int i = ScenceManager.instance.neutralClasses.Count - 1; i >= 0; i--)
        {
            var neutralClass = ScenceManager.instance.neutralClasses[i];
            neutralClass.buffManager.ReduceBuffDuretionTurn();
            neutralClass.IsActived = false;
            OnStartNewTurn(neutralClass);
            neutralClass.buffManager.RemoveBuff();
        }

        attackCommands.Clear();
        moveCommands.Clear();
        Debug.Log("New turn");
    }
    #endregion

    private void OnStartNewTurn(SrpgClassUnit unit)
    {
        for (int i = unit.buffManager.buffs.Count - 1; i >= 0 ; i--)
        {
            for (int j = 0; j < unit.buffManager.buffs[i].buffEffects.Count; j++)
            {
                unit.buffManager.buffs[i].buffEffects[j].OnTurnStart(unit);
            }
        }
    }

    public void SetUnitActived(SrpgClassUnit unit)
    {
        for (int i = unit.buffManager.buffs.Count - 1; i >= 0; i--)
        {
            for (int j = 0; j < unit.buffManager.buffs[i].buffEffects.Count; j++)
            {
                unit.buffManager.buffs[i].buffEffects[j].OnTurnEnd(unit);
                Debug.Log(unit.buffManager.buffs[i].curDurationTimes);
            }
        }
        unit.IsActived = true;
    }

    #region 选择道具使用目标
    //输入:无
    //效果:当玩家按下左键的时候，检测位置有没有SrpgClass,有的话再判断是否是合法位置，都是是的话就对目标位置执行物品效果。
    //输出:无
    private void HandlePlayerSelectItemUsePosition()
    {
        if (playerInputManager.GetPlayerLeftMouseKeyDown())
        {
            var cursorPosition = playerInputManager.GetMouseInTilemapPosition(MapManager.instance.tilemaps[1]);
            var isItemUsePosWasLegal = JudgeIsLegalItemUseTarget(cursorPosition);
            if (isItemUsePosWasLegal)
            {
                curHoldItem.Execute(ScenceManager.instance.mapObjectGameObjects[cursorPosition].GetComponent<SrpgClassUnit>());
                curSelectClass.items.Remove(curHoldItem);//移除道具
                ClearAttackCursor(itemUseCursors);//移除范围

                if (battleState != BattleStat.PlayerWinEnd)
                    battleState = BattleStat.PlayerActionSelect;
                //目前使用道具不会结束本回合，如果需要结束回合，只需要将当前单位的isActived设为True然后将战斗状态设为选择单位即可
            }
        }
    }
    #endregion

    #region 选择攻击位置
    //输入:无
    //效果:当玩家按下左键的时候，判断目标位置的敌人是否合法，如果是则进入攻击阶段
    //输出:无
    private void HandlePlayerSelectAttackPosition()
    {
        //TO DO:攻击目标
        if (playerInputManager.GetPlayerLeftMouseKeyDown())
        {

            var cursorPosition = playerInputManager.GetMouseInTilemapPosition(MapManager.instance.tilemaps[1]);
            //↓目标位置有AttackCursor，并且是个SRPGClass
            if (attackCursors.ContainsKey(cursorPosition) && ScenceManager.instance.mapObjectGameObjects.ContainsKey(cursorPosition))
            {
                if(ScenceManager.instance.mapObjectGameObjects[cursorPosition].GetComponent<SrpgClassUnit>()!=null && ScenceManager.instance.mapObjectGameObjects[cursorPosition].GetComponent<SrpgClassUnit>().classCamp == ClassCamp.enemy)
                {
                    ClearAttackCursor(attackCursors);
                    //AttackCommand attackCommand = new AttackCommand(curSelectClass, scenceManager.mapObjectGameObjects[cursorPosition].GetComponent<SrpgClass>());
                    RunTurn(curSelectClass, ScenceManager.instance.mapObjectGameObjects[cursorPosition].GetComponent<SrpgClassUnit>());
                    //TO DO：攻击逻辑，然后将当前角色设为Unactive
                    //攻击时应该面向目标
                    //RunTurn(curSelectClass, scenceManager.mapObjectGameObjects[cursorPosition].GetComponent<SrpgClass>());
                    if(battleState != BattleStat.PlayerWinEnd)
                        battleState = BattleStat.PlayerCharacterSelect;
                }

            }
            else
            {
                ClearAttackCursor(attackCursors);
                battleState = BattleStat.PlayerActionSelect;
            }
        }
    }
    #endregion

    #region 判断使用物品的位置是否合法
    //输入:Tilemap上的Cursor坐标
    //效果:检测目标位置的角色阵营是否是合法的物品使用对象
    //输出:是否合法
    public bool JudgeIsLegalItemUseTarget(Vector3Int pos)
    {
        if (!ScenceManager.instance.mapObjectGameObjects.ContainsKey(pos))
        {
            return false;
        }
        var target = ScenceManager.instance.mapObjectGameObjects[pos].GetComponent<SrpgClassUnit>();
        switch (curHoldItem.m_ItemUseTarget)
        {

            case ItemUseTarget.ally:
                //判断目标是否不为敌人
                if (target != null)
                {
                    if (target.classCamp == ClassCamp.enemy || target.classCamp == ClassCamp.neutral)
                    {
                        return false;
                    }
                    else
                        return true;
                }
                else
                    return false;
            case ItemUseTarget.enemy:
                if (target != null)
                {
                    if (target.classCamp == ClassCamp.ally || target.classCamp == ClassCamp.neutral)
                    {
                        return false;
                    }
                    else
                        return true;
                }
                else
                    return false;
            //判断目标是否不为友方
            default:
                return true;
                //如果目标为自己，一般使用范围中只有自己,所以不用另做额外判断
        }
    }
    #endregion

    #region 打开角色详情
    public void HandlePlayerOpenSrpgClassDetail()
    {
        if (playerInputManager.GetPlayerRightMouseKeyDown())
        {
            Debug.Log("Open Detail");
        }

    }
    #endregion
    public void RunTurn(SrpgClassUnit attacker, SrpgClassUnit defender, bool isSkill = false)
    {
        AttackCommand attackCommand = new AttackCommand(attacker, defender);
        attackCommand.Execute();
        attackCommands.Push(attackCommand);

    }

    #region 更新战斗UI栏
    //输入:无
    //效果:更新战斗的左下角角色UI和右下角TileUI
    //输出:无
    public void UpdateBattleUI()
    {
        battleClassUI.UpdateUI(null);

        var tile = MapManager.instance.GetCursorPositionSrpgTile();
        battleTileUI.UpdateTileUI(tile);

        var mapObject = MapManager.instance.CheckMapObjectOnPlayerCursorPosition();
        if(mapObject != null)
        {
            var srpgClass = mapObject.gameObject.GetComponent<SrpgClassUnit>();
            battleClassUI.UpdateUI(srpgClass);
        }






    }

    #endregion

    #region 世界坐标转换为UGUI坐标
    //输入:世界坐标Vector3
    //效果:输入一个世界坐标位置，可以转换为相同位置的UGUI位置
    //输出:UGUI坐标
    public Vector3 WorldToUGUI(Vector3 worldPos)
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPos);//世界坐标转换为屏幕坐标
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        screenPoint -= screenSize / 2;//将屏幕坐标变换为以屏幕中心为原点
        Vector2 anchorPos = screenPoint / screenSize * battleUIRectTransform.sizeDelta;//缩放得到UGUI坐标
        return anchorPos;
    }
    #endregion

    #region 设置当前选择的角色
    //输入:角色的位置
    //效果:尝试获取角色
    //输出:无
    public void SetCurSelectClass(Vector3Int ClassPosition)
    {
        preSelectClass = curSelectClass;
        curSelectClass = ScenceManager.instance.GetClassInVector3Int(ClassPosition);
    }
    #endregion
    public void InitBatlleScence()
    {
        battleState = BattleStat.BattleStart;
        ScenceManager.instance.InitClass();
        ScenceManager.instance.InitAllMapObject();
        stageManager.OnBattleStart();
    }
    #region 玩家选择移动的位置
    //输入:无
    //效果:玩家点击左键的时候，判断位置是否合法，如果是的话，就移动目前的角色到目标位置
    //输出:无
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
    #endregion

    #region 返回操作
    //输入:无
    //效果:帮助玩家返回，在不同的状态下有不同的返回操作。
    //输出:无
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
                ClearAttackCursor(attackCursors);
                battleState = BattleStat.PlayerActionSelect;
            }else if(battleState == BattleStat.PlayerActionSelect)//撤销移动
            {
                if (curSelectClass.animator.isWalked) 
                    return;
                actionSelectGameObject.SetActive(false);
                var preMoveCommand = moveCommands.Pop();
                //如果该单位这回合触发过可交互物体，就Un_do还原可交互物体，没有的话则跳过
                Command preUnitActionCommand = null;
                if (curSelectClass.unitActionCommands.Count > 0)
                    preUnitActionCommand = curSelectClass.unitActionCommands.Pop();
                if(preUnitActionCommand != null)
                    preUnitActionCommand.Un_Do();

                preMoveCommand.Un_Do();
                battleState = BattleStat.PlayerCharacterSelect;
            }else if(battleState == BattleStat.PlayerItemSelect)
            {
                itemSelectGameObject.SetActive(false);
                SetActionSelecterActive();
                battleState = BattleStat.PlayerActionSelect;
            }else if(battleState == BattleStat.PlayerItemTargetSelect)
            {
                ClearAttackCursor(itemUseCursors);
                SetItemSelecterActive();
                battleState = BattleStat.PlayerItemSelect;
            }
        }
    }
    #endregion

    #region 玩家选择角色
    //输入:无
    //效果:玩家按下左键的时候，判断目标位置的角色是否合法，是的话则进入玩家移动选择阶段
    //输出:无
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
                pathFinder.DeleteAlreadyUseMoveCursors(curSelectClass.m_Position);
            }
            else
            {
                pathFinder.ClearMoveCursors();
            }
        }
    }
    #endregion

    public void HandlePlayerSkillSelect()
    {

    }

    #region 玩家选择行动阶段
    //输入:无
    //效果:当是玩家选择行动的阶段，将行动选择栏显示出来
    //输出:无
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
    #endregion

    #region 物品的使用
    //输入:被使用的物品
    //效果:进入物品目标的选择阶段
    //输出:无
    public void HandleItemUse(SrpgUseableItem item)
    {
        itemSelectGameObject.SetActive(false);
        itemUseCursors = CreatAttackRange(item.m_UseRenge);
        curHoldItem = item;
        battleState = BattleStat.PlayerItemTargetSelect;

    }
    #endregion

    #region 四种行动模式
    //输入:无
    //效果:攻击模式进入攻击目标选择阶段，物品模式打开物品的选择菜单,等待模式结束该回合，技能模式选择技能的目标
    //输出:无
    public void Action_Attack()
    {
        Debug.Log("Action_Attack");
        battleState = BattleStat.PlayerAttackSelect;
        actionSelectGameObject.SetActive(false);
        attackCursors = CreatAttackRange(curSelectClass.Weapon.attackRenge);
    }
    public void Action_Item()
    {
        battleState = BattleStat.PlayerItemSelect;
        actionSelectGameObject.SetActive(false);
        //TO DO:显示物品菜单
        if (itemSelectGameObject.activeSelf == false)
            SetItemSelecterActive();

        itemSelectGameObject.GetComponent<ItemSelectPanel>().InitItemButtons(curSelectClass);
    }
    public void Action_Wait()
    {
        SetUnitActived(curSelectClass);
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

    #endregion

    #region 显示行动选择页面
    //输入:无
    //效果:显示行动选择页面，并且将它移动到行动的角色附近
    //输出:无
    public void SetActionSelecterActive()
    {
        actionSelectGameObject.SetActive(true);
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 characterPos = curSelectClass.m_Position;

        Vector3 uiVector = (characterPos - 2 * Vector3.Normalize(characterPos - screenPos));
        Vector3 finalPos = WorldToUGUI(uiVector);
        actionSelectGameObject.transform.localPosition = finalPos;
    }
    #endregion
    private void SetItemSelecterActive()
    {
        itemSelectGameObject.SetActive(true);
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 characterPos = curSelectClass.m_Position;
        Vector3 uiVector = (characterPos - 2 * Vector3.Normalize(characterPos - screenPos));
        Vector3 finalPos = WorldToUGUI(uiVector);
        itemSelectGameObject.transform.localPosition = finalPos;
    }

    public void GetCurSrpgMoveRenge()
    {
        pathFinder.CreatMoveRenge(curSelectClass);
    }

    #region 创建攻击范围
    //输入:攻击范围数组
    //效果:使用攻击范围数组在目标位置创建攻击范围Cursor
    //输出:创建完毕的范围Cursor
    public Dictionary<Vector3Int, GameObject> CreatAttackRange(int[][] weaponAttackRange)
    {
        var Cursors = new Dictionary<Vector3Int, GameObject>();
        int n = weaponAttackRange.Length;
        int center = n / 2;
        for (int i = 0; i < n; i++)
        {
            for(int j = 0; j < n; j++)
            {
                Vector3Int cursorPos = new Vector3Int(curSelectClass.m_Position.x + (i - center),curSelectClass.m_Position.y + (center - j),0);
                if(weaponAttackRange[i][j] == 1)
                    CreatAttackCursor(cursorPos,Cursors);
            }
        }

        return Cursors;
    }
    #endregion

    public void CreatAttackCursor(Vector3Int attackCursorPos, Dictionary<Vector3Int, GameObject> cursors)
    {
        var attackCursorObject = Instantiate(attackCursorPrefab, mapObjectBase.transform);
        attackCursorObject.transform.position = attackCursorPos;
        cursors.Add(attackCursorPos, attackCursorObject);
    }

    #region 清除所有的Cursor
    //输入:需要被删除的Cursor
    //效果:删除传入的Cursor列表
    //输出:无
    public void ClearAttackCursor(Dictionary<Vector3Int,GameObject> cursors)
    {
        if (cursors != null)
        {
            foreach (var kvp in cursors)
            {
                DestroyImmediate(kvp.Value.gameObject);
            }
            cursors.Clear();
        }
    }
    #endregion




}
