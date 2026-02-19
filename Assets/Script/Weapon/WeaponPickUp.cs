using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public float pickUpRange = 5f;
    public Camera playerCamera;    
    public Transform weaponHolder; 
    public LayerMask pickUpLayer;  

    private bool hasGun = false;   

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

        GameObject[] guns = GameObject.FindGameObjectsWithTag("Gun");
        foreach (GameObject g in guns)
        {
            float dist = Vector3.Distance(transform.position, g.transform.position);
            if (dist <= pickUpRange)
            {
                PickUp(g);
                return;
            }
        }
    }

    void PickUp(GameObject gun)
    {
        gun.transform.SetParent(weaponHolder);
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;

        Rigidbody rb = gun.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider col = gun.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        GunShot script = gun.GetComponent<GunShot>();
        if (script != null) script.enabled = true;

        hasGun = true;
    }
}
