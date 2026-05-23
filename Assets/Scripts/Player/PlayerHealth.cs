using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxhealth = 100;
    private int currenthealth;
    public Slider healthSlider;
    public Transform player;

    void Start()
    {
        currenthealth = maxhealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxhealth;
            healthSlider.value = currenthealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currenthealth -= damage;
        if (healthSlider != null)
        {
            healthSlider.value = currenthealth;
        }

        if (currenthealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (player != null)
        {
           player.position = Vector3.zero;
        }
    }

    public void Respawn()
    {
        currenthealth = maxhealth;
        if (healthSlider != null)
        {
            healthSlider.value = currenthealth;
        }
    }
}
