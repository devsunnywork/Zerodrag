using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimplePlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 7f;
    public float rotationSpeed = 10f;

    [Header("Gravity & Jump")]
    public float gravity = -9.81f;
    public float gravityMultiplier = 3f;
    public float jumpHeight = 1.5f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Camera Control")]
    public Transform camTransform;
    public Vector3 camOffset = new Vector3(0, 2.5f, -4f);
    public float mouseSensitivity = 3f;
    public float camSmoothness = 12f;
    public float minY = -30f;
    public float maxY = 60f;

    [Header("References")]
    public Animator anim;
    
    private CharacterController controller;
    private Vector3 moveVelocity;
    private Vector3 verticalVelocity;
    private bool isGrounded;
    private float pitch = 0f;
    private float yaw = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (anim == null) anim = GetComponentInChildren<Animator>();
        if (camTransform == null) camTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = transform.eulerAngles.y;
    }

    void Update()
    {
        HandleGravityAndJump();
        HandleMovement();
    }

    void LateUpdate()
    {
        HandleCameraControl();
    }

    void HandleCameraControl()
    {
        if (camTransform == null) return;

        // Mouse rotation
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        // Rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 targetPos = transform.position + (rotation * camOffset);

        // Reverting to Lerp as requested (even if it has jitter)
        camTransform.position = Vector3.Lerp(camTransform.position, targetPos, Time.deltaTime * camSmoothness);
        camTransform.LookAt(transform.position + Vector3.up * 1.5f);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, verticalInput).normalized;

        bool isRunningInput = Input.GetKey(KeyCode.LeftShift);
        moveVelocity = Vector3.zero;

        if (direction.magnitude >= 0.1f)
        {
            // Calculate target rotation based on camera orientation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            
            // Rotate the entire player object
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            float speed = isRunningInput ? runSpeed : walkSpeed;
            moveVelocity = targetRotation * Vector3.forward * speed;
        }

        // Apply movement
        controller.Move((moveVelocity + verticalVelocity) * Time.deltaTime);

        // Update Animator
        if (anim != null)
        {
            float moveMag = direction.magnitude;
            anim.SetBool("isWalking", moveMag >= 0.1f && !isRunningInput);
            anim.SetBool("isRunning", moveMag >= 0.1f && isRunningInput);
            anim.SetBool("isGrounded", isGrounded);
            
            if (isGrounded && verticalVelocity.y <= 0)
            {
                anim.SetBool("isJumping", false);
            }
        }
    }

    void HandleGravityAndJump()
    {
        if (groundCheck != null)
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        else
            isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity * gravityMultiplier);
            if (anim != null) anim.SetBool("isJumping", true);
        }

        verticalVelocity.y += gravity * gravityMultiplier * Time.deltaTime;
    }
}
