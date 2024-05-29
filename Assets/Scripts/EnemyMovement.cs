using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] int maxHealth = 3;
    int currentHealth;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    GameObject Lario;
    BoxCollider2D playerFeetCollider;
    public AudioSource myAudioSource1;
    public AudioSource myAudioSource2;
    bool stopMovement;
    int facingDirection = 1;
    [SerializeField] int pointsForCoinPickup = 50;
    void Start()
    {
        stopMovement = false;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        Lario = GameObject.FindGameObjectWithTag("Player");
        if (Lario != null)
        {
            playerFeetCollider = Lario.GetComponent<BoxCollider2D>();
        }
        if (gameObject.name == "MegaGorb")
        {
            currentHealth = maxHealth;
        }
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
        if (!other.CompareTag("Bullet"))
        {
            moveSpeed = -moveSpeed;
            FlipEnemyFacing();
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (gameObject.name == "MegaGorb")
            {
                TakeDamage(1);
            }
            else
            {
                Die();
            }
        }
    }
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
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
        facingDirection *= -1;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;

    }
    void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup);
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
