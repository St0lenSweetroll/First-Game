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
    public float destroyDelay = 2f;


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
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {
            myParticleSystem.Stop();
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.simulated = false;
            transform.parent = collision.transform;
            StartCoroutine(DestroyWithDelay(collision.gameObject));
        }
    }
        void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazards") || other.CompareTag("Mushroom"))
        {
            Destroy(gameObject);
        }
    }
    IEnumerator DestroyWithDelay(GameObject enemy)
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(enemy);
        Destroy(gameObject);
    }
}
