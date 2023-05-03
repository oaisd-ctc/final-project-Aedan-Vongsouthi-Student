using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    Player player;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] public float offset;
    [SerializeField] public int amountOfBullets;
    [SerializeField] public float spread, bulletspeed;
    
    void Awake() 
    {
        Transform parent = gameObject.transform.parent;
        player = parent.gameObject.GetComponent<Player>();
    }

    private void Update() 
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
    }

    void OnFire(InputValue value)
    {
        if(player.isAlive)
        {
            Shoot();
        } else {
            return;
        }
    }

    void Shoot()
    {
        for (int i = 0; i < amountOfBullets; i++)
            {
               GameObject b = Instantiate(bullet, gun.position, transform.rotation);
               Rigidbody2D brb = b.GetComponent<Rigidbody2D>();
               Vector2 dir = transform.rotation * Vector2.up;
               Vector2 pdir = Vector2.Perpendicular(dir) * Random.Range(-spread, spread);
               brb.velocity = (dir * pdir) * bulletspeed;
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
