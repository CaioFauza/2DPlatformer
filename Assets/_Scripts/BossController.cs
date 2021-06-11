using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    GameManager gm;
    Animator animator;
    Rigidbody2D rigidBody;
    Vector3 enemyDirection;
    int speed = 8;
    void Start()
    {
        gm = GameManager.GetInstance();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (gm.gameState == GameManager.GameState.MENU)
        {
            transform.position = new Vector3(631.37f, 0.41f, 0f);
            rigidBody.velocity = new Vector2(0f, 0f);
            return;
        }

        if (transform.position.x <= 625f)
        {
            transform.position = new Vector3(625f, transform.position.y, 0f);
        }

        if (!gm.bossFight) return;
        enemyDirection = transform.localScale;

        Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;
        Vector2 playerDirection = (playerPosition - transform.position).normalized;


        if (playerDirection.x < 0)
        {
            enemyDirection.x = -3.0f;

        }
        else if (playerDirection.x > 0)
        {
            enemyDirection.x = 3.0f;
        }
        transform.localScale = enemyDirection;
        rigidBody.velocity = playerDirection * speed;
        animator.SetBool("Walk", rigidBody.velocity.x != 0 ? true : false);

    }
}
