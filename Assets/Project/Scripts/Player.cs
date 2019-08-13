using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Movement speed in units per second.
    /// </summary>
    [Header("Movement")]
    public float moveSpeed;

    /// <summary>
    /// Force applied upwards when jumping.
    /// </summary>
    public float jumpForce;

    /// <summary>
    /// The distance used for a raycast to determine whether we're on the ground.
    /// </summary>
    public float jumpGroundDetectionDistance;

    /// <summary>
    /// Mouse look sensitivity.
    /// </summary>
    [Header("Camera")]
    public float lookSensitivity;

    /// <summary>
    /// Highest up angle we can have.
    /// </summary>
    public float maxLookX;

    /// <summary>
    /// Lowest up angle we can have.
    /// </summary>
    public float minLookX;

    /// <summary>
    /// Current X rotation of the camera.
    /// </summary>
    private float _rotX;

    /// <summary>
    /// Main camera.
    /// </summary>
    private Camera _cam;

    /// <summary>
    /// The <see cref="Rigidbody"/> component.
    /// </summary>
    private Rigidbody _rig;

    private void Awake()
    {
        _cam = Camera.main;
        _rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        TryJump();
        CamLook();
    }

    private void TryJump()
    {
        if (!Input.GetButtonDown("Jump")) return;

        var ray = new Ray(transform.position, Vector3.down);
        if (!Physics.Raycast(ray, jumpGroundDetectionDistance)) return;

        _rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void Move()
    {
        var x = Input.GetAxis("Horizontal") * moveSpeed;
        var z = Input.GetAxis("Vertical") * moveSpeed;
        _rig.velocity = new Vector3(x, _rig.velocity.y, z);
    }

    void CamLook()
    {
        var y = Input.GetAxis("Mouse X") * lookSensitivity;
        var x = Input.GetAxis("Mouse Y") * lookSensitivity;

        // Y rotation (left/right) is applied to the player (not the camera!), so that the
        // player is always facing towards the player model's forward direction.
        transform.eulerAngles += Vector3.up * y;

        // X rotation (up/down) is applied to the camera.
        // By changing the axis sign to positive, vertical mouse motion is flipped.
        const float axisSign = -1;
        _rotX = Mathf.Clamp(_rotX + x, minLookX, maxLookX);
        _cam.transform.localRotation = Quaternion.Euler(axisSign * _rotX, 0, 0);
    }
}
