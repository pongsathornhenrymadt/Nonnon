using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public Animator anim; // wait for die animation
    private PlayerMovement player;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement player = GetComponent<PlayerMovement>();
        currentHealth = maxHealth;
    }

    void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            //Die
            //anim.SetBool(); set dead
            Destroy(this.gameObject); // work
            Debug.Log("Die!");
        }
    }

    
    void Update()
    {
        TestDamage();
    }

    void TestDamage()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(1);
            Debug.Log("Take Damage!");
        }
    }
}
