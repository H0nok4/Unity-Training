using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapObjectType
{
    Class,
    MapCursor,
    MapObstacle,
    MouseCursor,
    Teleporter
}
public abstract class MapObject : MonoBehaviour
{
    //TO DO：地图对象所需要继承的类  2021/4/13目前为空
    public SpriteRenderer m_SpriteRenderer;
    public MapObjectType m_MapObjectType;
    public Vector3Int m_Position;

    public void UpdatePosition(Vector3Int newPosition)
    {
        m_Position = newPosition;
    }

    public virtual void OnSpawn()
    {

    }

    public virtual void OnDispawn()
    {

    }

    

}
