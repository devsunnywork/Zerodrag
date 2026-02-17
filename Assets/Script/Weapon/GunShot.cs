// GunShot handles ammo, shooting logic, and reload coroutines.
// NOTE: This script should be DISABLED by default on the gun prefab.
// WeaponPickUp.PickUp() enables it when the player picks up the gun.
using System.Collections;
using UnityEngine;
using TMPro;

public class GunShot : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField] AudioSource gunSound;
    public float bulletRange = 100f;
    public float damage = 25f;
    public ParticleSystem muzzleFlash; // Drag your muzzle flash ParticleSystem here
    public float maxAmmo = 10f;
    public float reloadTime = 2f;
    [SerializeField] AudioClip reloadSound;
    [SerializeField] TextMeshProUGUI ammoText;
    private float currentAmmo;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void OnEnable()
    {
        UpdateAmmoUI();
    }

    void OnDisable()
    {
        if (ammoText != null) ammoText.text = "";
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

        // Muzzle flash
        if (muzzleFlash != null) muzzleFlash.Play();

        gunSound.PlayOneShot(gunSound.clip);
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, bulletRange))
        {
            // Enemy ko damage do
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // NPC ko maara toh bhaag jayega
            NPCController npc = hit.collider.GetComponent<NPCController>();
            if (npc != null)
            {
                npc.StartFleeing();
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        if (ammoText != null) ammoText.text = "Reloading...";

        if (reloadSound != null)
            gunSound.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoUI();
    }
    void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo + " / " + maxAmmo;
    }
}