using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameManager gm;
    Rigidbody2D rigidBody;
    Vector3 enemyDirection;
    int speed = 4;
    void Start()
    {
        gm = GameManager.GetInstance();
        rigidBody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
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
        rigidBody.velocity = playerDirection*speed;
        
    }
}
