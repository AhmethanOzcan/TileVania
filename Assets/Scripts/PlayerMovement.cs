using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    bool playerHasHorizontalSpeed;
    bool playerHasVerticalSpeed;
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D myFeetCollider;
    [SerializeField] float runSpeed = 1f;
    [SerializeField] float jumpSpeed = 1f;
    [SerializeField] float deathSpeed = 1f;
    [SerializeField] float climbSpeed = 1f;
    [SerializeField] float gravityNormal = 1f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] float deathTime = 1f;
    Animator myAnimator;
    bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.gravityScale = gravityNormal;
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.gravityScale = gravityNormal;
        

        if(!isAlive) return;
        RunAndClimb();
        FlipSprite();
        Die();
    }

    
    private void FlipSprite()
    {
        playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.velocity.x), 1f);

        }
    }

    private void RunAndClimb()
    {
        
        float x = moveInput.x * runSpeed;
        float y = myRigidbody.velocity.y;
        if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            y = moveInput.y * climbSpeed;
            myRigidbody.gravityScale = 0;
        }
        Vector2 playerVelocity = new Vector2 (x, y);
        myRigidbody.velocity = playerVelocity;
        playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        if(playerHasVerticalSpeed && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myAnimator.SetBool("isRunning", false);
            myAnimator.SetBool("isClimbing", true);
        }
        else if(playerHasHorizontalSpeed)
        {
            myAnimator.SetBool("isClimbing", false);
            myAnimator.SetBool("isRunning", true);
        }
        else
        {
            myAnimator.SetBool("isClimbing", false);
            myAnimator.SetBool("isRunning", false);
        }
            
    }
    private void Die()
        {
            if(myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazard")))
            {
                isAlive = false;
                myAnimator.SetTrigger("Dying");
                myRigidbody.velocity = new Vector2 (0f, deathSpeed);
                myCapsuleCollider.isTrigger = true;
                myFeetCollider.isTrigger = true;
                StartCoroutine(ProcessDeath());
            }
        }

    void OnFire(InputValue value)
    {
        if(!isAlive) return;
        Instantiate(bullet, gun.position, transform.rotation);
    }

    void OnJump(InputValue value)
    {
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if(value.isPressed)
        {
            myRigidbody.velocity += new Vector2 (0f, jumpSpeed);
        }
    }


    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    IEnumerator ProcessDeath()
    {
        yield return new WaitForSecondsRealtime(deathTime);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

}
