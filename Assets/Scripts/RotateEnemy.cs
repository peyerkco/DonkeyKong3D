using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEnemy : MonoBehaviour
{
    [Header("Rotate Stats")]
    public float rotateSpeed = 180; // Degrees per second

    void Start() {
    }

    void Update()
    {
    }

    public void RotateTowards(Transform enemy, Vector3 targetPosition) {
        Vector3 directionToTarget = targetPosition - enemy.position;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);
        enemy.rotation = Quaternion.Lerp(enemy.rotation, rotationToTarget, rotateSpeed * Time.deltaTime);
    }
}