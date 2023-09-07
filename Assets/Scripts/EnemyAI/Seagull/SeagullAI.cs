using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SeagullAI : MonoBehaviour
{
    public Transform target;
    public bool inRange;
    public bool seagullFacingLeft;
    public bool startIdle;

    [SerializeField] private float speed;
    [SerializeField] private float idleSpeed;
    [SerializeField] private float idleRange;
    [SerializeField] private float nextWaypointDistance;

    Path path;
    int currentWaypoint = 0;
    private float initialIdleX;
    //bool reachedEndOfPath = false;  

    Seeker seeker;
    Rigidbody2D RB2D;

    void Start() {
        seeker = GetComponent<Seeker>();
        RB2D = GetComponent<Rigidbody2D>();
        inRange = false;
        seagullFacingLeft = true;
        startIdle = true;
        initialIdleX = transform.position.x;

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath() {
        if (seeker.IsDone()) {
            seeker.StartPath(RB2D.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate() {
        if (path == null) {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count) {
            //reachedEndOfPath = true;
            return;
        } else {
            //reachedEndOfPath = false;
        }
        
        Vector3 direction = ((Vector2)path.vectorPath[currentWaypoint] - RB2D.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        
        if (inRange) {
            RB2D.AddForce(force);
        } else if (!startIdle) {
            if (seagullFacingLeft) {
                if (transform.position.x - initialIdleX <= -idleRange) {
                    FlipRight();
                }
                RB2D.velocity = new Vector2(-idleSpeed, 0f);
                } else {
                if (transform.position.x - initialIdleX >= idleRange) {
                    FlipLeft();
                }
                RB2D.velocity = new Vector2(idleSpeed, 0f);
            }
        }

        float distance = Vector2.Distance(RB2D.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint ++;
        }

       if (RB2D.velocity.x >= 0.01f && force.x > 0f) {
            FlipRight();
        } else if (RB2D.velocity.x <= -0.01 && force.x < 0f) {
            FlipLeft();
        }
    }

    public void FlipLeft() {
        transform.localScale = new Vector3(1f, 1f, 1f);
        seagullFacingLeft = true;
    }

    public void FlipRight() {
        transform.localScale = new Vector3(-1f, 1f, 1f);
        seagullFacingLeft = false;
    }

    public void UpddateIdleStart() {
        initialIdleX = transform.position.x;
    }
}
