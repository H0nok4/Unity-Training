using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameState
{
    start,playing,stop,someoneWin
}
public class GameManager : MonoBehaviour
{
    //核心类，管理得分，回合开始，暂停游戏等相关功能
    public Camera startUICamera;
    public int Player1Score;
    public int Player2Score;

    public GameState gameState;

    public Paimon player1Paimon;
    public Paimon player2Paimon;
    public Physics physics;
    public Ball ball;

    public PlayerLeftController leftController;

    public TMP_Text leftScore;
    public TMP_Text rightScore;

    //====辅助位置
    [SerializeField] Transform leftPaimonStartPos;
    [SerializeField] Transform rightPaimonStartPos;
    [SerializeField] Transform leftBallStartPos;
    [SerializeField] Transform rightBallStartPos;

    public Stack<PaimonUI> paimonUI;
    public PaimonUI startUI;
    public PaimonUI optionUI;
    public PaimonUI inGameOptionUI;
    public PaimonUI gameoverUI;

    public bool preTurnWinnerIsRight = false;
    public bool isGamePause;

    public static GameManager instance;

    private void Start()
    {
        //单例模式
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }

        gameState = GameState.start;
        paimonUI = new Stack<PaimonUI>();
        paimonUI.Push(startUI);
    }

    public void Update()
    {
        if (gameState == GameState.playing)
        {
            //每帧更新玩家，球，物理系统
            player1Paimon.HandleUpdate();
            player2Paimon.HandleUpdate();
            ball.HandleUpdate();
            physics.HandleUpdate();
        }


    }

    public void FixedUpdate()
    {
        if (gameState == GameState.playing)
        {
            //每1/60秒调用一次物理系统的更新，还有接受玩家的输入
            physics.HandleFixedUpdate();
            leftController.HandleFixedUpdate();
        }

    }

    public void StartSinglePlayerGame()
    {
        //开始一个新的单人游戏，需要将分数，位置初始化
        startUICamera.gameObject.SetActive(false);
        gameState = GameState.playing;
        Player1Score = 0;
        Player2Score = 0;
        StartNewTurn();
        while(paimonUI.Count > 0)
        {
            var ui = paimonUI.Pop();
            ui.HideThisUI();
        }
    }

    public void StartMultiPlayerGame()
    {
        //开启多人游戏
        gameState = GameState.stop;
        paimonUI.Push(inGameOptionUI);
        paimonUI.Peek().ShowThisUI();
        //TO DO：添加多人游戏功能
    }

    public void UIBack()
    {
        //UI框架的返回功能
        if (paimonUI.Count > 0)
        {
            var ui = paimonUI.Pop();
            ui.HideThisUI();
            if(paimonUI.Count > 0)
            {
                var curUI = paimonUI.Peek();
                curUI.ShowThisUI();
            }

        }
    }

    public void StartNewTurn()
    {
        //开始一个新的回合
        if (preTurnWinnerIsRight)
        {
            ball.transform.position = rightBallStartPos.position;
            
        }
        else
        {
            ball.transform.position = leftBallStartPos.position;
        }
        //清空球加速度，初始化位置
        ball.velocity = new Velocity();
        ball.isPowerHit = false;
        player1Paimon.transform.position = leftPaimonStartPos.position;
        player2Paimon.transform.position = rightPaimonStartPos.position;
        UpdateScoreBoard();



    }

    public void UpdateScoreBoard()
    {
        //更新分数板
        leftScore.text = Player1Score > 9 ? Player1Score.ToString() : "0" + Player1Score.ToString();
        rightScore.text = Player2Score > 9 ? Player2Score.ToString() : "0" + Player2Score.ToString();
    }

    IEnumerator GameEnd(bool isRightWinner)
    {
        //游戏结束
        gameState = GameState.someoneWin;
        //TO DO:胜者胜利动画，败者失败动画
        if (isRightWinner)
        {
            //TO DO:左边失败，右边胜利
            
            paimonUI.Push(gameoverUI);
            paimonUI.Peek().ShowThisUI();
            
        }
        else
        {
            //TO DO:左边胜利，右边失败
        }

        yield return new WaitForSeconds(1);

        //TO DO:返回主菜单或者重新开始
    }

    public void BackToStartUI()
    {
        //从单人游戏中,返回主菜单界面，初始化游戏的数据
        StartNewTurn();
        Player1Score = 0;
        Player2Score = 0;
        UpdateScoreBoard();
        startUICamera.gameObject.SetActive(true);
        while (paimonUI.Count > 0)
        {
            var ui = paimonUI.Pop();
            ui.HideThisUI();
        }
        //弹出所有的UI
        paimonUI.Push(startUI);
        //压入初始UI
        paimonUI.Peek().ShowThisUI();
        gameState = GameState.start;

    }

    public void ShowOptionUI()
    {
        gameState = GameState.stop;
        //TO DO:显示设置页面]
        paimonUI.Peek().HideThisUI();
        paimonUI.Push(optionUI);
        paimonUI.Peek().ShowThisUI();

    }

    public void ShowInGameOptionUI()
    {
        //显示游戏中的设置页面
        gameState = GameState.stop;
        paimonUI.Push(inGameOptionUI);
        paimonUI.Peek().ShowThisUI();
    }

    public void QuitGameButton()
    {
        //离开游戏按钮
        Application.Quit();
    }
    public void PlayerGetScore(bool isPlayer1)
    {
        //得分
        if (isPlayer1)
        {
            preTurnWinnerIsRight = false;
            Player1Score++;
            //TO DO:以球在玩家一一边的前提开始新回合
        }
        else
        {
            preTurnWinnerIsRight = true;
            Player2Score++;
            //TO DO:以球在玩家二一边的前提开始新回合
        }

        if (Player1Score >= 15)
        {
            StartCoroutine(GameEnd(false));
        }
        else if(Player2Score >= 15)
        {
            StartCoroutine(GameEnd(true));
        }

        if(gameState != GameState.someoneWin)
        {
            StartNewTurn();
        }



    }
}
