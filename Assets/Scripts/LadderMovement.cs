using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    public Transform player;
    public float speedUpDown;
    public PlayerMovement FPSControl;
    bool onLadder;

    [Header("Keybinds")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;

    // Start is called before the first frame update
    void Start()
    {
        onLadder = false;
        FPSControl = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(onLadder && Input.GetKey(upKey)) {
            player.transform.position += Vector3.up / speedUpDown;
        } 
        if(onLadder && Input.GetKey(downKey)) {
            player.transform.position += Vector3.down / speedUpDown;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Ladder") {
            onLadder = !onLadder;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Ladder") {
            onLadder = !onLadder;
        }
    }
}
