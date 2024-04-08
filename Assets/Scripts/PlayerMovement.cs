using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// IMPORTANT
// Player movement and first person camera movement were learned and inspired from this tutorial:
// https://youtu.be/f473C43s8nE?si=MRKiLRorH072uRfu

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float platformDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("WinLose")]
    public GameObject winCanvas;
    public GameObject loseCanvas;
    private bool canMove;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

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
        readyToJump = true;
        canMove = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if(!canMove) {
            return;
        }
        onPlatform = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, isPlatform);

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
        if(!canMove) {
            return;
        }
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Jumping
        if(Input.GetKey(jumpKey) && readyToJump && onPlatform) {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown); // Calls ResetJump() based on the jumpCooldown variable
        }
    }

    private void MovePlayer() {
        if(!canMove) {
            return;
        }
        // Calculate moveDirection
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(onPlatform) { // If on the platforms
            rb.AddForce(moveDirection.normalized * moveSpeed * 10.0f, ForceMode.Force);
        } else if (!onPlatform) { // If in the air, modify speed by airMultiplier
            rb.AddForce(moveDirection.normalized * moveSpeed * 10.0f * airMultiplier, ForceMode.Force);
        }
        
    }

    private void SpeedControl() {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);

        // Limit velocity if necessary
        if(flatVel.magnitude > moveSpeed) {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump() {
        // Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump() {
        readyToJump = true;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Win") {
            canMove = false;
            readyToJump = false;
            winCanvas.SetActive(true);
        }
        if(other.gameObject.tag == "Barrel") {
            canMove = false;
            readyToJump = false;
            loseCanvas.SetActive(true);
        }
    }
}
