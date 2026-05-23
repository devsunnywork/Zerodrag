using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public float pickUpRange = 5f;
    public Camera playerCamera;  
    public Transform weaponHolder; 
    public LayerMask pickUpLayer; 
    private GameObject currentGun;   

    private bool hasGun = false;   

    void Update()
    {
        if (!hasGun && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F)))
        {
            TryPickUp();
        }

        if (hasGun && Input.GetKeyDown(KeyCode.G))
        {
            DropGun();  
        }
    }

    void TryPickUp()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange, pickUpLayer, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.CompareTag("Gun"))
            {
                PickUp(hit.collider.gameObject);
                return;
            }
        }

        Collider[] nearby = Physics.OverlapSphere(transform.position, pickUpRange, pickUpLayer, QueryTriggerInteraction.Collide);
        foreach (Collider col in nearby)
        {
            if (col.CompareTag("Gun"))
            {
                PickUp(col.gameObject);
                return;
            }
        }
    }

    void PickUp(GameObject gun)
    {
        currentGun = gun;
        gun.transform.SetParent(weaponHolder);
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;

        Rigidbody rb = gun.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider col = gun.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        RifleController newScript = gun.GetComponent<RifleController>();
        if (newScript != null) newScript.enabled = true;

        hasGun = true;
    }

    void DropGun()
    {
        if (currentGun == null) return; 

        currentGun.transform.SetParent(null); 

        Rigidbody rb = currentGun.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false; 

        Collider col = currentGun.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        RifleController script = currentGun.GetComponent<RifleController>();
        if (script != null) script.enabled = false;

        currentGun = null;
        hasGun = false;
    }
}