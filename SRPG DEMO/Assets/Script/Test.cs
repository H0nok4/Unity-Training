using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;
using System.IO;

public class Test : MonoBehaviour
{
    public SpriteRenderer sprite1;
    public SpriteRenderer sprite2;
    public void Start()
    {
        SrpgUseableItem potion = new SmallPotion();
        Sprite tempSprite = Resources.Load<Sprite>("Sprite/ItemSprite/Bandage");
        //sprite1.sprite = potion.sprite;
        //sprite2.sprite = tempSprite;
    }

    private void Update()
    {

    }
}