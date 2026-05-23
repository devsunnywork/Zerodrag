using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 100f;
    public Transform player;
    public float shootingRange = 20f;
    public float fireRate = 1f;
    public float accuracy = 0.8f; 
    public float damage = 10f;

    public GameObject bulletPrefab;
    public Transform firePoint;

    private float nextFireTime;
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null || health <= 0) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= shootingRange)
        {
            LookAtPlayer();
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 5f * Time.deltaTime);
    }

    void Shoot()
    {
        if (anim != null) anim.SetTrigger("Shoot");

        bool hit = Random.value < accuracy;
        if (hit)
        {
            Playerstats ps = player.GetComponent<Playerstats>();
            if (ps != null)
            {
                ps.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (anim != null) anim.SetTrigger("Die");
        Destroy(gameObject, 2f);
    }
}