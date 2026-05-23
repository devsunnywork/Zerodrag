using System.Collections;
using UnityEngine;

public class RifleController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 10f;

    [Header("Ammo Settings")]
    public int maxAmmo = 30;
    public int currentAmmo;
    public float reloadTime = 2f;
    private bool isReloading = false;

    [Header("References")]
    public Camera playercamera;
    public AudioSource audioSource;
    public AudioClip fireSound;
    public AudioClip reloadSound;

    private float nextTimeToFire = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void OnEnable()
    {
        isReloading = false;
    }

    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0 || (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        currentAmmo--;

        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }

        RaycastHit hit;
        if (Physics.Raycast(playercamera.transform.position, playercamera.transform.forward, out hit, range))
        {
            NPCController npc = hit.transform.GetComponentInParent<NPCController>();
            if (npc != null)
            {
                npc.StartFleeing();
                npc.TakeDamage(damage);
            }
            else if (hit.collider.CompareTag("Enemy"))
            {
                EnemyController enemy = hit.transform.GetComponentInParent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        
        if (audioSource != null && reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
