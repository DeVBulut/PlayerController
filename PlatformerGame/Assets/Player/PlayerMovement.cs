using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public PlayerStateController controller;
    public Rigidbody2D rb;
    public GameObject playerObject;
    public SpriteRenderer spriteRenderer;
    public TextMeshProUGUI text3;

    //Vertical
    private Vector3 _velocity = Vector3.zero;
    private float coyoteTimeValue;
    private float defaultGravityScale;
    private float fallMultiplier = 1.2f;
    private float peakMultiplier = 0f;
    private float ascendMultiplier = 1.4f;
    private float maxCoyoteTimeValue = .2f;
    private bool justJumped;

    //Horizontal
    private float inputAxis;

    //Sprite
    private float defaultSpriteScale;
    private float negativeDefaultSpriteScale;

    void Start()
    {
        Vector3 startScale = playerObject.transform.localScale;
        defaultGravityScale = rb.gravityScale;
        defaultSpriteScale = startScale.x;
        negativeDefaultSpriteScale = -startScale.x;
    }

    public void Move()
    {
        Vector2 velocity = rb.velocity;
        if (controller.CanMove())
        {
            if (controller.OnGround() && velocity.x < 3 && velocity.x > -3)
            {

                inputAxis = Input.GetAxisRaw("Horizontal") * controller.runSpeed;
                Vector3 targetVelocity = new Vector2(inputAxis, rb.velocity.y);
                //Going Right
                if (inputAxis > 0) {
                    
                    rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref _velocity, controller._movementSmoothing + 0.1f);
                }
                //Going Left
                else if (inputAxis < 0)
                {
                    rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref _velocity, controller._movementSmoothing + 0.1f);
                }
                //No Input
                else
                {
                    rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref _velocity, controller._movementSmoothing + .05f);
                }

            }
            else if (controller.OnGround())
            {

                inputAxis = Input.GetAxisRaw("Horizontal") * controller.runSpeed;
                Vector3 targetVelocity = new Vector2(inputAxis, rb.velocity.y);
                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref _velocity, controller._movementSmoothing + .05f);
            }
            else if(!controller.OnGround())
            {

                inputAxis = Input.GetAxisRaw("Horizontal") * controller.airSpeed;
                Vector3 targetVelocity = new Vector2(inputAxis, rb.velocity.y);
                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref _velocity, controller._movementSmoothing);
            }
        }
        else
        {
            if (controller.OnGround())
            {

                inputAxis = Input.GetAxisRaw("Horizontal") * (controller.runSpeed -2f);
                Vector3 targetVelocity = new Vector2(inputAxis, rb.velocity.y);
                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref _velocity, controller._movementSmoothing);
            }
        }
    }


    public void HandleJump()
    {
        if (CheckCoyoteTime() && !justJumped && Input.GetKeyDown(KeyCode.Space))
        {
            justJumped = true;
            JumpPhysics();
        }
    }


    private void JumpPhysics()
    {
        if (justJumped)
        {
            justJumped = false;
            Vector3 velocity = rb.velocity;
            velocity.y = 0f;
            rb.velocity = velocity;
            rb.AddForce(new Vector2(rb.velocity.x, controller.jumpPower));
            
        }
    }

    public bool CheckCoyoteTime()
    {
        if (controller.OnGround())
        {
            coyoteTimeValue = maxCoyoteTimeValue;
        }
        else
        {
            coyoteTimeValue -= Time.deltaTime;
        }
        Vector2 velocity = rb.velocity;
        float magnitude = velocity.y;
        if (magnitude > 1f)
        {
            return false;
        }
        else if (coyoteTimeValue > 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetGravityScale(string state)
    {
        if (state == controller.PLAYER_STATE_JUMP_PEAK)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (peakMultiplier) * Time.deltaTime;
        }
        else if (state == controller.PLAYER_STATE_JUMP_DESCEND)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime;

        }
        else if (state == controller.PLAYER_STATE_JUMP_ASCEND)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (ascendMultiplier) * Time.deltaTime;
        }
        if (state == controller.PLAYER_STATE_ATTACK1 || state == controller.PLAYER_STATE_ATTACK1)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (peakMultiplier) * Time.deltaTime;
        }
        else
        {
            rb.gravityScale = defaultGravityScale;
        }

    }

    public void Flip()
    {
        if (controller.CanMove())
        {
            float ax = Input.GetAxisRaw("Horizontal");
            if (ax < 0f)
            {
                spriteRenderer.flipX = true;
            }
            else if(ax > 0f)
            {
                spriteRenderer.flipX = false;
            }
        }
    }
}
