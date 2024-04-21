using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class MonkeyBehavior : BehaviorTree.Tree
{
    [Header("Transforms")]
    public Transform[] waypoints;
    public Transform player;

    [Header("Monkey Stats")]
    public float speed = 10.0f;
    public float detectionRange = 15.0f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckPlayerInRange(player, transform, detectionRange),
                new TaskMoveToTarget(transform, player, speed)
            }),
        });

        return root;
    }
}
