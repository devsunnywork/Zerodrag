using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 100f;

    void Start()
    {
        
    }

    void Update()
    {
        
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
        Debug.Log(gameObject.name + " has died.");
        Destroy(gameObject);
    }
}