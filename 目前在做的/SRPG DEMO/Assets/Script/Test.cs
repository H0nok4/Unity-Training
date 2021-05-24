using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;
using System.IO;

public class Test : MonoBehaviour
{
    
    public void Start()
    {
        Buff burn = new Burn();
        Buff stun = new Stun();
        ScenceManager.instance.playerClasses[0].buffManager.AddBuff(burn);
        ScenceManager.instance.playerClasses[0].buffManager.AddBuff(stun);
        //ScenceManager.instance.playerClasses[0].buffManager.AddBuff(new Strong());
    }

    private void Update()
    {

    }
}