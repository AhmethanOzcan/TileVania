using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    PlayerMovement player;
    float xWay;
    [SerializeField] float bulletSpeed = 1f;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        myRigidbody = GetComponent<Rigidbody2D>();
        xWay = player.transform.localScale.x;
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2 (bulletSpeed*xWay, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemies")
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }
}
