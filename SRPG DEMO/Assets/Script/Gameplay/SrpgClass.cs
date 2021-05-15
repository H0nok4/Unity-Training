using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public class SrpgClass :  MapObject
{
    #region 成员变量
    [SerializeField] ClassType m_classType;
    [SerializeField] ClassCamp m_classCamp;
    [SerializeField] int m_Level;
    [SerializeField] int curHealth;
    [SerializeField] SrpgClassProperty srpgClassProperty;
    [SerializeField] int m_moveCost;
    [SerializeField] SrpgWeapon m_Weapon;
    [SerializeField] SrpgArmor m_Armor;
    [SerializeField] ClassAnimator m_classAnimator;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] GameObject highLightSprite;
    [SerializeField] bool isActived;
    [SerializeField] AIStateMeching m_StateMeching;
    [SerializeField] List<SrpgUseableItem> m_Items = new List<SrpgUseableItem>();
    [SerializeField] bool Inited = false;
    [SerializeField] Coroutine pathMoving;
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
        get { return m_moveCost; }
    }

    public ClassCamp classCamp
    {
        get { return m_classCamp; }
    }

    private Dictionary<SrpgClassPropertyType, int> m_Property;


    public Dictionary<SrpgClassPropertyType, int> classProperty
    {
        get { return m_Property; }
    }

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
    #endregion

    #region 方法
    private void Update()
    {
        m_classAnimator.isWalked = isWalked;
    }

    public void InitClass()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_classAnimator = GetComponent<ClassAnimator>();
        UpdatePosition(new Vector3Int((int)transform.position.x,(int)transform.position.y,0));
        m_Weapon = ItemDatabase.weapon_Dictionary["IronSword"];
        m_Armor = ItemDatabase.armor_Dictionary["Leather"];
        InitClassProperty();
        curHealth = m_Property[SrpgClassPropertyType.MaxHealth];
        m_StateMeching = GetComponent<AIStateMeching>();
        m_StateMeching.InitStateMeching(this);
        SrpgUseableItem smallPostion = new SmallPotion();
        m_Items.Add(smallPostion);
    }

    public void StartBattleInit()
    {

    }

    public void InitClassProperty()
    {
        m_Property = new Dictionary<SrpgClassPropertyType, int>();
        m_Property.Add(SrpgClassPropertyType.Attack, CalculateProperty(srpgClassProperty, SrpgClassPropertyType.Attack));
        m_Property.Add(SrpgClassPropertyType.Defense, CalculateProperty(srpgClassProperty, SrpgClassPropertyType.Defense));
        m_Property.Add(SrpgClassPropertyType.MaxHealth, CalculateProperty(srpgClassProperty, SrpgClassPropertyType.MaxHealth));
        m_Property.Add(SrpgClassPropertyType.MagicAttack, CalculateProperty(srpgClassProperty, SrpgClassPropertyType.MagicAttack));
        m_Property.Add(SrpgClassPropertyType.MagicDefense, CalculateProperty(srpgClassProperty, SrpgClassPropertyType.MagicDefense));
        m_Property.Add(SrpgClassPropertyType.Avoid, CalculateProperty(srpgClassProperty, SrpgClassPropertyType.Avoid));
        m_Property.Add(SrpgClassPropertyType.HitChanceBase, CalculateProperty(srpgClassProperty, SrpgClassPropertyType.HitChanceBase));
        m_Property.Add(SrpgClassPropertyType.CritChance, CalculateProperty(srpgClassProperty, SrpgClassPropertyType.CritChance));
        m_Property.Add(SrpgClassPropertyType.CritDamage, CalculateProperty(srpgClassProperty, SrpgClassPropertyType.CritDamage));
    }

    public int CalculateProperty(SrpgClassProperty baseProperty,SrpgClassPropertyType type)
    {
        if (type == SrpgClassPropertyType.Attack)
        {
            int attack = 0;
            attack += baseProperty.attack;
            if(m_Weapon != null)
            {
                attack += m_Weapon.attack;
            }

            return attack;
        }else if(type == SrpgClassPropertyType.Defense)
        {
            int defense = 0;
            defense += baseProperty.defense;
            if(m_Armor != null)
            {
                defense += m_Armor.defense;
            }

            return defense;
        }else if(type == SrpgClassPropertyType.MaxHealth)
        {
            int maxHealth = 0;
            maxHealth += baseProperty.maxHealth;
            if(m_Armor != null)
            {
                maxHealth += m_Armor.health;
            }

            return maxHealth;
        }else if(type == SrpgClassPropertyType.MagicAttack)
        {
            int magicAttack = 0;
            magicAttack += baseProperty.magicAttack;
            if(m_Weapon != null)
            {
                magicAttack += m_Weapon.magicAttack;
            }

            return magicAttack;
        }else if(type == SrpgClassPropertyType.Avoid)
        {
            int avoid = 0;
            avoid += baseProperty.avoid;
            if(m_Armor != null)
            {
                avoid += m_Armor.avoid;
            }

            return avoid;
        }else if (type == SrpgClassPropertyType.HitChanceBase)
        {
            int hitChance = 0;
            hitChance += baseProperty.hitChanceBase;
            if(m_Weapon != null)
            {
                hitChance += m_Weapon.hitChance;
            }

            return hitChance;
        }else if(type == SrpgClassPropertyType.CritChance)
        {
            int critChance = 0;
            critChance += baseProperty.critChance;

        }else if(type == SrpgClassPropertyType.CritDamage)
        {
            int critDamage = 0;
            critDamage += baseProperty.critDamage;
        }

        return 0;
    }

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
    }
    #endregion

    #region 被攻击方法
    public DamageDetail OnDamaged(SrpgClass attacker, SrpgTile srpgTile)
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
            if(rd2 < attacker.classProperty[SrpgClassPropertyType.CritChance])
            {
                damage *= attacker.classProperty[SrpgClassPropertyType.CritDamage] / 100;
            }
        }



        DamageDetail damageDetail = new DamageDetail()
        {
            damage = damage,
            isAvoid = rd <= avoid_Chance,
            isCritical = rd2 <= attacker.classProperty[SrpgClassPropertyType.CritChance]
        };

        ChangeHealth(damage);

        //TO DO:武器序列化的时候将会带一串string，反序列化后用这一串string在专门设置的武器特效字典里找到对应的匿名函数。
        /*if (weapon.onDamageTarget != null)
            weapon.onDamageTarget.Invoke(this);
        if (weapon.onDamageSelf != null)
            weapon.onDamageSelf.Invoke(attacker);*/

        Debug.Log($"Recive Damage :{damage.ToString()},curHealth :{curHealth}");

        return damageDetail;
        
    }
    #endregion
    public void FaceTo(Vector3Int targetPos)
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


        GameObject.Find("GameManager").GetComponent<ScenceManager>().UpdateMapObjectPosition();
        OnMoveEnd();
        isMoveingPath = false;
    }

    
    public IEnumerator Move(Vector3Int targetPosition)//行走动画，传入一个目标位置后就可以向目标走动
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
        GameObject.Find("GameManager").GetComponent<BattleManager>().isWalking = true;
    }
    public void OnMoveEnd()
    {

        GameObject.Find("GameManager").GetComponent<BattleManager>().isWalking = false;
        var interactiveObject = ScenceManager.instance.TryGetInteractiveObject(m_Position);
        //Temp:如果是玩家的话，会询问是否Interac。
        if(interactiveObject != null)
        {
            interactiveObject.Interact(this);
        }

    }

    public void TeleportTo(Vector3Int targetPos)
    {
        transform.position = targetPos;
        StopCoroutine("StartPathMove");
        UpdatePosition(targetPos);
    }

    public void onDead()
    {
        gameObject.SetActive(false);
        ScenceManager.instance.UnRegisterSRPGClass(this);
    }

    #endregion
}


public class DamageDetail
{
    public int damage;
    public bool isAvoid;
    public bool isCritical;
}