using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float acceleration = 3f;
    [SerializeField] float jumpThrust = 5f;
    [SerializeField] float groundDrag = 2f;
    bool isGrounded = false;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.W) && isGrounded)
        {
            rb.AddForce(Vector3.forward * acceleration);
        }

        if (Input.GetKey(KeyCode.S) && isGrounded)
        {
            rb.AddForce(Vector3.back * acceleration);
        }

        if (Input.GetKey(KeyCode.A) && isGrounded)
        {
            rb.AddForce(Vector3.left * acceleration);
        }

        if (Input.GetKey(KeyCode.D) && isGrounded)
        {
            rb.AddForce(Vector3.right * acceleration);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpThrust, ForceMode.Impulse);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Vector3.Distance(collision.GetContact(0).normal, Vector3.up) < 1f)
        {
            if (!isGrounded)
            {
                isGrounded = true;
                rb.drag = groundDrag;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        rb.drag = 0;
    }
}
