using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb = null;
    public float speed = 15f;
    public float damage = 0.5f;
    public float delaySeconds = 3f;

    private WaitForSeconds cullDelay = null;
    
    // Start is called before the first frame update
    void Start()
    {
        cullDelay = new WaitForSeconds(delaySeconds);
        StartCoroutine(DelayedCull());
        
        rb.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // layer 6 (in Unity)  = enemy
        if (collider.gameObject.layer == 6)
        {
            IDamageable enemyAttributes = collider.GetComponent<IDamageable>();
            if (enemyAttributes != null)
            {
                enemyAttributes.ApplyDamage(damage);
            }
            
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private IEnumerator DelayedCull()
    {
        yield return cullDelay;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
