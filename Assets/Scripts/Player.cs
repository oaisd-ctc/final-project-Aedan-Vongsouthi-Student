using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    EnemyAI enemyAI;
    public AudioManager audioManager;
    public Transform attackPoint;
    public float attackRange = .5f;
    public LayerMask enemyLayer;
    public int attackDamage = 40;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public int playerKnockback;

    [SerializeField] HealthBar healthBar;
    Vector2 moveInput;
    public Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;
    float runSpeed;
    float currentTime;
    float dashStart = 0f;
    [SerializeField] float dashCooldown = 2f;
    [SerializeField] float defaultRunSpeed = 5f;
    [SerializeField] float jumpScale = 5f;
    [SerializeField] float climbScale = 5f;
    [SerializeField] float dashSpeed = 100f;
    [SerializeField] float dashDuration = 2f;
    [SerializeField] Vector2 deathFling = new Vector2(30f, 20f);


    public bool isAlive = true;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        enemyAI = GetComponent<EnemyAI>();
        audioManager = GetComponent<AudioManager>();
        gravityScaleAtStart = myRigidBody.gravityScale;
        runSpeed = defaultRunSpeed;
    }

    void Update()
    {
    if (GameManager.gameManager.playerHealth.Health == 0)
       {
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentLevel + 1);
       }

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

    public void PlayerTakeDamage(int damage)
    {
        GameManager.gameManager.playerHealth.DamageUnit(damage);
        healthBar.SetHealth(GameManager.gameManager.playerHealth.Health);
    }

    private void PlayerHeal(int healing)
    {
        GameManager.gameManager.playerHealth.HealUnit(healing);
        healthBar.SetHealth(GameManager.gameManager.playerHealth.Health);
    }

    void OnFire(InputValue value)
    {
        if (Time.time >= nextAttackTime)
        {
            myAnimator.SetTrigger("Attack");
            FindObjectOfType<AudioManager>().Play("SwordSlash");
            Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            foreach (Collider2D enemy in hitenemies)
            {
                enemy.GetComponent<EnemyHealth>().EnemyTakeDamage(attackDamage);
            }

            nextAttackTime = Time.time + 1f / attackRate;
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
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

        
            Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidBody.velocity.y);
            myRigidBody.velocity = playerVelocity;
            Invoke("ReturnToDefaultSpeed", dashDuration);

        if (playerHasHorizontalSpeed)
        {
            myAnimator.SetBool("isRunning", true);
        } else {
            myAnimator.SetBool("isRunning", false);
        }
    }

    void OnDash(InputValue value)
    {
        if (Time.time > dashStart + dashCooldown)
        {
            runSpeed = dashSpeed;
            dashStart = Time.time;
            myAnimator.SetBool("isDashing", true);
        }
    }

    void ReturnToDefaultSpeed()
    {
        runSpeed = defaultRunSpeed;
        myAnimator.SetBool("isDashing", false);
        CancelInvoke("ReturnToDefaultSpeed");
    }

    void OnJump(InputValue value)
    {
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        if (isAlive)
        {
            if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                if (value.isPressed)
                {
                    myRigidBody.gravityScale = gravityScaleAtStart;
                    myRigidBody.velocity += new Vector2(0f, jumpScale);
                    myAnimator.SetBool("isJumping", playerHasVerticalSpeed);
                }

            } else if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Water")) && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                myRigidBody.gravityScale = gravityScaleAtStart;
                myRigidBody.velocity += new Vector2(0f, (jumpScale * 100));
                myAnimator.SetBool("isJumping", playerHasVerticalSpeed);
            } else {
                myAnimator.SetBool("isJumping", false);
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
            //myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
        } else {
            myRigidBody.gravityScale = gravityScaleAtStart;
            //myAnimator.SetBool("isClimbing", false);
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

    void OnDrawGizmosSelected() 
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
