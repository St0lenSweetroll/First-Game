using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 swimSpeed = new Vector2(2f, 2f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] float mushroomKick = 10f;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    SpriteRenderer mySpriteRenderer;
    AudioClip firstAudioClip;
    AudioClip secondAudioClip;
    AudioSource myAudioSource;
    float gravityScaleAtStart;

    bool isAlive = true;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAudioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        Run();
        FlipSprite();
        ClimbLadder();
        MushroomKick();
        Die();
        Drown();

    }
    void OnMove(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }
        moveInput = value.Get<Vector2>();    
    }

    void OnJump(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing")))
        {
            return;
        }
        myRigidbody.velocity += new Vector2(0f, jumpSpeed);
    }
    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }
    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        Instantiate(bullet, gun.position, transform.rotation * Quaternion.Euler(0, 0, 135));
    }
    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }

    }
    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2 (myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }
    void MushroomKick()
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Mushroom")) && !Input.GetKey(KeyCode.S))

        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, mushroomKick);
        else
        {
            return;
        }
    }
    void Die()
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")) | Input.GetKey(KeyCode.L))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            float randomDeathKickX = Random.Range(-50f, 50f);
            float randomDeathKickY = Random.Range(0f, 50f);
            Vector2 deathKick = new Vector2(randomDeathKickX, randomDeathKickY);
            myRigidbody.velocity = deathKick;
            myRigidbody.freezeRotation = false;
            myFeetCollider.enabled = false;
            myBodyCollider.sharedMaterial = null;
            mySpriteRenderer.color = Color.red;
            myAudioSource.Play();
        }
    }
    void Drown()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.freezeRotation = false;
            myFeetCollider.enabled = false;
            myBodyCollider.sharedMaterial = null;
            mySpriteRenderer.color = Color.red;
            myAudioSource.Play();
        }
    }
}
