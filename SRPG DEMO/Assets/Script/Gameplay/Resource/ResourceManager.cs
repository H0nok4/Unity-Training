using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourceManager : MonoBehaviour
{
    public static Dictionary<string, Sprite> itemSpriteDic = new Dictionary<string, Sprite>();
    // Start is called before the first frame update
    public void Awake()
    {
        AssetBundle itemSprite = AssetBundle.LoadFromFile("Assets/AssetBundles/sprite/itemsprites");
        Sprite[] itemSprites = itemSprite.LoadAllAssets<Sprite>();

        foreach (var sprite in itemSprites)
        {
            itemSpriteDic.Add(sprite.name, sprite);
        }

    }



}
