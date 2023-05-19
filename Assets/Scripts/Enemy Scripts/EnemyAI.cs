using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class EnemyAI : MonoBehaviour
{
    public Player player;
    public int enemyDamage;
    public int enemyKnockback;
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public Transform enemyGFX;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    public Rigidbody2D enemyRB;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        enemyRB = GetComponent<Rigidbody2D>();
        
        InvokeRepeating("UpdatePath", 0f, .5f);
        seeker.StartPath(enemyRB.position, target.position, OnPathComplete);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(enemyRB.position, target.position, OnPathComplete);
        }
        
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        } else {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - enemyRB.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        enemyRB.AddForce(force);

        float distance = Vector2.Distance(enemyRB.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (enemyRB.velocity.x >= Mathf.Epsilon)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (enemyRB.velocity.x <= -Mathf.Epsilon) {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            player.PlayerTakeDamage(enemyDamage);
            Debug.Log(GameManager.gameManager.playerHealth.Health);
        }
    }
}
