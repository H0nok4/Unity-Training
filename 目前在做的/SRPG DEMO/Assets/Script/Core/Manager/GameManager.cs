using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    InStartScence,InBattleScence,PlayScenario
}
public class GameManager : MonoBehaviour
{
    public GameState gameState;
    public GameState preGameState;
    public static GameManager instance;
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
        ItemDatabase.Init();

    }

    private void Update()
    {
        if(gameState == GameState.InStartScence)
        {
            StartUIManager.instance.HandleUpdate();
        }else if(gameState == GameState.InBattleScence)
        {
            BattleManager.instance.HandleUpdate();
            MapManager.instance.HandleUpdate();
        }else if(gameState == GameState.PlayScenario)
        {
            GameDirecter.instance.HandleUpdate();
        }
    }

    public void SwitchGameState(GameState curState)
    {
        preGameState = gameState;
        gameState = curState;
    }

    public void ReverseGameState()
    {
        GameState tempState = gameState;
        gameState = preGameState;
        preGameState = tempState;
    }
}
