using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    Rigidbody2D myRigidbody;
    ParticleSystem myParticleSystem;
    PlayerMovement player;
    float xSpeed;
    Rigidbody2D rigidbodyofGoober;
    Animator animatorofGoober;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        GameObject otherObject = GameObject.FindGameObjectWithTag("Enemy");
        rigidbodyofGoober = otherObject.GetComponent<Rigidbody2D>();
        animatorofGoober = otherObject.GetComponent<Animator>();
        myParticleSystem = GetComponent<ParticleSystem>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
        transform.localScale = new Vector2((Mathf.Sign(xSpeed)) * transform.localScale.x, -(Mathf.Sign(xSpeed)) * -(transform.localScale.y));
    }
    void Update()
    {
        myRigidbody.velocity = new Vector2(xSpeed, 0f);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        myParticleSystem.Stop();
        Invoke("DestroySelf", 2f);
    }
    void DestroySelf()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            rigidbodyofGoober.freezeRotation = false;
            animatorofGoober.enabled = false;
            Destroy(other.gameObject, 2f);
            Destroy(gameObject);
        }
        if (other.tag == ("Hazards"))
        {
            Destroy(gameObject);
        }
        if (other.tag == ("Mushroom"))
        {
            Destroy(gameObject);
        }
    }
}
