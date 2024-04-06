using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IMPORTANT
// Player movement and first person camera movement were learned and inspired from this tutorial:
// https://youtu.be/f473C43s8nE?si=MRKiLRorH072uRfu

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    // Update is called once per frame
    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
