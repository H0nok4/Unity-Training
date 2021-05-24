using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public Vector3Int pos;
    public abstract void Interact(SrpgClassUnit srpgClass);

    public virtual void Un_Do(SrpgClassUnit unit)
    {

    }
}
