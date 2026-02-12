using UnityEngine;

public class GunShot : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField] AudioSource gunSound;
    public float bulletRange = 100f;
    public float damage = 25f;
    public GameObject hitEffect;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        gunSound.PlayOneShot(gunSound.clip);  
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, bulletRange))
        {
            GameObject impactGO = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 0.5f);
            
            //Debug.Log("Hit: " + hit.collider.gameObject.name);

            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}