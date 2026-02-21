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
        if (audioSource != null && fireSound != null) audioSource.PlayOneShot(fireSound);

        Debug.DrawRay(playercamera.transform.position, playercamera.transform.forward * range, Color.red, 1f);

        // Sabse badhiya tareeka: Ignore EVERYTHING on Player layer + Tag
        int layerMask = ~LayerMask.GetMask("Player", "Ignore Raycast");

        RaycastHit[] hits = Physics.SphereCastAll(playercamera.transform.position, 0.2f, playercamera.transform.forward, range, layerMask, QueryTriggerInteraction.Ignore);
        
        if (hits.Length > 0)
        {
            System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

            foreach (var h in hits)
            {
                // FIX: Apne haath ya gun se takrane se bachne ke liye 1 meter ka gap rakhein
                if (h.distance < 1.0f) continue; 
                if (h.collider.CompareTag("Player")) continue;

                // NPC Check
                NPCController npc = h.transform.GetComponentInParent<NPCController>();
                if (npc != null) 
                {
                    npc.StartFleeing();
                    npc.TakeDamage(damage);
                    Debug.Log("🎯 Target Hit: NPC " + h.transform.root.name);
                    break; 
                }
                
                // Enemy Check
                EnemyController enemy = h.transform.GetComponentInParent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Debug.Log("🎯 Target Hit: Enemy!");
                    break;
                }

                // Stop at solid objects without flooding logs
                if (h.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
                {
                    break; 
                }
            }
        }
    }
    IEnumerator Reload()
    {
    
        isReloading = true;
        Debug.Log("Reloading...");

        if (audioSource != null && reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reload Complete!");
    }
}
