// WeaponPickUp handles detecting and parenting guns to the player's weapon holder.
// Uses raycast + proximity + FindWithTag fallback for maximum reliability.
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public float pickUpRange = 5f;
    public Camera playerCamera;    // Drag your Main Camera here
    public Transform weaponHolder; // Child of Camera where gun will sit
    public LayerMask pickUpLayer;  // Set this to include the layer of your gun

    private bool hasGun = false;   // Track if player already has a gun

    void Update()
    {
        if (hasGun) return;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
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

        Debug.Log("Trying to pick up gun...");

        // Method 1: Raycast (looking at the gun) — includes trigger colliders
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * pickUpRange, Color.red, 2f);

        if (Physics.Raycast(ray, out hit, pickUpRange, pickUpLayer, QueryTriggerInteraction.Collide))
        {
            Debug.Log("Raycast hit: " + hit.collider.name + " | Tag: " + hit.collider.tag);
            if (hit.collider.CompareTag("Gun"))
            {
                PickUp(hit.collider.gameObject);
                return;
            }
        }

        // Method 2: Proximity sphere — includes trigger colliders
        Collider[] nearby = Physics.OverlapSphere(transform.position, pickUpRange, pickUpLayer, QueryTriggerInteraction.Collide);
        Debug.Log("OverlapSphere found " + nearby.Length + " colliders nearby");
        foreach (Collider col in nearby)
        {
            Debug.Log("Nearby object: " + col.name + " | Tag: " + col.tag);
            if (col.CompareTag("Gun"))
            {
                PickUp(col.gameObject);
                return;
            }
        }

        // Method 3: Last resort — find any Gun-tagged object within range
        GameObject[] guns = GameObject.FindGameObjectsWithTag("Gun");
        foreach (GameObject g in guns)
        {
            float dist = Vector3.Distance(transform.position, g.transform.position);
            Debug.Log("FindWithTag found: " + g.name + " at distance " + dist);
            if (dist <= pickUpRange)
            {
                PickUp(g);
                return;
            }
        }

        Debug.LogWarning("No gun found to pick up!");
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

        hasGun = true;
        Debug.Log("SUCCESS! Picked up: " + gun.name);
    }
}
