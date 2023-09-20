using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float healthPool = 10f;
    public float speed = 5f;
    public float jumpForce = 6f;
    public float groundedLeeway = 0.1f;

    private Rigidbody2D rb = null;
    private float currentHealth = 1f;

    public Rigidbody2D Rb2D
    {
        get { return rb; }
        protected set { rb = value; }
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
        protected set { currentHealth = value; }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healthPool;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected bool CheckGrounded()
    {
        return Physics2D.Raycast(transform.position, -Vector2.up, groundedLeeway);
    }
    
    protected virtual void Die()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
