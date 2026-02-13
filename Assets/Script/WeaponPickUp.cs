using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public float pickUpRange = 3f;
    public Camera playerCamera;    // Drag your Main Camera here
    public Transform weaponHolder; // Child of Camera where gun will sit
    public LayerMask pickUpLayer;  // Set this to include the layer of your gun

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickUp();
        }
    }

    void TryPickUp()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera not assigned on WeaponPickUp script!");
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Visualizing the ray in the Scene view
        Debug.DrawRay(ray.origin, ray.direction * pickUpRange, Color.red, 2f);

        // Using pickUpLayer so the ray passes THROUGH the player and only hits the gun/world
        if (Physics.Raycast(ray, out hit, pickUpRange, pickUpLayer))
        {
            Debug.Log("Raycast hit: " + hit.collider.name + " | Tag: " + hit.collider.tag);

            if (hit.collider.CompareTag("Gun"))
            {
                PickUp(hit.collider.gameObject);
            }
        }
    }

    void PickUp(GameObject gun)
    {
        // 1. Move gun to weapon holder
        gun.transform.SetParent(weaponHolder);
        
        // 2. Reset position and rotation to fit the holder
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;

        // 3. Disable Physics so it doesn't fall or bump into player
        Rigidbody rb = gun.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider col = gun.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // 4. Enable Shooting script
        GunShot script = gun.GetComponent<GunShot>();
        if (script != null) script.enabled = true;

        Debug.Log("Picked up: " + gun.name);
    }
}
