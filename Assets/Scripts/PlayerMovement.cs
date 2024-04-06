using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IMPORTANT
// Player movement and first person camera movement were learned and inspired from this tutorial:
// https://youtu.be/f473C43s8nE?si=MRKiLRorH072uRfu

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float platformDrag;

    [Header("Platform Check")]
    public float playerHeight;
    public LayerMask isPlatform;
    bool onPlatform;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    private void Update()
    {
        onPlatform = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, isPlatform);

        MyInput();
        SpeedControl();

        // Handle drag
        if(onPlatform) {
            rb.drag = platformDrag;
        } else {
            rb.drag = 0;
        }
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    private void MyInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer() {
        // Calculate moveDirection
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10.0f, ForceMode.Force);
    }

    private void SpeedControl() {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);

        // Limit velocity if necessary
        if(flatVel.magnitude > moveSpeed) {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
