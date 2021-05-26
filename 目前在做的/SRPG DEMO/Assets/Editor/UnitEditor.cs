using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

public class StageEditor : EditorWindow
{
    GameObject curObject;
    GameObject preObject;
    SrpgClassUnit preUnit;
    SrpgClassUnit curUnit;

    SrpgClass curClass;
    ClassInfo curClassInfo;
    [MenuItem("UnitEditor/Open")]
    static void Init()
    {
        GetWindow<StageEditor>("SRPG Unit Editor");
    }

    private void OnGUI()
    {
        
        foreach(var gb in Selection.gameObjects)
        {
            var unit = gb.GetComponent<SrpgClassUnit>();
            if(unit != null)
            {
                curUnit = unit;

                if(curUnit.srpgClass.classInfo == null)
                {
                    DisplayNoClass();
                }
                

                if(curUnit.srpgClass.classInfo != null && curUnit.srpgClass != null)
                {
                    ShowSrpgClassEditor();
                }
                break;
            }
        }

    }


    public static void Save()
    {
        Debug.Log("Save");
        var scene = SceneManager.GetActiveScene();
        EditorSceneManager.SaveScene(scene);
    }


    public void DisplayNoClass()
    {
        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("当前Unit缺少一个Class");
        GUI.skin.label.fontSize = 10;
        GUILayout.Space(10);

        GUILayout.Space(10);
        GUI.skin.label.fontSize = 20;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("请放入一个ClassInfo");
        curClassInfo = (ClassInfo)EditorGUILayout.ObjectField("Class Info", curClassInfo, typeof(ClassInfo));

        if (GUILayout.Button("使用这个ClassInfo"))
        {
            if(curClassInfo != null)
            {
                curUnit.srpgClass.classInfo = curClassInfo;
                Save();
            }

            
        }

        GUILayout.EndVertical();


    }

    private void ShowSrpgClassEditor()
    {
        GUILayout.BeginVertical();
        GUI.skin.label.fontSize = 20;
        GUILayout.Label("角色属性基础");
        curUnit.srpgClass.classInfo = (ClassInfo)EditorGUILayout.ObjectField(curUnit.srpgClass.classInfo,typeof(ClassInfo));
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUI.skin.label.fontSize = 16;
        GUILayout.Label("角色名称");
        curUnit.srpgClass.srpgClassName = EditorGUILayout.TextField(curUnit.srpgClass.srpgClassName);
        GUILayout.Space(10);
        GUILayout.Label("角色等级");
        curUnit.srpgClass.level = EditorGUILayout.IntField(curUnit.srpgClass.level);
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.Label("角色武器");
        curUnit.srpgClass.weaponName = EditorGUILayout.TextField(curUnit.srpgClass.weaponName);
        GUILayout.Space(5);
        GUILayout.Label("角色护甲");
        curUnit.srpgClass.armorName = EditorGUILayout.TextField(curUnit.srpgClass.armorName);

        GUILayout.Space(20);
        GUILayout.BeginVertical();
        int itemCount = EditorGUILayout.IntField("ItemCount", 0);
        GUILayout.EndHorizontal();

        //for (int i = 0; i < itemCount; i++)

        GUILayout.EndVertical();
        GUILayout.Space(15);
        GUILayout.BeginVertical();
        GUILayout.Label("基础最大生命值 : " + $"{curUnit.srpgClass.classInfo.maxHealth + (curUnit.Level * curUnit.srpgClass.classInfo.maxHealthLevelBonus)}");
        GUILayout.BeginHorizontal();
        GUILayout.Label("基础攻击 : " + $"{curUnit.srpgClass.classInfo.attack + (curUnit.Level * curUnit.srpgClass.classInfo.attackLevelBonus)}");
        GUILayout.Label("基础防御 : " + $"{curUnit.srpgClass.classInfo.defense + (curUnit.Level * curUnit.srpgClass.classInfo.defenseLevelBonus)}" + "%");
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Label("基础法术攻击 : " + $"{curUnit.srpgClass.classInfo.magicAttack + (curUnit.Level * curUnit.srpgClass.classInfo.magicAttackBonus)}");
        GUILayout.Label("基础法术防御 : " + $"{curUnit.srpgClass.classInfo.magicDefense + (curUnit.Level * curUnit.srpgClass.classInfo.magicDefenseBonus)}" + "%");
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Label("基础闪避 : " + $"{curUnit.srpgClass.classInfo.avoid}" + "%");
        GUILayout.Label("基础暴击 : " + $"{curUnit.srpgClass.classInfo.critChance}" + "%");
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.Label("基础暴击伤害 : " + $"{curUnit.srpgClass.classInfo.critDamage}" + "%");
        GUILayout.EndVertical();


        if (GUILayout.Button("保存"))
        {
            EditorUtility.SetDirty(curUnit);
            Save();
        }


    }

}


public class ClassInfos : EditorWindow
{
    public SrpgClassUnit curUnit;
    ClassInfo curInfo;

    private void OnGUI()
    {
        ShowSrpgClassInfo();
    }

    private void ShowSrpgClassInfo()
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label("基础攻击" + $"{curUnit.srpgClass.classInfo.attack}");
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

}