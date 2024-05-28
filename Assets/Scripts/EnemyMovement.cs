using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    public AudioSource myAudioSource1;
    public AudioSource myAudioSource2;
    [SerializeField] private string newLayerName = "Default";
    bool stopMovement;
    void Start()
    {
        stopMovement = false;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        if (stopMovement == false)
        {
            myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        {
            moveSpeed = -moveSpeed;
            FlipEnemyFacing();
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Die();
        }
    }
    void StopMovement()
    {
        if (stopMovement == true)
        {
            myRigidbody.velocity = new Vector2(0f, 0f);
        }

    }
    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), 1f);

    }
    void Die()
    {
        stopMovement = true;
        StopMovement();
        myRigidbody.freezeRotation = false;
        myAnimator.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        float randomDeathKickX = Random.Range(-20f, 20f);
        float randomDeathKickY = Random.Range(0f, 20f);
        Vector2 deathKick = new Vector2(randomDeathKickX, randomDeathKickY);
        myRigidbody.velocity = deathKick;
        Destroy(gameObject, 2f);
    }
}
