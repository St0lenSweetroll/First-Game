using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    AudioSource myAudioSource;
    CircleCollider2D myCircleCollider;
    Rigidbody2D myRigidBody;
    [SerializeField] float pickupDelay = 0.1f;
    [SerializeField] Vector2 force = new Vector2(0f, 10f);
    [SerializeField] int pointsForCoinPickup = 100;
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        myCircleCollider = GetComponent<CircleCollider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup);
            myAudioSource.Play();
            myCircleCollider.enabled = false;
            myRigidBody.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            myRigidBody.AddForce(force, ForceMode2D.Impulse);
            StartCoroutine(Destroy());
        }
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSecondsRealtime(pickupDelay);
        Destroy(gameObject);
    }

}
