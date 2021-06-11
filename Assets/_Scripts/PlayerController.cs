using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    GameManager gm;
    Animator animator;
    Rigidbody2D rigidBody;
    public ParticleSystem walkDust, jumpDust;

    TextMeshProUGUI checkpointText, powerUpText;

    GameObject enemy;
    float inputX, coyoteTime = 0.2f, coyoteCounter, jumpBuffer = 0.1f, jumpBufferCounter;
    bool jumpInput, isGrounded = true, jumped = false;
    Vector3 playerDirection;
    int speed = 9, jumpCounter = 0;
    public AudioClip damageSFX, bossSFX;

    void Start()
    {
        animator = GetComponent<Animator>();
        gm = GameManager.GetInstance();
        rigidBody = GetComponent<Rigidbody2D>();
        enemy = GameObject.FindWithTag("Enemy");

        checkpointText = GameObject.Find("CheckpointText").GetComponent<TextMeshProUGUI>();
        powerUpText = GameObject.Find("PowerUpText").GetComponent<TextMeshProUGUI>();

        checkpointText.gameObject.SetActive(false);
        powerUpText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (gm.gameState != GameManager.GameState.GAME) return;

        inputX = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(inputX * speed, rigidBody.velocity.y);
        jumpInput = Input.GetKeyDown(KeyCode.UpArrow);
        if (inputX == 0) walkDust.Play();
        if (Input.GetKeyDown(KeyCode.Escape) && gm.gameState == GameManager.GameState.GAME) gm.ChangeState(GameManager.GameState.PAUSE);

        ApplyLevelBoundaries();

        playerDirection = transform.localScale;
        if (inputX < 0)
        {
            playerDirection.x = -0.15f;

        }
        else if (inputX > 0)
        {
            playerDirection.x = 0.15f;
        }

        transform.localScale = playerDirection;
        animator.SetBool("Walk", inputX != 0 ? true : false);

        if (isGrounded)
        {
            jumpCounter = 0;
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (jumpInput) jumpBufferCounter = jumpBuffer;
        else jumpBufferCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0 && jumpCounter < 1)
        {
            if (!isGrounded && coyoteCounter > 0)
            {
                rigidBody.AddForce(new Vector2(0, 18.0f * (coyoteCounter < 0.45f ? 1.3f : 1f)), ForceMode2D.Impulse);
            }
            else
            {
                if (jumpInput)
                {
                    rigidBody.AddForce(new Vector2(0, 18.0f * (coyoteCounter < 0.45f ? 1.3f : 1f)), ForceMode2D.Impulse);
                }
            }
            jumpCounter++;
            jumpBufferCounter = 0;
            jumped = true;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) && rigidBody.velocity.y > 0) rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.3f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Floor")
        {
            isGrounded = true;
            animator.SetBool("Jump", false);
            if (jumped)
            {
                jumpDust.Play();
                jumped = false;
            }
        }

        if (collision.collider.tag == "PowerUp")
        {
            gm.powerUpAvailable = false;
            powerUpText.gameObject.SetActive(true);
            GameObject[] powerUps = GameObject.FindGameObjectsWithTag("PowerUp");
            foreach (GameObject powerup in powerUps)
            {
                GameObject.Destroy(powerup);
            }
        }

        if (collision.collider.tag == "EndGame")
        {
            gm.gameStatus = true;
            jumpBufferCounter = 0;
            animator.SetBool("Walk", false);
            transform.position = new Vector3(-17.69f, -2.33f, 0);
            gm.bossFight = false;
            gm.checkpoint = false;
            checkpointText.gameObject.SetActive(false);
            powerUpText.gameObject.SetActive(false);
            gm.ChangeState(GameManager.GameState.END);

        }

        if (collision.collider.tag == "Enemy" || collision.collider.tag == "Boss")
        {
            AudioManager.PlaySFX(damageSFX);
            Die();
        }

        if (collision.collider.tag == "Level1Portal")
        {
            gm.level = 1;
            transform.position = new Vector3(225.63f, -2.5f, 0f);
            enemy.transform.position = new Vector3(222.63f, 2.5f, 0f);
        }

        if (collision.collider.tag == "Level2Portal")
        {
            gm.level = 2;
            transform.position = new Vector3(395.6f, 2.07f, 0f);
            enemy.transform.position = new Vector3(389.22f, 5.09f, 0f);
            gm.checkpoint = true;
            checkpointText.gameObject.SetActive(true);
        }

        if (collision.collider.tag == "Level3Portal")
        {
            gm.level = 3;
            transform.position = new Vector3(500.51f, -0.87f, 0f);
            enemy.transform.position = new Vector3(497.58f, 1.61f, 0f);
        }

        if (collision.collider.tag == "Level4Portal")
        {
            gm.level = 4;
            transform.position = new Vector3(621f, 2.12f, 0f);
            enemy.transform.position = new Vector3(-21.04f, -1.17f, 0f);
            gm.bossFight = true;
            AudioManager.PlaySFX(bossSFX);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Floor") isGrounded = false;
        animator.SetBool("Jump", true);
    }

    public void Die()
    {
        jumpBufferCounter = 0;
        animator.SetBool("Walk", false);
        gm.bossFight = false;
        if (!gm.checkpoint)
        {
            transform.position = new Vector3(-17.69f, -2.33f, 0);
            powerUpText.gameObject.SetActive(false);
        }
        else
        {
            gm.level = 2;
        }
        gm.ChangeState(GameManager.GameState.END);
    }

    void ApplyLevelBoundaries()
    {
        if (transform.position.x <= -27f) transform.position = new Vector3(-27f, transform.position.y, 0f);

        if (transform.position.y < -4.0f)
        {
            transform.position = new Vector3(-17.69f, -2.33f, 0);
            Die();
        }

        switch (gm.level)
        {
            case 0:
                if (transform.position.x >= 129f) transform.position = new Vector3(129f, transform.position.y, 0f);
                break;
            case 1:
                if (transform.position.x <= 224f) transform.position = new Vector3(224f, transform.position.y, 0f);
                if (transform.position.x >= 305f) transform.position = new Vector3(305f, transform.position.y, 0f);
                break;
            case 2:
                if (transform.position.x <= 394f) transform.position = new Vector3(394f, transform.position.y, 0f);
                if (transform.position.x >= 469f) transform.position = new Vector3(469f, transform.position.y, 0f);
                break;
            case 3:
                if (transform.position.x <= 499f) transform.position = new Vector3(499f, transform.position.y, 0f);
                if (transform.position.x >= 589f) transform.position = new Vector3(589f, transform.position.y, 0f);
                break;
            case 4:
                if (transform.position.x <= 619f) transform.position = new Vector3(619f, transform.position.y, 0f);
                if (transform.position.x >= 693f) transform.position = new Vector3(693f, transform.position.y, 0f);
                break;

        }
    }
}
