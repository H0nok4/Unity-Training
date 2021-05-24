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
    [SerializeField] string m_UnitName;
    [SerializeField] int m_Level;
    [SerializeField] int curHealth;
    [SerializeField] int m_MovePoint;
    [SerializeField] SrpgWeapon m_Weapon;
    [SerializeField] SrpgArmor m_Armor;
    [SerializeField] ClassAnimator m_classAnimator;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] GameObject highLightSprite;
    [SerializeField] bool isActived;
    [SerializeField] AIStateMeching m_StateMeching;
    [SerializeField] List<SrpgUseableItem> m_Items;
    [SerializeField] BuffManager m_buffManager;
    public Stack<Command> unitActionCommands;
    private Slider m_HPSlider;
    #endregion

    #region 属性

    public int Level
    {
        get { return m_Level; }
        set { m_Level = value; }
    }
    public int CurHealth
    {
        get { return curHealth; }
        set { curHealth = value; }
    }
    public SrpgWeapon Weapon
    {
        get { return m_Weapon; }
    }

    public SrpgArmor armor
    {
        get { return m_Armor; }
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
        get { return m_MovePoint; }
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
        get { return m_Items; }
    }

    public SrpgClass srpgclass
    {
        get { return m_srpgClass; }
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
    public void InitClass()
    {
        srpgclass.InitSrpgClass();

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_classAnimator = GetComponent<ClassAnimator>();
        m_classAnimator.InitAnimator(m_srpgClass.classInfo);
        UpdatePosition(new Vector3Int((int)transform.position.x, (int)transform.position.y, 0));
        m_buffManager = new BuffManager(this);

        m_Weapon = srpgclass.srpgWeapon;
        m_Armor = srpgclass.srpgArmor;
        m_MovePoint = m_srpgClass.classInfo.classMovePoint;
        m_Items = m_srpgClass.items;

        InitClassProperty(m_srpgClass);
        curHealth = m_Property[SrpgClassPropertyType.MaxHealth];
        m_StateMeching = GetComponent<AIStateMeching>();
        m_StateMeching.InitStateMeching(this);

        unitActionCommands = new Stack<Command>();
        m_HPSlider = GetComponentInChildren<Slider>();
        m_HPSlider.maxValue = maxHealth;
    }
    public void InitClass(SrpgClass srpgClass)
    {
        this.m_srpgClass = srpgClass;
        srpgClass.InitSrpgClass();

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_classAnimator = GetComponent<ClassAnimator>();
        m_classAnimator.InitAnimator(srpgClass.classInfo);
        UpdatePosition(new Vector3Int((int)transform.position.x,(int)transform.position.y,0));
        m_buffManager = new BuffManager(this);

        m_UnitName = srpgClass.srpgClassName;
        m_MovePoint = srpgClass.classInfo.classMovePoint;
        m_Level = srpgClass.level;
        m_Weapon = srpgClass.srpgWeapon;
        m_Armor = srpgClass.srpgArmor;

        InitClassProperty(srpgClass);
        curHealth = m_Property[SrpgClassPropertyType.MaxHealth];

        m_Items = srpgClass.items;

        unitActionCommands = new Stack<Command>();
        m_HPSlider = GetComponentInChildren<Slider>();
        m_HPSlider.maxValue = maxHealth;
    }

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
            attack += classInfo.attack;
            if(m_Weapon != null)
            {
                attack += m_Weapon.attack;
            }

            return attack;
        }else if(type == SrpgClassPropertyType.Defense)
        {
            int defense = 0;
            defense += classInfo.defense;
            if(m_Armor != null)
            {
                defense += m_Armor.defense;
            }

            return defense;
        }else if(type == SrpgClassPropertyType.MaxHealth)
        {
            int maxHealth = 0;
            maxHealth += classInfo.maxHealth;
            if(m_Armor != null)
            {
                maxHealth += m_Armor.health;
            }

            return maxHealth;
        }else if(type == SrpgClassPropertyType.MagicAttack)
        {
            int magicAttack = 0;
            magicAttack += classInfo.magicAttack;
            if(m_Weapon != null)
            {
                magicAttack += m_Weapon.magicAttack;
            }

            return magicAttack;
        }else if(type == SrpgClassPropertyType.Avoid)
        {
            int avoid = 0;
            avoid += classInfo.avoid;
            if(m_Armor != null)
            {
                avoid += m_Armor.avoid;
            }

            return avoid;
        }else if (type == SrpgClassPropertyType.HitChanceBase)
        {
            int hitChance = 0;
            hitChance += classInfo.hitChanceBase;
            if(m_Weapon != null)
            {
                hitChance += m_Weapon.hitChance;
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

    public override void OnSpawn()
    {

    }

    public override void OnDispawn()
    {

    }

    public void SetDefaultAIBaseOnClassType()
    {
        if(this.m_classType == ClassType.footman)
        {
            m_StateMeching.SetCurrentState(Footman_Attack.Instance());
        }
    }

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

    #region 被攻击方法
    public DamageDetail OnDamaged(SrpgClassUnit attacker, SrpgTile srpgTile)
    {
        attacker.FaceTo(m_Position);
        SrpgWeapon weapon = attacker.Weapon;
        int delta_Level = attacker.Level - m_Level;
        Mathf.Clamp(delta_Level, -5, 20);
        int damage =  (int)(attacker.m_Property[SrpgClassPropertyType.Attack] * ((100 - m_Property[SrpgClassPropertyType.Defense]) / 100.0) * (1 + (0.1 * delta_Level)) * Random.Range(0.85f, 1.15f));
        int avoid_Chance = srpgTile.avoidChange + m_Property[SrpgClassPropertyType.Avoid];
        int rd = Random.Range(0, 101);
        int rd2 = Random.Range(0, 101);
        if (rd <= avoid_Chance)
        {
            damage = (int)(damage * 0.5);
        }
        else
        {
            if(rd2 < attacker.critChance)
            {
                damage *= attacker.critDamage / 100;
            }
        }

        #region 计算buff加成
        for (int i = 0; i < attacker.buffManager.buffs.Count; i++)
        {
            for(int j = 0; j < attacker.buffManager.buffs[i].buffEffects.Count; j++)
            {
                attacker.buffManager.buffs[i].buffEffects[j].OnAttack(attacker, this,ref damage);
            }
        }
        for (int i = 0; i < attacker.buffManager.buffs.Count; i++)
        {
            for (int j = 0; j < attacker.buffManager.buffs[i].buffEffects.Count; j++)
            {
                this.buffManager.buffs[i].buffEffects[j].OnDefend(this,attacker,ref damage);
            }
        }
        #endregion

        DamageDetail damageDetail = new DamageDetail()
        {
            damage = damage,
            isAvoid = rd <= avoid_Chance,
            isCritical = rd2 <= attacker.critChance
        };

        ChangeHealth(damage);
        Debug.Log($"Recive Damage :{damage.ToString()},curHealth :{curHealth}");

        return damageDetail;
        
    }
    #endregion
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

    private void FaceTo(Vector3Int targetPos)
    {
        int deltaX = targetPos.x - m_Position.x;

        m_classAnimator.moveX = Mathf.Clamp(deltaX, -1, 1);
        int deltaY = targetPos.y - m_Position.y;
        m_classAnimator.moveY = Mathf.Clamp(deltaY, -1, 1);
    }

    public void MoveTo(List<CellData> path)
    {
        StartCoroutine(StartPathMove(path));
    }

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
    public void OnMoveStart()
    {
        BattleManager.instance.isWalking = true;
    }
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

    public void TeleportTo(Vector3Int targetPos)
    {
        transform.position = targetPos;
        StopCoroutine("StartPathMove");
        UpdatePosition(targetPos);
    }

    public void AddItem(SrpgUseableItem m_Item)
    {
        m_Items.Add(m_Item);
    }

    public void RemoveItem(SrpgUseableItem m_Item)
    {
        m_Items.Remove(m_Item);
    }

    private void onDead()
    {
        gameObject.SetActive(false);
        ScenceManager.instance.UnRegisterSRPGClass(this);
    }

    private void UpdateHPSlider()
    {
        m_HPSlider.value = curHealth;
    }

    #endregion
}


public class DamageDetail
{
    public int damage;
    public bool isAvoid;
    public bool isCritical;
}