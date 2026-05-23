using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public float rotationSmoothTime = 0.12f;
    public float speedSmoothTime = 0.1f;
    public bool canMove = true;
    
    private float turnSmoothVelocity;
    private float speedSmoothVelocity;
    private float currentSpeed;

    [Header("Gravity & Jump")]
    public float gravity = -15f;
    public float jumpHeight = 2f;
    public Transform groundCheck;
    public float groundDistance = 0.3f;
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
    public Transform playerModel; 
    public float modelRotationOffset = 180f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float pitch = 0f;
    private float yaw = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        if (anim == null) 
        {
            anim = GetComponent<Animator>();
            if (anim != null) Debug.Log("<color=green>PlayerMovement: Animator found on main object.</color>");
        }
        
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
            if (anim != null) Debug.Log("<color=green>PlayerMovement: Animator found on child object.</color>");
        }

        if (anim == null) 
        {
            Debug.LogError("<color=red>PlayerMovement: Animator NOT FOUND! Animalions will not play.</color>");
        }
        else
        {
            // Verify parameters exist to catch typos
            CheckAnimatorParameters();
        }

        if (camTransform == null) camTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angles = camTransform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void CheckAnimatorParameters()
    {
        bool hasWalking = false, hasRunning = false, hasGrounded = false;
        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if (param.name == "isWalking") hasWalking = true;
            if (param.name == "isRunning") hasRunning = true;
            if (param.name == "isGrounded") hasGrounded = true;
        }

        if (!hasWalking) Debug.LogWarning("PlayerMovement: Parameter 'isWalking' missing in Animator!");
        if (!hasRunning) Debug.LogWarning("PlayerMovement: Parameter 'isRunning' missing in Animator!");
        if (!hasGrounded) Debug.LogWarning("PlayerMovement: Parameter 'isGrounded' missing in Animator!");
    }

    void Update()
    {
        if (!canMove) 
        {
             // Debug.Log("PlayerMovement: canMove is FALSE, movement blocked.");
             return;
        }

        HandleCameraControl();
        HandleGravityAndJump();
        HandleMovement();
    }

    void HandleCameraControl()
    {
        if (camTransform == null) return;

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 targetPos = transform.position + (rotation * camOffset);

        camTransform.position = Vector3.Lerp(camTransform.position, targetPos, Time.deltaTime * camSmoothness);
        camTransform.LookAt(transform.position + Vector3.up * 1.5f);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(horizontal, 0f, vertical).normalized;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        
        // This is safe even if param doesn't exist (cached logic)
        bool isArmed = false;
        if (anim != null) 
        {
            foreach (var p in anim.parameters) {
                if (p.name == "isArmed") {
                    isArmed = anim.GetBool("isArmed");
                    break;
                }
            }
        }
        
        float targetSpeed = 0;
        if (inputDir.magnitude > 0.1f)
        {
            targetSpeed = isSprinting ? sprintSpeed : walkSpeed;
        }

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        // Rotation logic
        if (isArmed)
        {
            float targetAngle = camTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.05f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        if (inputDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;

            if (!isArmed)
            {
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }

        // Animator parameters
        if (anim != null)
        {
            bool isWalking = inputDir.magnitude > 0.1f && !isSprinting;
            bool isRunning = inputDir.magnitude > 0.1f && isSprinting;
            
            anim.SetBool("isWalking", isWalking);
            anim.SetBool("isRunning", isRunning);
            anim.SetBool("isGrounded", isGrounded);

            // Debug log to see real-time values in console
            if (isWalking || isRunning)
            {
                Debug.Log($"<color=cyan>Anim Debug: isWalking={isWalking}, isRunning={isRunning}, targetSpeed={targetSpeed}</color>");
            }
        }

        // Fix model local rotation
        if (playerModel != null)
        {
            playerModel.localRotation = Quaternion.Euler(0, modelRotationOffset, 0);
        }
    }

    void HandleGravityAndJump()
    {
        isGrounded = controller.isGrounded; 
        
        if (groundCheck != null)
        {
            bool sphereCheck = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            isGrounded = isGrounded || sphereCheck;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if (anim != null) anim.SetTrigger("Jump");
            Debug.Log("<color=yellow>Jump triggered!</color>");
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}


