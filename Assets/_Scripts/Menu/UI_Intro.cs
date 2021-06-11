using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Intro : MonoBehaviour
{
    GameManager gm;

    public GameObject player, enemy, powerUp;

    private void OnEnable()
    {
        gm = GameManager.GetInstance();
    }
    public void StartGame()
    {
        GameObject[] powerUps = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (GameObject powerup in powerUps)
        {
            GameObject.Destroy(powerup);
        }
        Instantiate(powerUp, new Vector3(254.50f, -1.46f, 0), Quaternion.identity);
        gm.ChangeState(GameManager.GameState.GAME);
    }

    public void Update()
    {
        if (gm.gameState == GameManager.GameState.INTRO && Input.GetKeyDown(KeyCode.Space)) StartGame();
    }
}
