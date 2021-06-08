using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractiveObject
{
    
    [SerializeField] string itemName;

    public override void Interact(SrpgClassUnit srpgClass)
    {
        if (ItemDatabase.Items_Dictionary.ContainsKey(itemName))
        {
            srpgClass.AddItem(ItemDatabase.Items_Dictionary[itemName]);
        }
        else
        {
            Debug.Log($"Item Name invalid →{itemName}");
        }
        ScenceManager.instance.UnRegisterInteractiveObject(pos);
        this.gameObject.SetActive(false);
    }

    public override void Un_Do(SrpgClassUnit unit)
    {
        this.gameObject.SetActive(true);
        unit.RemoveItem(ItemDatabase.Items_Dictionary[itemName]);
    }
}
