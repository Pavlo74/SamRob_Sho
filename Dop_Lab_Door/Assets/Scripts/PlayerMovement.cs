using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float jumpHeight = 5f;

    Vector2 movementVector;
    Rigidbody2D rb;
    Animator anim;
    CapsuleCollider2D capsuleCollider;
    public BoxCollider2D boxColl;
    public SpriteRenderer spriteRend;
    public float timer_close_door = 0;
    private float waitTime = 1.0f;
    private float timer = 0.0f;
    bool timeStart;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        Walk();
        Flip();
        Jump();
        CheckingGround();
        
        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Door")))
        {
            timeStart = true;
            boxColl.enabled = false;
            spriteRend.enabled = false;     
        }

        if (timeStart)
        {
            timer += Time.deltaTime;
        }

        if (timer > waitTime && !capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Door")))
        {
            boxColl.enabled = true;
            spriteRend.enabled = true; 
            timer = timer - waitTime;
            timeStart = false;
        }
    }


   

    public void Walk()
    {
        movementVector.x = Input.GetAxis("Horizontal");
        anim.SetFloat("moveX", Mathf.Abs(movementVector.x));
        rb.velocity = new Vector2(movementVector.x * movementSpeed, rb.velocity.y);
    }

    public void Flip()
    {
       if(movementVector.x > 0)
        {
            spriteRend.flipX = false;
        }
       else if(movementVector.x < 0)
        {
            spriteRend.flipX = true;
        }
    }

    public void Jump()
    {
        //if (!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        if ( onGround && Input.GetKeyDown(KeyCode.Space) )
        {
            rb.AddForce(Vector2.up * jumpHeight);
        }
    }

    public bool onGround;
    public LayerMask Ground;
    public Transform GroundCheck;
    private float GroundCheckRadius;

    void CheckingGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, Ground);
        anim.SetBool("onGround", onGround);
    }

    
    
}
