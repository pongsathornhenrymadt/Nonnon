using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float checkRadius;
    public int maxJumpCount;

    [Header("Dash Settings")] 
    [SerializeField] private float dashVelocity = 10f;
    [SerializeField] private float dashTime = 0.5f;
    private Vector2 dashDir;
    private bool isDashing;
    private bool canDash = true;
    /*
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 1f;
    [SerializeField] private float dashCooldown = 1f;
    private bool isDashing;
    */

    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = true;
    private float moveDirection;
    private bool isJumping = false;
    private bool isGrounded;
    private int jumpCount;
    

    public Transform groundCheck;
    public LayerMask ground;
    
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        jumpCount = maxJumpCount;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        GetInput(); // input
        
        Animate(); // flip
        
        Move(); // move

        var dashInput = Input.GetKeyDown(KeyCode.F);

        if (dashInput && canDash)
        {
            isDashing = true;
            canDash = false;
            dashDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (dashDir == Vector2.zero)
            {
                dashDir = new Vector2(transform.localScale.x, 0);
            }
            //Add stoping dash
            StartCoroutine(StopDashing());
        }

        if (isDashing)
        {
            rb.velocity = dashDir.normalized * dashVelocity;
            return;
        }
    }
    private void FixedUpdate()
    {
        //check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, ground);
        if (isGrounded)
        {
            canDash = true;
            jumpCount = maxJumpCount;
        }
        
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }


    //better for physics
    
    private void Move()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y); // move
        if (isJumping)
        {
            rb.velocity = new Vector2(0f, jumpForce);
            jumpCount--;
        }
        isJumping = false;
        
        anim.SetBool("isWalking", moveDirection != 0);
    }
    
    private void Animate()
    {
        if (moveDirection > 0 && !facingRight)
        {
            FlipCharacter();
        }
        else if (moveDirection < 0 && facingRight)
        {
            FlipCharacter();
        }
    }

    private void GetInput()
    {
        moveDirection = Input.GetAxis("Horizontal"); //Input
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            isJumping = true;
        }
    }

    private void FlipCharacter()
    {
        facingRight = !facingRight; 
        transform.Rotate(0f, 180f, 0f);
    }

    /*
    private IEnumerator Dash()
    {
        isDashing = true;
        rb.velocity = new Vector2(moveDirection * dashSpeed, 0f);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

    }
    */
}
