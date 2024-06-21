using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform trackingTarget;
    [SerializeField] Vector3 trackingOffset = Vector3.up;
    // [SerializeField] float trackingDistance;
    [SerializeField] float mouseSensitivity = 100f;

    PlayerController trackedPlayer;

    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        trackedPlayer = trackingTarget.GetComponent<PlayerController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        MoveCamera();
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            trackedPlayer.LaunchTongue(transform.forward);
        }
    }

    void MoveCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, transform.rotation.eulerAngles.y, 0f);
        transform.Rotate(Vector3.up * mouseX);

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
