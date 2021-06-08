using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ClassType
{
    hero,
    footman,
    raider,
    caster
}
public enum ClassCamp
{
    player,
    enemy,
    ally,
    neutral
}
public class SrpgClassUnit :  MapObject
{
    #region 成员变量
    [SerializeField] ClassType m_classType;
    [SerializeField] ClassCamp m_classCamp;
    [SerializeField] SrpgClass m_srpgClass;
    [SerializeField] int curHealth;
    [SerializeField] ClassAnimator m_classAnimator;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] GameObject highLightSprite;
    [SerializeField] bool isActived;
    [SerializeField] AIStateMeching m_StateMeching;
    [SerializeField] BuffManager m_buffManager;
    public Stack<Command> unitActionCommands;
    private Slider m_HPSlider;
    #endregion

    #region 属性

    public int Level
    {
        get { return srpgClass.level; }
        set { srpgClass.level = value; }
    }
    public int CurHealth
    {
        get { return curHealth; }
        set { curHealth = value; }
    }
    public SrpgWeapon Weapon
    {
        get { return m_srpgClass.srpgWeapon; }
    }
    public SrpgArmor armor
    {
        get { return m_srpgClass.srpgArmor; }
    }
    public bool IsActived
    {
        get { return isActived; }
        set { isActived = value; }
    }
    public ClassType classType
    {
        get { return m_classType; }
    }
    public bool isRunningAI{ get; set;}
    public bool isWalked { get; set; }
    public bool isMoveingPath { get; set; }
    public int moveCost
    {
        get { return srpgClass.classInfo.classMovePoint; }
    }
    public ClassCamp classCamp
    {
        get { return m_classCamp; }
    }

    private Dictionary<SrpgClassPropertyType, int> m_Property;
    public ClassAnimator animator
    {
        get { return m_classAnimator; }
    }
    public AIStateMeching StateMeching
    {
        get { return m_StateMeching; }
    }
    public List<SrpgUseableItem> items
    {
        get { return srpgClass.items; }
    }
    public SrpgClass srpgClass
    {
        get { return m_srpgClass; }
        set { m_srpgClass = value; }
    }

    public BuffManager buffManager
    {
        get { return m_buffManager; }
    }
    #endregion

    #region 向外暴露的属性
    public int attack
    {
        get { return GetState(SrpgClassPropertyType.Attack); }
    }

    public int defense
    {
        get { return GetState(SrpgClassPropertyType.Defense); }
    }

    public int maxHealth
    {
        get { return GetState(SrpgClassPropertyType.MaxHealth); }
    }

    public int magicAttack
    {
        get { return GetState(SrpgClassPropertyType.MagicAttack); }
    }

    public int magicDefense
    {
        get { return GetState(SrpgClassPropertyType.MagicDefense); }
    }

    public int avoid
    {
        get { return GetState(SrpgClassPropertyType.Avoid); }
    }

    public int critChance
    {
        get { return GetState(SrpgClassPropertyType.CritChance); }
    }

    public int critDamage
    {
        get { return GetState(SrpgClassPropertyType.CritDamage); }
    }

    public int hitChance
    {
        get { return GetState(SrpgClassPropertyType.HitChanceBase); }
    }
    #endregion

    #region 方法
    private void Update()
    {
        m_classAnimator.isWalked = isWalked;
    }
    #region 初始化Unit(无SrpgClass)
    public void InitClass()
    {
        srpgClass.InitSrpgClass();

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_classAnimator = GetComponent<ClassAnimator>();
        m_HPSlider = GetComponentInChildren<Slider>();
        m_classAnimator.InitAnimator(m_srpgClass.classInfo);
        UpdatePosition(new Vector3Int((int)transform.position.x, (int)transform.position.y, 0));
        m_buffManager = new BuffManager(this);
        AddCurWeaponBuff();

        InitClassProperty(m_srpgClass);
        curHealth = maxHealth;


        m_StateMeching = GetComponent<AIStateMeching>();
        m_StateMeching.InitStateMeching(this);

        unitActionCommands = new Stack<Command>();

        m_HPSlider.maxValue = maxHealth;
        UpdateHPSlider();
    }
    #endregion

    #region 初始化Unit(有SrpgClass)
    public void InitClass(SrpgClass srpgClass)
    {
        this.m_srpgClass = srpgClass;
        srpgClass.InitSrpgClass();

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_classAnimator = GetComponent<ClassAnimator>();
        m_HPSlider = GetComponentInChildren<Slider>();
        m_classAnimator.InitAnimator(srpgClass.classInfo);
        UpdatePosition(new Vector3Int((int)transform.position.x,(int)transform.position.y,0));
        m_buffManager = new BuffManager(this);
        AddCurWeaponBuff();

        InitClassProperty(srpgClass);
        curHealth = maxHealth;


        unitActionCommands = new Stack<Command>();

        m_HPSlider.maxValue = maxHealth;
        UpdateHPSlider();
    }
    #endregion

    #region 将武器上的Buff添加到角色Unit身上
    public void AddCurWeaponBuff()
    {
        for(int i = 0; i < srpgClass.srpgWeapon.buffs.Length; i++)
        {
            var type = System.Type.GetType(srpgClass.srpgWeapon.buffs[i]);
            if(type == null)
            {
                Debug.LogError("No valid buff name");
            }
            var buff = type.Assembly.CreateInstance(srpgClass.srpgWeapon.buffs[i]);
            if (buff != null)
            {
                Debug.Log("Suc add buff");
                buffManager.AddBuff((Buff)buff);
            }

        }

    }
    #endregion

    #region 初始化属性
    public void InitClassProperty(SrpgClass srpgClass)
    {
        m_Property = new Dictionary<SrpgClassPropertyType, int>();
        m_Property.Add(SrpgClassPropertyType.Attack, CalculateProperty(srpgClass.classInfo, SrpgClassPropertyType.Attack));
        m_Property.Add(SrpgClassPropertyType.Defense, CalculateProperty(srpgClass.classInfo, SrpgClassPropertyType.Defense));
        m_Property.Add(SrpgClassPropertyType.MaxHealth, CalculateProperty(srpgClass.classInfo, SrpgClassPropertyType.MaxHealth));
        m_Property.Add(SrpgClassPropertyType.MagicAttack, CalculateProperty(srpgClass.classInfo, SrpgClassPropertyType.MagicAttack));
        m_Property.Add(SrpgClassPropertyType.MagicDefense, CalculateProperty(srpgClass.classInfo, SrpgClassPropertyType.MagicDefense));
        m_Property.Add(SrpgClassPropertyType.Avoid, CalculateProperty(srpgClass.classInfo, SrpgClassPropertyType.Avoid));
        m_Property.Add(SrpgClassPropertyType.HitChanceBase, CalculateProperty(srpgClass.classInfo, SrpgClassPropertyType.HitChanceBase));
        m_Property.Add(SrpgClassPropertyType.CritChance, CalculateProperty(srpgClass.classInfo, SrpgClassPropertyType.CritChance));
        m_Property.Add(SrpgClassPropertyType.CritDamage, CalculateProperty(srpgClass.classInfo, SrpgClassPropertyType.CritDamage));
    }
    #endregion

    #region 计算属性

    private int CalculateProperty(ClassInfo classInfo,SrpgClassPropertyType type)
    {
        if (type == SrpgClassPropertyType.Attack)
        {
            int attack = 0;
            attack += classInfo.attack + (classInfo.attackLevelBonus * Level);
            if(srpgClass.srpgWeapon != null)
            {
                attack += srpgClass.srpgWeapon.attack;
            }

            return attack;
        }else if(type == SrpgClassPropertyType.Defense)
        {
            int defense = 0;
            defense += classInfo.defense + (classInfo.defenseLevelBonus * Level);
            if(srpgClass.srpgArmor != null)
            {
                defense += srpgClass.srpgArmor.defense;
            }

            return defense;
        }else if(type == SrpgClassPropertyType.MaxHealth)
        {
            int maxHealth = 0;
            maxHealth += classInfo.maxHealth + (classInfo.maxHealthLevelBonus * Level);
            if(srpgClass.srpgArmor != null)
            {
                maxHealth += srpgClass.srpgArmor.health;
            }

            return maxHealth;
        }else if(type == SrpgClassPropertyType.MagicAttack)
        {
            int magicAttack = 0;
            magicAttack += classInfo.magicAttack + (classInfo.magicAttackBonus * Level);
            if(srpgClass.srpgWeapon != null)
            {
                magicAttack += srpgClass.srpgWeapon.magicAttack;
            }

            return magicAttack;
        }else if (type == SrpgClassPropertyType.MagicDefense)
        {
            int magicDefense = 0;
            magicDefense += classInfo.magicAttack + (classInfo.magicDefenseBonus * Level);
            if (srpgClass.srpgArmor != null)
            {
                magicDefense += srpgClass.srpgArmor.magicDefense;
            }
            return magicDefense;
        }
        else if(type == SrpgClassPropertyType.Avoid)
        {
            int avoid = 0;
            avoid += classInfo.avoid;
            if(srpgClass.srpgArmor != null)
            {
                avoid += srpgClass.srpgArmor.avoid;
            }

            return avoid;
        }else if (type == SrpgClassPropertyType.HitChanceBase)
        {
            int hitChance = 0;
            hitChance += classInfo.hitChanceBase;
            if(srpgClass.srpgWeapon != null)
            {
                hitChance += srpgClass.srpgWeapon.hitChance;
            }

            return hitChance;
        }else if(type == SrpgClassPropertyType.CritChance)
        {
            int critChance = 0;
            critChance += classInfo.critChance;

        }else if(type == SrpgClassPropertyType.CritDamage)
        {
            int critDamage = 0;
            critDamage += classInfo.critDamage;
        }

        return 0;
    }
    #endregion

    #region 根据AI的职业来初始化AI状态机
    public void SetDefaultAIBaseOnClassType()
    {
        if(this.m_classType == ClassType.footman)
        {
            m_StateMeching.SetCurrentState(Footman_Attack.Instance());
        }
    }
    #endregion

    #region 生命值改变
    //输入:准备改变的数值，正为减少，负为增加
    //输出:无
    public void ChangeHealth(int m_value)
    {
        curHealth -= m_value;

        if(curHealth <= 0)
        {
            //单位死亡，先隐藏角色，然后
            onDead();
        }
        UpdateHPSlider();
    }
    #endregion

    #region 被攻击方法(普通攻击)
    public DamageDetail OnDamaged(SrpgClassUnit attacker)
    {
        DamageDetail damageDetail = new DamageDetail();
        SrpgTile srpgTile = MapManager.instance.GetSrpgTilemapData(this.m_Position);
        if(attacker != null)
        {
            attacker.FaceTo(m_Position);
        }

        int delta_Level = attacker.Level - srpgClass.level;
        Mathf.Clamp(delta_Level, -5, 20);

        int damage =  (int)(attacker.m_Property[SrpgClassPropertyType.Attack] * ((100 - m_Property[SrpgClassPropertyType.Defense]) / 100.0) * (1 + (0.1 * delta_Level)) * Random.Range(0.85f, 1.15f));
        int avoid_Chance = srpgTile.avoidChange + m_Property[SrpgClassPropertyType.Avoid];
        int rd = Random.Range(0, 101);//闪避骰子
        int rd2 = Random.Range(0, 101);//暴击骰子

        damageDetail.damage = damage;
        damageDetail.attacker = attacker;
        damageDetail.defender = this;

        if (rd <= avoid_Chance)
        {
            //damage = (int)(damage * 0.5);
            damageDetail.isAvoid = true;
        }
        else
        {
            if(rd2 < attacker.critChance)
            {
                //damage *= attacker.critDamage / 100;
                damageDetail.isCritical = true;
            }
        }
        DamageDetail originDetail = new DamageDetail(damageDetail);
        #region 计算buff加成
        for (int i = 0; i < attacker.buffManager.buffs.Count; i++)
        {
            for(int j = 0; j < attacker.buffManager.buffs[i].buffEffects.Count; j++)
            {
                attacker.buffManager.buffs[i].buffEffects[j].OnAttack(attacker, this,damageDetail,originDetail);
            }
        }

        for (int i = 0; i < this.buffManager.buffs.Count; i++)
        {
            for (int j = 0; j < this.buffManager.buffs[i].buffEffects.Count; j++)
            {
                this.buffManager.buffs[i].buffEffects[j].OnDefend(this,attacker,damageDetail,originDetail);
            }
        }
        #endregion



        ChangeHealth(damageDetail.damage);
        Debug.Log($"Recive Damage :{damageDetail.damage.ToString()},curHealth :{curHealth}");

        return damageDetail;
        
    }
    #endregion

    #region 被攻击方法(技能伤害)
    public DamageDetail OnDamaged(SrpgClassUnit attacker,int m_Damage,bool isHasAttacker)
    {
        DamageDetail damageDetail = new DamageDetail();
        SrpgTile srpgTile = MapManager.instance.GetSrpgTilemapData(this.m_Position);
        if (attacker != null && isHasAttacker)
        {
            attacker.FaceTo(m_Position);
        }

        int delta_Level = attacker.Level - srpgClass.level;
        Mathf.Clamp(delta_Level, -5, 20);

        int damage = (int)(m_Damage * ((100 - m_Property[SrpgClassPropertyType.Defense]) / 100.0) * (1 + (0.1 * delta_Level)) * Random.Range(0.85f, 1.15f));
        int avoid_Chance = srpgTile.avoidChange + m_Property[SrpgClassPropertyType.Avoid];
        int rd = Random.Range(0, 101);//闪避骰子
        int rd2 = Random.Range(0, 101);//暴击骰子

        if (isHasAttacker) {
            //如果isHasAttacker 为 true，说明这次攻击是由Unit发动的
            damageDetail.attacker = attacker;
        }
        else
        {
            //如果isHasAttacker 为 false，说明这次攻击是由Buff或者其他发动的，在Buff判断中需要这条另做判断
            //例如反甲反弹的伤害当做无Attacker的伤害，对于无Attacker的伤害，反甲不会继续反弹，避免了俩个反甲无限反弹的局面。
            damageDetail.attacker = null;
        }

        damageDetail.damage = damage;
        damageDetail.defender = this;

        if (rd <= avoid_Chance)
        {
            //damage = (int)(damage * 0.5);
            damageDetail.isAvoid = true;
        }
        else
        {
            if (rd2 < attacker.critChance)
            {
                //damage *= attacker.critDamage / 100;
                damageDetail.isCritical = true;
            }
        }

        DamageDetail originDetail = new DamageDetail(damageDetail);

        #region 计算buff加成
        for (int i = 0; i < attacker.buffManager.buffs.Count; i++)
        {
            for (int j = 0; j < attacker.buffManager.buffs[i].buffEffects.Count; j++)
            {
                attacker.buffManager.buffs[i].buffEffects[j].OnAttack(attacker, this,damageDetail,originDetail);
            }
        }

        for (int i = 0; i < this.buffManager.buffs.Count; i++)
        {
            for (int j = 0; j < this.buffManager.buffs[i].buffEffects.Count; j++)
            {
                this.buffManager.buffs[i].buffEffects[j].OnDefend(this, attacker,damageDetail,originDetail);
            }
        }
        #endregion

        ChangeHealth(damageDetail.damage);
        Debug.Log($"Recive Damage :{damageDetail.damage.ToString()},curHealth :{curHealth}");

        return damageDetail;

    }
    #endregion

    #region 获得Unit的真实属性
    private int GetState(SrpgClassPropertyType type)
    {
        int stateValue = m_Property[type];

        for(int i = 0; i < m_buffManager.buffs.Count; i++)
        {
            for(int j = 0; j < m_buffManager.buffs[i].buffEffects.Count; j++)
            {
                m_buffManager.buffs[i].buffEffects[j].StateChange(ref stateValue, type);
            }
        }

        return stateValue;
    }
    #endregion

    #region 改变Unit的朝向
    private void FaceTo(Vector3Int targetPos)
    {
        int deltaX = targetPos.x - m_Position.x;

        m_classAnimator.moveX = Mathf.Clamp(deltaX, -1, 1);
        int deltaY = targetPos.y - m_Position.y;
        m_classAnimator.moveY = Mathf.Clamp(deltaY, -1, 1);
    }
    #endregion

    #region 移动方法
    public void MoveTo(List<CellData> path)
    {
        StartCoroutine(StartPathMove(path));
    }
    #endregion

    #region 协程移动方法
    public IEnumerator StartPathMove(List<CellData> moveList)
    {
        isMoveingPath = true;
        OnMoveStart();
        var oldPos = transform.position;
        int index = 0;
        while(index < moveList.Count)
        {
            if(isWalked == false)
            {
                yield return Move(moveList[index].m_Position);
                index++; 
            }

        }


        ScenceManager.instance.UpdateMapObjectPosition();
        OnMoveEnd();
        isMoveingPath = false;
    }
    #endregion

    #region 移动到目标格
    private IEnumerator Move(Vector3Int targetPosition)//行走动画，传入一个目标位置后就可以向目标走动
    {
        
   
        int deltaX = targetPosition.x - m_Position.x;

        m_classAnimator.moveX = Mathf.Clamp(deltaX,-1,1);
        int deltaY = targetPosition.y - m_Position.y;
        m_classAnimator.moveY = Mathf.Clamp(deltaY,-1,1);

        isWalked = true;

        var targetPositionVec3 = new Vector3(targetPosition.x, targetPosition.y, 0);
        while((targetPositionVec3 - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPositionVec3, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        isWalked = false;
        UpdatePosition(new Vector3Int((int)transform.position.x, (int)transform.position.y, 0));
    }
    #endregion

    #region 在移动开始触发
    public void OnMoveStart()
    {
        BattleManager.instance.isWalking = true;
    }
    #endregion

    #region 在移动结束的时候触发
    public void OnMoveEnd()
    {

        BattleManager.instance.GetComponent<BattleManager>().isWalking = false;
        var interactiveObject = ScenceManager.instance.TryGetInteractiveObject(m_Position);
        //Temp:如果是玩家的话，会询问是否Interac。
        if(interactiveObject != null)
        {
            //BUG:例如开启箱子之后，如果玩家一直撤回动作，开启的箱子和获得的物品不会撤回，应该用一个命令来开启箱子，在撤回的时候顺便UN_DO命令还原箱子和移除获得的物品
            Command interactiveObjectCommand = new InteractObjectCommand(interactiveObject, this);
            interactiveObjectCommand.Execute();
            unitActionCommands.Push(interactiveObjectCommand);
        }

    }
    #endregion

    #region 传送到目标位置
    public void TeleportTo(Vector3Int targetPos)
    {
        StopCoroutine("StartPathMove");
        transform.position = targetPos;
        UpdatePosition(targetPos);
        ScenceManager.instance.UpdateMapObjectPosition();
    }
    #endregion

    #region 添加Srpg道具给SrpgClass
    public void AddItem(SrpgUseableItem m_Item)
    {
        srpgClass.items.Add(m_Item);
    }
    #endregion

    #region 设置Unit为行动过状态
    public void SetUnitActived()
    {
        for (int i = buffManager.buffs.Count - 1; i >= 0; i--)
        {
            for (int j = 0; j < buffManager.buffs[i].buffEffects.Count; j++)
            {
                buffManager.buffs[i].buffEffects[j].OnTurnEnd(this);
                Debug.Log(buffManager.buffs[i].curDurationTimes);
            }
        }
        m_SpriteRenderer.color = Color.gray;
        IsActived = true;
    }
    #endregion

    #region 设置Unit为未行动过状态
    public void SetUnitActive()
    {
        m_SpriteRenderer.color = Color.white;
        isActived = false;
    }
    #endregion

    #region 移除Class的道具
    public void RemoveItem(SrpgUseableItem m_Item)
    {
        srpgClass.items.Remove(m_Item);
    }
    #endregion

    #region 角色Unit死亡时调用
    private void onDead()
    {
        gameObject.SetActive(false);
        ScenceManager.instance.UnRegisterSRPGClass(this);
    }
    #endregion

    #region 更新角色血条UI
    public void UpdateHPSlider()
    {
        m_HPSlider.value = curHealth;
    }
    #endregion

    #endregion
}


public class DamageDetail
{
    public SrpgClassUnit attacker;
    public SrpgClassUnit defender;
    public int damage;
    public bool isAvoid;
    public bool isCritical;

    public DamageDetail()
    {

    }

    public DamageDetail(DamageDetail originDamageDetail)
    {
        this.attacker = originDamageDetail.attacker;
        this.defender = originDamageDetail.defender;
        this.damage = originDamageDetail.damage;
        this.isAvoid = originDamageDetail.isAvoid;
        this.isCritical = originDamageDetail.isCritical;
    }
}