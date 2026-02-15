using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    public GameObject brokenVersion;
    public float explosionForce = 500f;
    public float explosionRadius = 5f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        ScoreManager.instance.EnemyKilled();
        GameObject broken = Instantiate(brokenVersion, transform.position, transform.rotation);
        
        Rigidbody[] rbs = broken.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs)
        {
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }
        
        Destroy(gameObject);
    }
}
