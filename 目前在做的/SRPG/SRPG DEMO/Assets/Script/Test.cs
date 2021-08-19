using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;
using System.IO;
using System;

public class Test : MonoBehaviour
{

    Skill thisSkill;
    public void Start()
    {
        /*
        Buff burn = new Burn();
        Buff stun = new Stun();
        ScenceManager.instance.playerClasses[0].buffManager.AddBuff(burn);
        ScenceManager.instance.playerClasses[0].buffManager.AddBuff(stun);
        //ScenceManager.instance.playerClasses[0].buffManager.AddBuff(new Strong());
        */
        string jsonPath = "Assets/SkillData/Skill.json";
        var jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        Dictionary<string, Skill> skillDictionary = new Dictionary<string, Skill>();
        Skill test = new Skill()
        {
            SkillName = "Burning",
            SkillDes = "Make target burning,cause 50 damage and 50% make target [Burn]",
            SkillRenge = new int[3][]
            {
                new int[3]{0,1,0},
                new int[3]{1,0,1},
                new int[3]{0,1,0}
            },
            SkillTargetCamp = "Enemy",
        
            OnSkillActive = new SkillActive()
            {
                Target = new TargetDetermine[]
                {
                    new TargetDetermine
                    {
                        SkillTarget = "Target",
                        Damage = new DamageBehavior()
                        {
                            DamageValue = "50"
                        }
                    }
                }
            },

            OnAfterSkillActive = new AfterSkillActive()
            {
                If = new IfDetermine[]
                {
                    new IfDetermine()
                    {
                        IfParameter = new string[]
                        {
                            "Chance",
                            "50"
                        },
                        TargetDetermine = new TargetDetermine()
                        {
                            SkillTarget = "Target",
                            AddBuff = new AddBuffBehavior()
                            {
                                BuffName = "Burn"
                            }
                        }
                        
                    }
                }
            }
        
        };
        skillDictionary.Add(test.SkillName,test);
        //File.WriteAllText(jsonPath, JsonConvert.SerializeObject(skillDictionary,Formatting.Indented,jsonSetting));
        var skillDetail = JsonConvert.DeserializeObject<Dictionary<string,Skill>>(File.ReadAllText(jsonPath));
        thisSkill = skillDetail["Burning"];
        Debug.Log(this.thisSkill.SkillName);
        

    }

    public void TestFunc(SrpgClassUnit selfUnit,SrpgClassUnit targetUnit)
    {
        SkillSystem.SkillHandler(thisSkill, selfUnit, targetUnit);
        Debug.Log("Skill System Test");
    }

    private void Update()
    {

    }
}