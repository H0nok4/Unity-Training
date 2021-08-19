using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScenarioPlayStatus
{
    start,waitTextWriteDone,waitPlayerSelectOption,wait,end,waitForNewScenario
}

public class GameDirecter : MonoBehaviour
{
    public TextScript textScript = new TextScript();
    [SerializeField] Dictionary<string, int> ScenarioHeap = new Dictionary<string, int>();

    public ScenarioPlayStatus state;
    public string StartScenario;
    public string EndScenario;

    public List<ScenarioCommand> curScenarioCommands;
    public int curCommandPosition = 0;

    public static GameDirecter instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

    }

    public void HandleUpdate()
    {
        if(state == ScenarioPlayStatus.start)
        {
            curCommandPosition++;
        }else if(state == ScenarioPlayStatus.waitTextWriteDone)
        {
            HandleTextWriteDone();
        }else if(state == ScenarioPlayStatus.wait)
        {
            HandlePlayNextCommand();
        }
    }
    
    public void HandleTextWriteDone()
    {
        if (PlayerInputManager.instance.GetPlayerLeftMouseKeyDown())
        {
            if (UIManager.instance.isWriting)
            {
                UIManager.instance.WriteTextImmediately();
            }
        }
    }

    public void HandlePlayNextCommand()
    {
        if (PlayerInputManager.instance.GetPlayerLeftMouseKeyDown())
        {
            curScenarioCommands[++curCommandPosition].Run();
        }
    }

    public void HandlePlayerSelectOption(string OptionVar)
    {
        UIManager.instance.ClearOptions();
        ChangeVariable(OptionVar, 1);
        NextCommand();
        Debug.Log($"CurCommand.Type = {curScenarioCommands[curCommandPosition].type}");
    }

    #region 播放剧本
    public void PlayScenario(string Scenario)
    {
        StartCoroutine(StartPlayScenario(Scenario));
    }

    IEnumerator StartPlayScenario(string Scenario)
    {
        GameManager.instance.SwitchGameState(GameState.PlayScenario);
        state = ScenarioPlayStatus.start;
        var texts = textScript.LoadScenarioTexts(StartScenario);
        curScenarioCommands = textScript.CreatCommands(texts);
        curCommandPosition = 0;
        foreach(var command in curScenarioCommands)
        {
            if(command.type == ScenarioCommandType.flagCommand)
            {
                command.Run();
            }
        }

        curScenarioCommands[++curCommandPosition].Run();
        yield return null;
    }
    #endregion

    public void NextCommand()
    {
        state = ScenarioPlayStatus.wait;
        curScenarioCommands[++curCommandPosition].Run();
    }

    public void EndPlayScenario()
    {
        GameManager.instance.ReverseGameState();
        state = ScenarioPlayStatus.waitForNewScenario;
        curScenarioCommands.Clear();
        curCommandPosition = 0;
        ScenarioHeap.Clear();
        UIManager.instance.ClearAll();
    }

    #region 操作堆栈
    public void AddVariable(string variableName,int value)
    {
        if (!ScenarioHeap.ContainsKey(variableName))
        {
            ScenarioHeap.Add(variableName, value);
        }
        else
        {
            Debug.LogError($"Error:SameKeyInScenarioHeap KeyName = {variableName}");
        }

    }

    public void ChangeVariable(string variableName,int value)
    {
        if (ScenarioHeap.ContainsKey(variableName))
        {
            ScenarioHeap[variableName] = value;
        }
        else
        {
            Debug.LogError($"Error:NoneKeyInScenarioHeap KeyName = {variableName}");
        }
    }

    public int GetValueFromHeap(string variable)
    {
        if (ScenarioHeap.ContainsKey(variable))
        {
            return ScenarioHeap[variable];
        }

        Debug.LogError($"Error:NoneKeyInScenarioHeap KeyName = {variable}");
        return -1;
    }
    #endregion
}
