// GunShot handles ammo, shooting logic, and reload coroutines.
using System.Collections;
using UnityEngine;
using TMPro;

public class GunShot : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField] AudioSource gunSound;
    public float bulletRange = 100f;
    public float damage = 25f;
    public GameObject hitEffect;
    public float maxAmmo = 10f;
    public float reloadTime = 2f;
    [SerializeField] AudioClip reloadSound;
    [SerializeField] TextMeshProUGUI ammoText;
    private float currentAmmo;
    private bool isReloading = false;


    void Start()
    {
        currentAmmo = maxAmmo;
        if (this.enabled) UpdateAmmoUI();
        else if (ammoText != null) ammoText.text = ""; // Hide if not held
    }   

    void OnEnable()
    {
        UpdateAmmoUI();
    }

    void OnDisable()
    {
        if (ammoText != null) ammoText.text = ""; // Hide when dropped/disabled
    }

    void Update()

    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1") || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {

        currentAmmo--;
        UpdateAmmoUI();
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
     
    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        if (ammoText != null)
            ammoText.text = "Reloading...";

        if (reloadSound != null)
            gunSound.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoUI();
        Debug.Log("Reload Complete!");
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = "Ammo: " + currentAmmo + " / " + maxAmmo;
    }
}