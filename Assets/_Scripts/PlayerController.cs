using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameManager gm;
    Animator animator;
    Rigidbody2D rigidBody;
    public ParticleSystem walkDust, jumpDust;
    float inputX, coyoteTime = 0.5f, coyoteCounter, jumpBuffer = 0.2f, jumpBufferCounter;
    bool jumpInput, isGrounded = true, jumped = false;
    Vector3 playerDirection;
    int speed = 9;

    void Start()
    {
        animator = GetComponent<Animator>();
        gm = GameManager.GetInstance();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(inputX * speed, rigidBody.velocity.y);
        jumpInput = Input.GetKeyDown(KeyCode.UpArrow);
        if (inputX == 0) walkDust.Play();

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
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (jumpInput) jumpBufferCounter = jumpBuffer;
        else jumpBufferCounter -= Time.deltaTime;

        if (jumpBufferCounter >= 0 && coyoteCounter > 0)
        {
            rigidBody.AddForce(new Vector2(0, 20.0f * (coyoteCounter < 0.45f ? 1.3f : 1f)), ForceMode2D.Impulse);
            jumpBufferCounter = 0;
            jumped = true;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) && rigidBody.velocity.y > 0) rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.3f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Floor") isGrounded = true;
        animator.SetBool("Jump", false);
        if (jumped)
        {
            jumpDust.Play();
            jumped = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Floor") isGrounded = false;
        animator.SetBool("Jump", true);
    }
}
