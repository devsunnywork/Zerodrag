// PlayerMovement handles WASD movement with KeyCode fallbacks for robustness.
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    public float speed;
    public float mouseSensitivity;
    public float gravity = -9.8f;
    public float jumpHeight = 2f;

    private CharacterController controller;
    private float xRotation;
    private bool isGrounded;
    private Vector3 velocity;

    [SerializeField] Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;


    [SerializeField] Transform playerCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

       controller = GetComponent<CharacterController>(); 
       Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {

      Move();   
      mouseLook();
      handleGravity(); 
        
    }

    void Move()
    {
        float x = 0;
        float z = 0;

        // Using direct KeyCodes to bypass Input Axis errors
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) x = 1;
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) x = -1;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) z = 1;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) z = -1;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }

    void mouseLook()
    {

      float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime ;
      float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

      transform.Rotate(Vector3.up * mouseX);

      xRotation -= mouseY;
      xRotation = Mathf.Clamp(xRotation, -90f, 90f);


      playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f); 


  
    } 

    void handleGravity()
    {
    isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
      

      if(isGrounded && velocity.y < 0)
      {
        velocity.y = -2f;
      } 

      if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space))
      {
          Debug.Log("Jump Pressed! isGrounded: " + isGrounded);
          if (isGrounded)
          {
              velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
          }
      }

      velocity.y += gravity * Time.deltaTime;

      controller.Move(velocity * Time.deltaTime); 
    } 
}

