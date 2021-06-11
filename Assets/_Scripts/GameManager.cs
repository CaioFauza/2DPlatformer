using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager _instance;

    public enum GameState { MENU, INTRO, SETTINGS, GAME, PAUSE, END };

    public GameState gameState { get; private set; }
    public delegate void ChangeStateDelegate();
    public static ChangeStateDelegate changeStateDelegate;

    public bool gameStatus, powerUpAvailable, bossFight, checkpoint;

    public int level;

    public static GameManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameManager();
        }
        return _instance;
    }

    private GameManager()
    {
        gameState = GameState.MENU;
        gameStatus = false;
        powerUpAvailable = true;
        bossFight = false;
        checkpoint = false;
        level = 0;
    }

    public void ChangeState(GameState nextState)
    {
        if (nextState == GameState.GAME && gameState != GameState.PAUSE) Reset();
        gameState = nextState;
        changeStateDelegate();
    }

    private void Reset()
    {
        gameStatus = false;
        if (!checkpoint)
        {
            powerUpAvailable = true;
            level = 0;
        }
        bossFight = false;

    }

}


