using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public Vector3Int pos;

    private void Awake()
    {
        pos = new Vector3Int((int)this.transform.position.x, (int)transform.position.y, 0);
    }
    public abstract void Interact(SrpgClassUnit srpgClass);

    public virtual void Un_Do(SrpgClassUnit unit)
    {

    }
}
