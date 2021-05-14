using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStat
{
    InBattleScence,
}
public class GameManager : MonoBehaviour
{
    public GameStat gameStat;
    public static GameManager instance;
    public BattleManager battleManager;
    public bool isGameStop = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        battleManager = GetComponent<BattleManager>();
        ItemDatabase.Init();
    }

    private void Update()
    {
        if(gameStat == GameStat.InBattleScence)
        {
            battleManager.HandleUpdate();
            MapManager.instance.HandleUpdate();
        }
    }
}
