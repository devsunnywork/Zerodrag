using UnityEngine;

public class GunShot : MonoBehaviour
{

    [SerializeField] Transform playerCamera;
    public float bulletRange = 100f;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, bulletRange))
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);
        }
    }
}