using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    Player player;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] public int amountOfBullets;
    [SerializeField] public float spreadAngle;
    List<Quaternion> bullets;
    [SerializeField] public float bulletSpeed = 2;
    [SerializeField] public float offset;
    public void Awake() 
    {
        Transform parent = gameObject.transform.parent;
        player = parent.gameObject.GetComponent<Player>();

        bullets = new List<Quaternion>(amountOfBullets);
        for (int i = 0; i < amountOfBullets; i++)
        {
            bullets.Add(Quaternion.Euler(Vector3.zero));
        }
    }

    public void Update() 
    {
        if (player.isAlive)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
    }


    public void Shoot()
    {
        int i = 0;
        foreach (Quaternion quat in bullets.ToList())
        {
            bullets[i] = Random.rotation;
            GameObject b = Instantiate(bullet, gun.position, bullets[i]);
            b.transform.rotation = Quaternion.RotateTowards(b.transform.rotation, bullets[i], spreadAngle);
            b.GetComponent<Rigidbody2D>().AddForce(b.transform.right * bulletSpeed);
            i++;
        }
    }
}
