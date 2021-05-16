using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirecter : MonoBehaviour
{
    public TextScript textScript = new TextScript();
    public Dictionary<string, int> ScenarioHeap = new Dictionary<string, int>();

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
    private void Start()
    {
        if (StartScenario != null)
            StartCoroutine(PlayScenario(StartScenario));



    }

    #region 播放剧本
    IEnumerator PlayScenario(string Scenario)
    {
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


        yield return null;
    }
    #endregion
}
