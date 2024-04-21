using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerStateController : MonoBehaviour
{
    public string playerState;
    public Animator animator;
    public Rigidbody2D rb;
    public Transform groundCheckLocation;
    public LayerMask groundLayer;
    public bool alive = true;
    public PlayerMovement pMovement;
    public PlayerCombat pCombat;
    public bool c4nM0v3;
    public TextMeshProUGUI text;
    public TextMeshProUGUI text1;


    //Adjustable Values
    [Range(0f, 100f)][SerializeField] public float runSpeed = 12f;
    [Range(0f, 50f)][SerializeField] public float airSpeed = 10f;
    [Range(0f, 0.3f)][SerializeField] public float _movementSmoothing = 0.1f;
    [Range(0f, 1500f)][SerializeField] public float jumpPower = 850;
    [Range(0f, 10f)][SerializeField] public float playerHealth = 5f;
    public float y;

    //CURRENT STATES
    public string PLAYER_STATE_IDLE = "Idle";
    public string PLAYER_STATE_RUNLEFT = "run_LEft";
    public string PLAYER_STATE_RUNRIGHT = "run_RIght";
    public string PLAYER_STATE_JUMP_ASCEND = "jump_rise";
    public string PLAYER_STATE_JUMP_PEAK = "jump_mid";
    public string PLAYER_STATE_JUMP_DESCEND = "jump_fall";
    public string PLAYER_STATE_ATTACK1 = "attack_1";
    public string PLAYER_STATE_ATTACK2 = "attack_2";
    public string PLAYER_STATE_ATTACK3 = "attack_3";
    public string PLAYER_STATE_AIRATTACK1 = "attack_1";
    public string PLAYER_STATE_AIRATTACK2 = "attack_2";
    public string PLAYER_STATE_BLOCK = "Blocking";
    public string PLAYER_STATE_DEAD = "dead";

    //Animation PARAMETERS
    const string ANIM_PARAM_HORIZONTAL = "horizontalVelocity";
    const string ANIM_PARAM_VERTICAL = "verticalVelocity";
    const string ANIM_PARAM_ONGROUND = "OnGround";
    public string ANIM_TRIGGER_ATTACK1 = "Attack_1";
    public string ANIM_TRIGGER_ATTACK2 = "Attack_2";
    public string ANIM_TRIGGER_ATTACK3 = "Attack_3";

    //Animation Labels
    public string ANIM_IDLE = "Idle";
    const string ANIM_RUNRIGHT = "RunningRight";
    const string ANIM_RUNLEFT = "RunningLeft";
    const string ANIM_RISE = "Rise";
    const string ANIM_PEAK = "Peak";
    const string ANIM_FALL = "Fall";
    public string ANIM_BLOCK = "Block_Start";
    public string ANIM_BLOCK_START = "Block_Start";
    const string ANIM_BLOCK_EXIT = "Block_Exit";

    private void Start()
    {
        //groundCheckLocation = this.gameObject.transform.GetChild(0);
        Vector3 startScale = transform.localScale;
        alive = true;
    }

    private void Update()
    {
        StateController();
        //ChangeAnimationState();
        pMovement.CheckCoyoteTime();
        pMovement.SetGravityScale(playerState);
        pMovement.HandleJump();
        pMovement.Flip();
        pCombat.Attack();
        pCombat.Die();
        c4nM0v3 = CanMove();
        text.text = playerState;
        text1.text = rb.velocity.x.ToString();
    }

    private void FixedUpdate()
    {
        pMovement.Move();
        pCombat.Block();
    }

    private void ChangeAnimationState()
    {
        Vector2 velocity = rb.velocity;
        float magnitudeX = velocity.x;
        float magnitudeY = velocity.y;
        animator.SetBool(ANIM_PARAM_ONGROUND, OnGround());
        animator.SetFloat(ANIM_PARAM_HORIZONTAL, Input.GetAxisRaw("Horizontal"));
        animator.SetFloat(ANIM_PARAM_VERTICAL, magnitudeY);
    }

    private void StateController()
    {
        if (alive == false)
        {
            playerState = "dead";
        }
        else if ((animator.GetCurrentAnimatorStateInfo(0).IsName("Block_Exit") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1) || pCombat.blocking)
        {
            playerState = PLAYER_STATE_BLOCK;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("keepblock") && animator.GetCurrentAnimatorStateInfo(0).IsName("Block_Exit"))
        {
            playerState = PLAYER_STATE_BLOCK;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName(ANIM_TRIGGER_ATTACK1) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 || animator.GetCurrentAnimatorStateInfo(0).IsName(ANIM_TRIGGER_ATTACK2) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            playerState = PLAYER_STATE_IDLE;
            animator.Play(ANIM_IDLE);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName(ANIM_TRIGGER_ATTACK1))
        {
            playerState = PLAYER_STATE_ATTACK1;
        }

        else if (animator.GetCurrentAnimatorStateInfo(0).IsName(ANIM_TRIGGER_ATTACK2))
        {
            playerState = PLAYER_STATE_ATTACK2;
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                playerState = PLAYER_STATE_IDLE;
            }
        }

        else if (animator.GetCurrentAnimatorStateInfo(0).IsName(ANIM_TRIGGER_ATTACK3))
        {
            playerState = PLAYER_STATE_ATTACK3;
        }

        else if (OnGround() && CanMove())
        {
            Vector2 velocity = rb.velocity;
            float magnitude = Input.GetAxisRaw("Horizontal");


            if (magnitude < 0.5f && magnitude > -0.5f)
            {
                playerState = PLAYER_STATE_IDLE;
                animator.Play(ANIM_IDLE);
            }
            else if (magnitude < -0.5f)
            {
                playerState = PLAYER_STATE_RUNLEFT;
                animator.Play(ANIM_RUNLEFT);
            }
            else if (magnitude > 0.5f)
            {
                playerState = PLAYER_STATE_RUNRIGHT;
                animator.Play(ANIM_RUNRIGHT);
            }
        }

        else
        {
            Vector2 velocity = rb.velocity;
            float magnitude = velocity.y;
            if (magnitude <= 2.5f && magnitude > -1.5f)
            {
                playerState = PLAYER_STATE_JUMP_PEAK;
                animator.Play(ANIM_PEAK);
            }
            else if (magnitude > 0f)
            {
                playerState = PLAYER_STATE_JUMP_ASCEND;
                animator.Play(ANIM_RISE);
            }
            else if (magnitude < -1.5f)
            {
                playerState = PLAYER_STATE_JUMP_DESCEND;
                animator.Play(ANIM_FALL);
            }
        }
    }



    public bool CanMove()
    {
        if (OnGround() && animator.GetCurrentAnimatorStateInfo(0).IsName(ANIM_TRIGGER_ATTACK1) || animator.GetCurrentAnimatorStateInfo(0).IsName(ANIM_TRIGGER_ATTACK2) || animator.GetCurrentAnimatorStateInfo(0).IsName(ANIM_TRIGGER_ATTACK3))
        {
            rb.velocity = Vector2.zero;
            return false;
        } else if (pCombat.blocking || animator.GetCurrentAnimatorStateInfo(0).IsName("keepblock") || animator.GetCurrentAnimatorStateInfo(0).IsName("Block_Exit"))
        {
            return false;
        }
        else if (playerState == PLAYER_STATE_DEAD)
        {
            rb.velocity = Vector2.zero;
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool OnGround()
    {
        if (Physics2D.OverlapCapsule(groundCheckLocation.position, new Vector2(0.36f, 0.3f), CapsuleDirection2D.Horizontal, 0, groundLayer))
        {
            return true;
        }
        return false;
    }

}
