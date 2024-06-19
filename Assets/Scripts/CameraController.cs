using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform trackingTarget;
    [SerializeField] Vector3 trackingOffset = Vector3.up;
    // [SerializeField] float trackingDistance;

    PlayerController trackedPlayer;

    // Start is called before the first frame update
    void Start()
    {
        trackedPlayer = trackingTarget.GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        MoveCamera();
        MovePlayer();
    }

    void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, trackingTarget.position + trackingOffset, 0.4f);
        // To be done 
    }

    void MovePlayer()
    {
        float ix = Input.GetAxis("Horizontal");
        float iy = Input.GetAxis("Vertical");

        Vector3 direction = Vector3.ProjectOnPlane(ix * transform.right + iy * transform.forward, Vector3.up).normalized;

        trackedPlayer.AccelerateGroundPlayer(direction);
    }
}
