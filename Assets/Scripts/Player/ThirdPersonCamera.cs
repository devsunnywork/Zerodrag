using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -4);
    public float sensitivity = 3f;
    public float smoothness = 10f;

    public float minY = -20f;
    public float maxY = 60f;

    private float pitch = 0f;
    private float yaw = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 targetPos = target.position + (rotation * offset);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothness);
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
