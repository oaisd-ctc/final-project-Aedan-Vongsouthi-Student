using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    Rigidbody2D bulletRigidBody;
    [SerializeField] float bulletSpeed = 20f;
    Player player;
    float xspeed;

    void Start()
    {
        bulletRigidBody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        xspeed = player.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        bulletRigidBody.velocity = new Vector2(xspeed,0f);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        Destroy(gameObject);
    }
}