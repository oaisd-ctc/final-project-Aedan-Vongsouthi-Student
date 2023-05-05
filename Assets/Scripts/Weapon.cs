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
    
    public void Awake() 
    {
        Transform parent = gameObject.transform.parent;
        player = parent.gameObject.GetComponent<Player>();

    }

    public void Update() 
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }


    public void Shoot()
    {
        
        for (int i = 0; i < amountOfBullets; i++)
            {
                float ySpread = 2f;
                Vector3 bulletSpread = new Vector3(0f, ySpread , 0f);
                Instantiate(bullet, gun.position + bulletSpread, transform.rotation);
                ySpread += 2f;
               
            } 
    }
}
