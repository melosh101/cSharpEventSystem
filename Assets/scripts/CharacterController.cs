using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool isFacingRight = true;
    private bool isMoving = false;
    private Vector2 moveAction = new Vector2(0, 0);

    private static readonly int IsRunning = Animator.StringToHash("isRunning");

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            rb.AddForce(moveAction * 1, ForceMode2D.Impulse);
            return;
        }
        rb.AddForce(new Vector2(moveAction.x * 1, 0), ForceMode2D.Impulse);
    }

    private void OnMove(InputValue context)
    {
        moveAction = context.Get<Vector2>();
        
        if (moveAction.x < 0 && isFacingRight)
        {
            Flip();
        } else if (moveAction.x > 0 && !isFacingRight)
        {
            Flip();
        }
        
        
        if (!isMoving)
        {
            animator.SetBool(IsRunning, true);
            isMoving = true;
            return;
        }
        isMoving = false;

        animator.SetBool(IsRunning, false);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        var currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer.Equals(6))
        {
            Debug.Log("player is now grounded");
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer.Equals(6))
        {
            Debug.Log("no longer grounded");
            isGrounded = false;
        }
    }
}
