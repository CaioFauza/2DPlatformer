using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameManager gm;
    Rigidbody2D rigidBody;
    Vector3 enemyDirection;
    int speed = 6;
    void Start()
    {
        gm = GameManager.GetInstance();
        rigidBody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (gm.gameState == GameManager.GameState.MENU && !gm.checkpoint) transform.position = new Vector3(-25f, -1.17f, 0f);
        if (gm.gameState != GameManager.GameState.GAME || !gm.powerUpAvailable)
        {
            rigidBody.velocity = new Vector2(0f, 0f);
            return;
        }

        if (gm.bossFight) return;
        enemyDirection = transform.localScale;

        Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;
        Vector2 playerDirection = (playerPosition - transform.position).normalized;


        if (playerDirection.x < 0)
        {
            enemyDirection.x = -0.9f;

        }
        else if (playerDirection.x > 0)
        {
            enemyDirection.x = 0.9f;
        }
        transform.localScale = enemyDirection;
        rigidBody.velocity = playerDirection * speed;

    }
}
