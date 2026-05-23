using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private Animator anim;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip reloadSound;

    void Awake()
    {
        anim = GetComponent<Animator>();
        this.enabled = false; 
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    void Shoot()
    {
        if (anim != null) anim.SetTrigger("Shoot");
        if (muzzleFlash != null) muzzleFlash.Play();
        if (audioSource != null && shootSound != null) audioSource.PlayOneShot(shootSound);
    }

    void Reload()
    {
        if (anim != null) anim.SetTrigger("Reload");
        if (audioSource != null && reloadSound != null) audioSource.PlayOneShot(reloadSound);
    }
}
