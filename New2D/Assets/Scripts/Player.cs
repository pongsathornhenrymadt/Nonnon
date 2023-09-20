using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character, IDamageable
{
    [Header("Input")]
    public KeyCode meleeAttackKey = KeyCode.Mouse0;

    public KeyCode rangeAttackKey = KeyCode.Mouse1;
    public KeyCode jumpKey = KeyCode.Space;
    public string xMoveAxis = "Horizontal";
    public Vector2 moveVector2;

    /*
    [Header("Movement")]
    public float speed = 5f;
    public float jumpForce = 6f;
    public float groundedLeeway = 0.1f;
    */

    [Header("Combat")] 
    public Transform meleeAttackOrigin = null;
    public Transform rangeAttackOrigin = null;
    public GameObject projectile = null;
    public float meleeAttackRadius = 0.6f;
    public float meleeDamage = 2f;
    public float meleeAttackDelay = 1.1f;
    public float rangedAttackDelay = 0.3f;
    public LayerMask enemyLayer = 8;
    
    private Rigidbody2D rb2D = null;
    private float moveIntentionX = 0;
    private bool attempJump = false;
    private bool attempMeleeAttack = false;
    private bool attempRangedAttacked = false;
    private float timeUntilMeleeReadied = 0f;
    private float timeUntilRangedReadied = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Rigidbody2D>())
        {
            rb2D = GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        HandleJump();
        HandleAttackMelee();
        HandleRangeAttack();
    }

    private void FixedUpdate()
    {
        HandleRun();
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, -Vector2.up * groundedLeeway, Color.blue);
        if (meleeAttackOrigin != null)
        {
            Gizmos.DrawWireSphere(meleeAttackOrigin.position, meleeAttackRadius);
        }
    }

    public void GetInput()
    {
        moveIntentionX = Input.GetAxis(xMoveAxis);
        attempMeleeAttack = Input.GetKeyDown(meleeAttackKey);
        attempRangedAttacked = Input.GetKeyDown(rangeAttackKey);
        attempJump = Input.GetKeyDown(jumpKey);
    }

    private void HandleRun()
    {
        if (moveIntentionX > 0 && transform.rotation.y == 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        } 
        else if(moveIntentionX < 0 && transform.rotation.y != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
        rb2D.velocity = new Vector2(moveIntentionX * speed, rb2D.velocity.y);
    }
    
    private void HandleJump()
    {
        if (attempJump && CheckGrounded())
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
        }
    }
    
    private void HandleAttackMelee()
    {
        if (attempMeleeAttack && timeUntilMeleeReadied <= 0)
        {
            Debug.Log("Player: Attempting Melee Attack");
            Collider2D[] overlappedColliders =
                Physics2D.OverlapCircleAll(meleeAttackOrigin.position, meleeAttackRadius, enemyLayer);
            for (int i = 0; i < overlappedColliders.Length; i++)
            {
                IDamageable enemyAttributes = overlappedColliders[i].GetComponent<IDamageable>();
                if (enemyAttributes != null)
                {
                    enemyAttributes.ApplyDamage(meleeDamage);
                }
            }

            timeUntilMeleeReadied = meleeAttackDelay;
        }
        else
        {
            timeUntilMeleeReadied -= Time.deltaTime;
        }
    }

    private void HandleRangeAttack()
    {
        if (attempRangedAttacked && timeUntilRangedReadied <= 0)
        {
            Debug.Log("Player: Attempting Ranged Attack");
            Instantiate(projectile, rangeAttackOrigin.position, rangeAttackOrigin.rotation);

            timeUntilRangedReadied = rangedAttackDelay;
        }
        else
        {
            timeUntilRangedReadied -= Time.deltaTime;
        }
    }

    private bool CheckGrounded()
    {
        return Physics2D.Raycast(transform.position, -Vector2.up, groundedLeeway);
    }
    
    public virtual void ApplyDamage(float amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
}
