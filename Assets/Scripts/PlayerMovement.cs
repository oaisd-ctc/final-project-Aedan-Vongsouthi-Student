using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    bool hasDashed = false;
    bool isPressed = false;
    float gravityScaleAtStart;
    [SerializeField] float speedScale = 5f;
    [SerializeField] float jumpScale = 5f;
    [SerializeField] float climbScale = 5f;
    [SerializeField] float dashScale = 100f;
    [SerializeField] Vector2 deathFling = new Vector2(30f, 20f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;


    bool isAlive = true;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
    }

    void Update()
    {
        if (isAlive)
        {
            Run();
            FlipSprite();
            ClimbLadder();
            PlayerDeath();
        } else {
            return;
        }
    }

    void OnFire(InputValue value)
    {
        if(isAlive)
        {
            Instantiate(bullet, gun.position, transform.rotation);
        } else {
            return;
        }
    }

    void OnMove(InputValue value)
    {
        if(isAlive)
        {
            moveInput = value.Get<Vector2>();
            //Debug.Log(moveInput);
        } else {
            return;
        }
    }

    void Run()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > 0.2f;

        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            Vector2 playerVelocity = new Vector2((moveInput.x * speedScale) / 1.5f, myRigidBody.velocity.y / 1.1f);
            myRigidBody.velocity = playerVelocity;
        } else {
            Vector2 playerVelocity = new Vector2(moveInput.x * speedScale, myRigidBody.velocity.y);
            myRigidBody.velocity = playerVelocity;
        }

        if (isPressed)
        {
            Vector2 dashVelocity = new Vector2((moveInput.x * dashScale), myRigidBody.velocity.y);
            myRigidBody.velocity = dashVelocity;
            hasDashed = true;
            isPressed = false;
        } else if (hasDashed){
            Vector2 playerVelocity = new Vector2(moveInput.x * speedScale, myRigidBody.velocity.y);
            myRigidBody.velocity = playerVelocity;
            hasDashed = false;
        }

        if (playerHasHorizontalSpeed)
        {
            myAnimator.SetBool("isRunning", true);
        } else {
            myAnimator.SetBool("isRunning", false);
        }
    }

    void OnDash(InputValue value)
    {
        if (value.isPressed)
        {
            isPressed = true;
        }
    }

    void OnJump(InputValue value)
    {
        if (isAlive)
        {
            if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                if (value.isPressed)
                {
                    myRigidBody.gravityScale = gravityScaleAtStart;
                    myRigidBody.velocity += new Vector2(0f, jumpScale);
                }

            } else if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Water")) && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                myRigidBody.gravityScale = gravityScaleAtStart;
                myRigidBody.velocity += new Vector2(0f, (jumpScale * 100));
            } else {
                return;
            }
        } else {
            return;
        }
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;

        if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            Vector2 climbingVelocity = new Vector2(myRigidBody.velocity.x, moveInput.y * climbScale);
            myRigidBody.velocity = climbingVelocity;
            myRigidBody.gravityScale = 0f;
            myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
        } else {
            myRigidBody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }
    }

    void PlayerDeath()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")) || myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidBody.velocity = deathFling;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }

    }

    void OnReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
