using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : InteractiveObject
{
    [SerializeField] Vector3Int m_targetPos;

    public Vector3Int Pos
    {
        get { return new Vector3Int((int)transform.position.x,(int)transform.position.y,0); }
    }

    public Vector3Int targetPos
    {
        get { return m_targetPos; }
    }
    public override void Interact(SrpgClassUnit srpgClass)
    {
        if(ScenceManager.instance.mapObjectGameObjects.ContainsKey(targetPos))
        {
            return;
        }
        srpgClass.TeleportTo(targetPos);

    }
}
