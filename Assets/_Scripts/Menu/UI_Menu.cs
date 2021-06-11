using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Menu : MonoBehaviour
{
    GameManager gm;
    public GameObject player, enemy, powerUp;

    private void OnEnable()
    {
        gm = GameManager.GetInstance();
    }

    public void StartGame()
    {
        if (gm.checkpoint)
        {
            player.transform.position = new Vector3(397.0f, 2.07f, 0f);
            enemy.transform.position = new Vector3(389.22f, 5.09f, 0f);
            GameObject[] powerUps = GameObject.FindGameObjectsWithTag("PowerUp");
            foreach (GameObject powerup in powerUps)
            {
                GameObject.Destroy(powerup);
            }
            Instantiate(powerUp, new Vector3(254.50f, -1.46f, 0), Quaternion.identity);
            gm.ChangeState(GameManager.GameState.GAME);
        }
        else
        {
            player.transform.position = new Vector3(-17.69f, -2.33f, 0);
            gm.ChangeState(GameManager.GameState.INTRO);
        }
    }

    public void Update()
    {
        if (gm.gameState == GameManager.GameState.MENU && Input.GetKeyDown(KeyCode.Space)) StartGame();
    }

    public void VolumeSettings()
    {
        gm.ChangeState(GameManager.GameState.SETTINGS);
    }

    public void Exit()
    {
        Application.Quit();
    }
}