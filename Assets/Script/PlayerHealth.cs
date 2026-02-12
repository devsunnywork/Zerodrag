using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxhealth = 100;
    private int currenthealth;
    public Slider healthSlider;
    public GameOverManager gameOverManager;

    void Start()
    {
        currenthealth = maxhealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxhealth;
            healthSlider.value = currenthealth;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
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
        if (gameOverManager != null)
        {
            gameOverManager.ShowDeathScreen();
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
