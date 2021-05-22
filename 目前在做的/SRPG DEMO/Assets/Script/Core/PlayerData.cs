using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public List<SrpgClass> playerClasses = new List<SrpgClass>();
    public List<SrpgClass> playerBattleClasses = new List<SrpgClass>();

    #region 单例
    public static PlayerData instance;

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
        DontDestroyOnLoad(gameObject);
    }
    #endregion
}
