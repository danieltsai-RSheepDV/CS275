using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float acceleration = 3f;
    [SerializeField] float jumpThrust = 5f;
    [SerializeField] float groundDrag = 2f;
    bool isGrounded = false;

    [SerializeField] Transform tongueTip;
    [SerializeField] Transform tongueBody;
    [SerializeField] float tongueSpeed = 5f;
    [SerializeField] float maxTongueLength = 4f;
    bool tongueReady = true;

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
        /*
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
        */

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpThrust, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            LaunchTongue(Vector3.forward);
        }
    }

    void LaunchTongue(Vector3 direction)
    {
        if (tongueReady)
        {
            Vector3 dest = transform.position + direction * maxTongueLength;
            StartCoroutine(ExtendTongue(dest));
        }
    }

    IEnumerator ExtendTongue(Vector3 destination)
    {
        tongueReady = false;
        // when extending, tongue should always start from the player
        tongueTip.position = transform.position;

        while (Vector3.Distance(tongueTip.position, destination) > 0.2f)
        {
            tongueTip.position = Vector3.MoveTowards(tongueTip.position, destination, Time.fixedDeltaTime * tongueSpeed);

            tongueBody.position = transform.position + (tongueTip.position - transform.position) * 0.5f;
            tongueBody.localScale = new Vector3(1f, 1f, Vector3.Distance(tongueTip.position, transform.position) * 0.5f);
            tongueBody.forward = (tongueTip.position - transform.position).normalized;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(RetractTongue());
    }

    IEnumerator RetractTongue()
    {
        /*
         * - Tongue position is halfway between tip and player
         * - Tongue scale (z) is half the distance between tip and player
         * - Tongue forward oriented with distance vector from player to tip 
         */
        while (Vector3.Distance(tongueTip.position, transform.position) > 0.2f)
        {
            tongueTip.position = Vector3.MoveTowards(tongueTip.position, transform.position, Time.fixedDeltaTime * tongueSpeed);

            tongueBody.position = transform.position + (tongueTip.position - transform.position) * 0.5f;
            tongueBody.localScale = new Vector3(1f, 1f, Vector3.Distance(tongueTip.position, transform.position) * 0.5f);
            tongueBody.forward = (tongueTip.position - transform.position).normalized;
            yield return new WaitForFixedUpdate();
        }
        tongueTip.GetComponent<Tongue>().ClearCaught();  // This returns number of entities caught
        tongueReady = true;
    }

    public void AccelerateGroundPlayer(Vector3 direc)
    {
        if (isGrounded)
            rb.AddForce(direc * acceleration);
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
