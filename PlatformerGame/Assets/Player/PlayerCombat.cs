using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public PlayerStateController controller;
    public Rigidbody2D rb;
    public Transform spawnPoint;

    //Combat Values
    private float attackCooldown = 0.15f;
    private float nextTimeToAttack = 0f;
    public bool blocking = false;
    public int count = 0;

    private float block_clock;
    private float blockCooldown = .4f;

    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && controller.CanMove() == false)
        {
            //if (animator.GetCurrentAnimatorStateInfo(0).IsName(controller.ANIM_TRIGGER_ATTACK2))
            //{
            //    Debug.Log("Attack3");
            //    animator.SetTrigger(controller.ANIM_TRIGGER_ATTACK3);
            //    nextTimeToAttack = Time.time + attackCooldown;
            //}
            //else
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(controller.ANIM_TRIGGER_ATTACK1) || animator.GetCurrentAnimatorStateInfo(0).IsName(controller.ANIM_TRIGGER_ATTACK1))
            {
                Debug.Log("Attack2");
                animator.SetTrigger(controller.ANIM_TRIGGER_ATTACK2);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Mouse0) && controller.CanMove() && Time.time > nextTimeToAttack)
        {
            Debug.Log("First Attack");
            animator.Play(controller.ANIM_TRIGGER_ATTACK1);
            nextTimeToAttack = Time.time + attackCooldown;
        }
    }

    public void Block()
    {
        if (Input.GetKey(KeyCode.Mouse1) && block_clock < 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Block_Start") && !animator.GetCurrentAnimatorStateInfo(0).IsName("keepblock")) {
                animator.Play("Block_Start");
            }
            blocking = true;
            rb.velocity = Vector2.zero;
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("keepblock")) {
                animator.Play("Block_Exit");
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Block_Exit"))
            {
                block_clock = blockCooldown;
            }
            blocking = false;
            block_clock -= Time.deltaTime;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BlockStart") || animator.GetCurrentAnimatorStateInfo(0).IsName("keepblock"))
        {
            blocking = true;
            rb.velocity = Vector2.zero;
        }
    }

    public void Die()
    {
        if (controller.playerHealth <= 0 && controller.alive)
        {
            Debug.Log("Dead");
            controller.alive = false;
            rb.velocity = Vector2.zero;
            animator.Play("Die");
            StartCoroutine(waitfordie(4));
        }
    }

    private IEnumerator waitfordie(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        controller.playerHealth = 10;
        rb.transform.position = spawnPoint.position;
        controller.alive = true;
        animator.Play("Idle");
    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Health>() != null)
        {
            Health health = collision.GetComponent<Health>();
            health.Damage();
        }
    }


}
