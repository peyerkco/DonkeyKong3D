using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelMovement : MonoBehaviour
{
    public Transform barrel;
    public float rollSpeed;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
    }


    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Right") {
            rb.AddForce(barrel.forward * rollSpeed, ForceMode.Impulse);
            Debug.Log("Right");
        }
        if(other.gameObject.tag == "Left") {
            rb.AddForce(-barrel.forward * rollSpeed, ForceMode.Impulse);
            Debug.Log("Left");
        }
        if(other.gameObject.tag == "End") {
            KillBarrel();
        }
    }

    private void KillBarrel() {
        Destroy(gameObject);
    }
}
