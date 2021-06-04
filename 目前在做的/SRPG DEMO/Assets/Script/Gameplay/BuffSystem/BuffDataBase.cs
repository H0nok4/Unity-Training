using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDataBase
{
    public void Init()
    {
        foreach (var KeyValuePair in buff_Dictionary)
        {
            var buff_Name = KeyValuePair.Key;
            var buff = KeyValuePair.Value;

            buff.buffName = buff_Name;
        }
    }

    public static Dictionary<string, Buff> buff_Dictionary { get; set; } = new Dictionary<string, Buff>()
    {
        {"Strong",new Strong() },
        {"Burn",new Burn() },
        {"Stun",new Stun() }
    };
}
