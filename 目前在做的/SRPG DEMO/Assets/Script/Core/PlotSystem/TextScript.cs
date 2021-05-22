using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;


public class TextScript
{
    public const string s_TextCommand = "text";
    public const string s_EndTextCommand = "endtext";
    public const string s_OptionCommand = "option";
    public const string s_EndOptionCommand = "endoption";
    public const string s_IfCommand = "if";
    public const string s_GotoCommand = "goto";
    public const string s_Flag = "#";
    public const string s_ScenarioStart = "$Start";
    public const string s_ScenarioEnd = "$End";
    public const string ScenarioPath = "Assets/Resources/Scenario";

    #region 格式化剧本
    //输入:剧本在Resource/Scenario下的名字
    //效果:将剧本文件按冒号分成一句一句的
    //输出:分成每句后的剧本语句
    public List<string> LoadScenarioTexts(string scenarioName)
    {
        
        var text = Resources.Load("Scenario/" + scenarioName);
        string[] allLineText = text.ToString().Split(new string[] { "/n", ";" }, System.StringSplitOptions.RemoveEmptyEntries);
        List<string> scenarioTexts = new List<string>();
        foreach (string s in allLineText)
        {

            var i = s.Trim();
            var result = i.TrimEnd(';');
            scenarioTexts.Add(result);
        }

        return scenarioTexts;
    }
    #endregion

    #region 创建命令
    //输入:格式化好的剧本
    //效果:根据每句格式化好的剧本，创建相对应的命令，存入命令队列中。
    //输出:无
    public List<ScenarioCommand> CreatCommands(List<string> texts)
    {
        List<ScenarioCommand> commands = new List<ScenarioCommand>();
        for (int i = 0; i < texts.Count; i++)
        {
            string[] codes = texts[i].Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries);
            switch (codes[0])
            {
                case s_ScenarioStart:
                    commands.Add(new StartCommand(new string[] { $"{i}" }));
                    break;
                case s_ScenarioEnd:
                    commands.Add(new EndCommand(new string[] { $"{i}" }));
                    break;
                case s_TextCommand:
                    List<string> temp = new List<string>();
                    int startPosition = i;
                    for (int j = 1; j < codes.Length; j++)
                    {
                        temp.Add(codes[j]);
                    }
                    while(texts[++i] != s_EndTextCommand )
                    {
                        if(i >= texts.Count - 1)
                        {
                            Debug.LogError($"Error:NoEndTextCommand ErrorPosition{startPosition}");
                        }
                        temp.Add(texts[i]);
                    }
                    for(int j = 2; j < temp.Count; j++)
                    {
                        List<string> tempParameter = new List<string>();
                        if(temp[0] != "top" && temp[0] != "mid" && temp[0] != "down")
                        {
                            Debug.LogError($"Error:NoLegalPosition ErrorPosition = {startPosition}");
                        }
                        else
                        {
                            tempParameter.Add(temp[0]);
                        }

                        tempParameter.Add(temp[1]);
                        tempParameter.Add(temp[j]);
                        TextCommand textCommand = new TextCommand(tempParameter.ToArray());
                        commands.Add(textCommand);
                    }
                    break;
                case s_OptionCommand:
                    List<string> optionCommandText = new List<string>();
                    while(texts[++i] != s_EndOptionCommand)
                    {
                        var options = texts[i].Split(' ');
                        StringBuilder optionText = new StringBuilder();
                        for(int j = 1; j < options.Length; j++)
                        {
                            optionText.Append(options[j]);
                            optionText.Append(" ");
                            
                        }
                        optionCommandText.Add(options[0]);
                        optionCommandText.Add(optionText.ToString());
                    }
                    OptionCommand optionCommand = new OptionCommand(optionCommandText.ToArray());
                    commands.Add(optionCommand);
                    break;
                case s_GotoCommand:
                    GotoCommand gotoCommand = new GotoCommand(new string[] { codes[1] });
                    commands.Add(gotoCommand);
                    break;
                case s_IfCommand:
                    List<string> ifCommandText = new List<string>();
                    for(int j = 1; j < 4; j++)
                    {
                        ifCommandText.Add(codes[j]);
                    }
                    ifCommandText.Add(codes[codes.Length - 1]);
                    IfGotoCommand ifCommand = new IfGotoCommand(ifCommandText.ToArray());
                    commands.Add(ifCommand);
                    break;
                default:
                    if (codes[0].StartsWith("#"))
                    {
                        List<string> flagCommandTexts = new List<string>();
                        flagCommandTexts.Add(codes[0]);
                        flagCommandTexts.Add(commands.Count.ToString());
                        FlagCommand flagcommand = new FlagCommand(flagCommandTexts.ToArray());
                        commands.Add(flagcommand);
                    }
                    break;
            }

        }

        testCommands(commands);
        return commands;
    }
    #endregion

    public void testCommands(List<ScenarioCommand> commands)
    {
        for(int i = 0;i<commands.Count;i++)
        {
            Debug.Log($"Command[{i}] = " + commands[i].type);
            for(int j = 0; j < commands[i].parameter.Length; j++)
            {
                Debug.Log($"Command[{i}].paramater[{j}] = " + commands[i].parameter[j]);
            }

        }
    }

}
