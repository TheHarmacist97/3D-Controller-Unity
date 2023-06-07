using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float drag;
    [SerializeField] private Transform camTransform;

    #region Planar Movement
    private float horizontalMovement, verticalMovement;
    private Vector3 moveDirection;
    #endregion

    #region Mouse Movement
    private float mouseX, mouseY;
    [SerializeField] private float sensX = 1f, sensY = 1f;
    private float xRotation, yRotation;
    #endregion

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        rb.freezeRotation = true;
    }

    private void Update()
    {
        GetInput();
        ControlDrag();
    }

    private void LateUpdate()
    {
        camTransform.localRotation = Quaternion.Euler(xRotation,0, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void ControlDrag()
    {
        rb.drag = drag;
    }

    private void GetInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;

        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
        yRotation += mouseX * sensX;
        xRotation -= mouseY * sensY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Acceleration);
    }
}
