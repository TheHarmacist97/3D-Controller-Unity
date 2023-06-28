using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collider coll;
    [SerializeField] private Rigidbody rb;
    [SerializeField, Range(1, 100)] private float moveSpeed = 6f, drag;
    [SerializeField] private Transform camTransform;

    #region Planar Movement
    private float horizontalMovement, verticalMovement;
    private Vector3 moveDirection;
    #endregion

    #region Mouse Movement
    [SerializeField, Range(0.01f, 3f)] private float sensX = 1f, sensY = 1f;
    private float mouseX, mouseY;
    private float xRotation, yRotation;
    #endregion

    #region Jump
    [SerializeField, Range(1, 5)] private int jumps;
    [SerializeField, Range(1, 100)] private float jumpForce;
    [SerializeField] private int currentJumps;
    [SerializeField] private bool isGrounded;
    private float halfHeight;
    #endregion

    Ray groundCheckRay;

    public bool IsGrounded
    {
        get => isGrounded;
        set
        {
            if(value != isGrounded)
            {
                if (!isGrounded)
                {
                    currentJumps = 0;
                }
                else
                {
                    if (currentJumps == 0)
                    {
                        currentJumps++;
                    }
                }
            }
            isGrounded = value;
        }
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        halfHeight = coll.bounds.extents.y;
    }

    private void Start()
    {
        rb.freezeRotation = true;
    }

    private void Update()
    {
        GetInput();
        ControlDrag();
        var characterTransform = transform;
        groundCheckRay = new Ray(characterTransform.position, -characterTransform.up);
        IsGrounded = Physics.Raycast(groundCheckRay, halfHeight + 0.01f);
    }

    private void LateUpdate()
    {
        camTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void ControlDrag()
    {
        rb.drag = drag;
    }

    private void GetInput()
    {
        //Planar movement
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        var characterTransform = transform;
        moveDirection = characterTransform.forward * verticalMovement + characterTransform.right * horizontalMovement;

        //Mouse look
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
        yRotation += mouseX * sensX;
        xRotation -= mouseY * sensY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Jump
        if (!Input.GetButtonDown("Jump")) return;
        
        if (currentJumps >= jumps) return;
        
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        currentJumps++;

    }

    private void FixedUpdate()
    {
        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Acceleration);

    }
}
