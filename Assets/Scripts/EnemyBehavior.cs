using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class EnemyBehavior : BehaviorTree.Tree
{
    [Header("Transforms")]
    public Transform[] waypoints;
    public Transform player;

    [Header("Enemy Stats")]
    public float speed = 4.0f;
    public float detectionRange = 15.0f;
    public float idleWaitTime = 1.0f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node> // Chase sequence
            {
                new CheckPlayerInRange(player, transform, detectionRange),
                new TaskMoveToTarget(transform, player, speed)
            }),
            new TaskPatrol(transform, waypoints, speed, idleWaitTime) // Patrol behavior
        });

        return root;
    }
}

public class CheckPlayerInRange : Node
{
    private Transform player;
    private Transform enemy;
    private Animator animator;
    private float range;

    public CheckPlayerInRange(Transform player, Transform enemy, float range)
    {
        this.player = player;
        this.enemy = enemy;
        animator = enemy.GetComponent<Animator>();
        this.range = range;
    }

    public override NodeState Evaluate()
    {
        float dist = Vector3.Distance(player.position, enemy.position);
        if (dist <= range)
        {
            state = NodeState.SUCCESS;
            if(enemy.tag == "Enemy") {
                animator.SetBool("walk", true);
            } else {
                animator.SetBool("run", true);
            }
            
            Debug.Log("Player in range, start chasing.");
        }
        else
        {
            state = NodeState.FAILURE;
        }
        return state;
    }
}

public class TaskMoveToTarget : Node
{
    private Transform enemy;
    private Animator animator;
    private Transform target;
    private float speed;

    public TaskMoveToTarget(Transform enemy, Transform target, float speed)
    {
        this.enemy = enemy;
        animator = enemy.GetComponent<Animator>();
        this.target = target;
        this.speed = speed;
    }

    public override NodeState Evaluate()
    {
        enemy.position = Vector3.MoveTowards(enemy.position, target.position, speed * Time.deltaTime);

        RotateEnemy rotateScript = enemy.GetComponent<RotateEnemy>();
        rotateScript.RotateTowards(enemy, target.position);

        if (Vector3.Distance(enemy.position, target.position) > 0.1f)
        {
            state = NodeState.RUNNING;
            if(enemy.tag == "Enemy") {
                animator.SetBool("walk", true);
            } else {
                animator.SetBool("run", true);
            }
        }
        else
        {
            state = NodeState.SUCCESS;
            if(enemy.tag == "Enemy") {
                animator.SetBool("walk", false);
            } else {
                animator.SetBool("run", false);
            }
            Debug.Log("Reached the player.");
        }
        return state;
    }
}

public class TaskPatrol : Node
{
    private Transform enemy;
    private Animator animator;
    private Transform[] waypoints;
    private float speed;
    private int currentIndex = 0;
    private float waitTime;
    private float waitCounter = 0;
    private bool waiting = false;

    public TaskPatrol(Transform enemy, Transform[] waypoints, float speed, float waitTime)
    {
        this.enemy = enemy;
        animator = enemy.GetComponent<Animator>();
        this.waypoints = waypoints;
        this.speed = speed;
        this.waitTime = waitTime;
    }

    public override NodeState Evaluate()
    {
        if (waiting)
        {
            waitCounter += Time.deltaTime;
            if (waitCounter >= waitTime)
            {
                waiting = false;
                waitCounter = 0;
            }
            else
            {
                state = NodeState.RUNNING;
                animator.SetBool("walk", false);
                return state;
            }
        }

        Transform targetWaypoint = waypoints[currentIndex];
        enemy.position = Vector3.MoveTowards(enemy.position, targetWaypoint.position, speed * Time.deltaTime);
        RotateEnemy rotateScript = enemy.GetComponent<RotateEnemy>();
        rotateScript.RotateTowards(enemy, targetWaypoint.position);
        animator.SetBool("walk", true);

        if (Vector3.Distance(enemy.position, targetWaypoint.position) < 0.1f)
        {
            if (!waiting)
            {
                waiting = true;
                animator.SetBool("walk", false);
                rotateScript.RotateTowards(enemy, targetWaypoint.position);

                currentIndex = (currentIndex + 1) % waypoints.Length;
                Debug.Log("Switching to the next waypoint.");
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}

