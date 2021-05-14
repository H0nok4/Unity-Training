using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ScenceManager : MonoBehaviour
{
    #region 成员变量
    public List<SrpgClass> playerClasses;
    public List<SrpgClass> enemyClasses;
    public List<SrpgClass> allyClasses;
    public List<SrpgClass> neutralClasses;
    public Dictionary<Vector3Int,GameObject> mapObjectGameObjects;//地图Objects
    public Dictionary<Vector3Int, GameObject> interactiveObjectGameObjects;//可互动的Objects
    public UnityAction onObjectUnregister;
    public static ScenceManager instance;
    #endregion

    #region 方法
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != null)
            {
                Destroy(gameObject);
            }
        }
    }//单例
    public void InitAllMapObject()
    {
        mapObjectGameObjects = new Dictionary<Vector3Int, GameObject>();
        var gameobjects = GameObject.FindGameObjectsWithTag("MapObject");
        foreach(var mapObjectGameObject in gameobjects)
        {
            mapObjectGameObjects.Add(new Vector3Int((int)mapObjectGameObject.transform.position.x, (int)mapObjectGameObject.transform.position.y, 0), mapObjectGameObject);
        }

        interactiveObjectGameObjects = new Dictionary<Vector3Int, GameObject>();
        var interactiveGameObjects = GameObject.FindGameObjectsWithTag("InteractiveObject");
        foreach(var interactiveObject in interactiveGameObjects)
        {
            interactiveObjectGameObjects.Add(new Vector3Int((int)interactiveObject.transform.position.x, (int)interactiveObject.transform.position.y, (int)interactiveObject.transform.position.z), interactiveObject);
        }

    }
    public void UpdateMapObjectPosition(MapObject mapObject)
    {
        Vector3Int removePos;
        foreach(var kvp in mapObjectGameObjects)
        {
            if(kvp.Value == mapObject)
            {
                removePos = kvp.Key;
                break;
            }
        }

        mapObjectGameObjects.Add(mapObject.m_Position,mapObject.gameObject);
    }
    public void UpdateMapObjectPosition()
    {
        List<Vector3Int> needUpdatePosition = new List<Vector3Int>();
        List<GameObject> needAddGameobject = new List<GameObject>();
        foreach (var kvp in mapObjectGameObjects)
        {
            if(kvp.Value.GetComponent<MapObject>().m_Position != kvp.Key)
            {
                needAddGameobject.Add(kvp.Value);
                needUpdatePosition.Add(kvp.Key);

            }
        }

        foreach(var gameObject in needAddGameobject)
        {
            mapObjectGameObjects.Add(new Vector3Int(gameObject.GetComponent<MapObject>().m_Position.x, gameObject.GetComponent<MapObject>().m_Position.y, 0), gameObject);
        }
        foreach(var pos in needUpdatePosition)
        {
            mapObjectGameObjects.Remove(pos);
        }

    }

    public SrpgClass GetClassInVector3Int(Vector3Int classPos)
    {
        if (mapObjectGameObjects.ContainsKey(classPos))
            return mapObjectGameObjects[classPos].GetComponent<SrpgClass>();
        else
            return null;
    }

    public void InitClass()
    {
        foreach(var Class in playerClasses)
        {
            Class.InitClass();
        }
        foreach (var Class in enemyClasses)
        {
            Class.InitClass();
            Class.SetDefaultAIBaseOnClassType();
        }
        foreach (var Class in allyClasses)
        {
            Class.InitClass();
            Class.SetDefaultAIBaseOnClassType();
        }
        foreach (var Class in neutralClasses)
        {
            Class.InitClass();
            Class.SetDefaultAIBaseOnClassType();
        }
    }

    public void DestroyMapObject(MapObject destroyMapObject)
    {
        mapObjectGameObjects.Remove(destroyMapObject.m_Position);
        destroyMapObject.OnDispawn();
    }

    public void UnRegisterSRPGClass(SrpgClass srpgClass)
    {
        if (playerClasses.Contains(srpgClass))
        {
            playerClasses.Remove(srpgClass);
        }
        if (enemyClasses.Contains(srpgClass))
        {
            enemyClasses.Remove(srpgClass);
        }
        if (allyClasses.Contains(srpgClass))
        {
            allyClasses.Remove(srpgClass);
        }
        if (neutralClasses.Contains(srpgClass))
        {
            neutralClasses.Remove(srpgClass);
        }
        //移除之后，应该删除该角色的Object，并且更新该场景单位的坐标，最后
        UpdateMapObjectPosition();

    }

    public InteractiveObject TryGetInteractiveObject(Vector3Int pos)
    {
        if (interactiveObjectGameObjects.ContainsKey(pos))
        {
            return interactiveObjectGameObjects[pos].GetComponent<InteractiveObject>();
        }

        return null;
    }
    public bool IsHaveTeleporter()
    {
        foreach(var obj in interactiveObjectGameObjects)
        {
            if (obj.Value.GetComponent<Teleporter>())
            {
                return true;
            }
        }
        return false;
    }

    public Teleporter TryGetNearestTeleporter(Vector3Int pos)
    {
        Teleporter tempTeleporter = null;
        int result = int.MaxValue;
        foreach(var intObject in interactiveObjectGameObjects)
        {
            var teleporter = intObject.Value.GetComponent<Teleporter>();
            if(teleporter != null)
            {
                int dis = Math.Abs(teleporter.Pos.x - pos.x) + Math.Abs(teleporter.pos.y - pos.y);
                if(dis <= result)
                {
                    result = dis;
                    tempTeleporter = teleporter;
                }

            }
        }
        if(tempTeleporter != null)
        {
            return tempTeleporter;
        }
        return null;
    }
    #endregion
}
