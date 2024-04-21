using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{

    private int health = 7;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("playerAttack")) 
        {
            health -= 1;
            Debug.Log(gameObject.name + " health: " + health);
            animator.Play("Hit", -1, 0f);
        }
    }
}
