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
    [SerializeField] public float spread;
    
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
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }


    void Shoot()
    {
        for (int i = 0; i < amountOfBullets; i++)
            {
              
               Instantiate(bullet, gun.position, new Quaternion (5f, 5f, 0f, 10f));
               
            } 
    }
}
