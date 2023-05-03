using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    Player player;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    public int amountOfBullets;
    public float spread, bulletspeed;
    
    void Awake() 
    {
        player = GetComponent<Player>();    
    }

    void OnFire(InputValue value)
    {
        if(player.isAlive)
        {
            Instantiate(bullet, gun.position, transform.rotation);
        } else {
            return;
        }
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
