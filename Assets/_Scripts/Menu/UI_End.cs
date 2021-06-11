using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_End : MonoBehaviour
{
    public TextMeshProUGUI message;
    GameManager gm;

    private void OnEnable()
    {
        gm = GameManager.GetInstance();
        message.text = gm.gameStatus ? "YOU WIN!!!" : "YOU LOSE!!";
    }

    public void BackMenu()
    {
        gm.ChangeState(GameManager.GameState.MENU);
    }

    public void Update()
    {
        if (gm.gameState == GameManager.GameState.END && Input.GetKeyDown(KeyCode.Space)) BackMenu();
    }
}