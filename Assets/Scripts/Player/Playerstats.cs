using UnityEngine;
using TMPro;

public class Playerstats : MonoBehaviour
{
    public float money = 0;
    public float rating = 0;
    public float health = 100f;
    public float maxHealth = 100f;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI ratingText;

    void Start()
    {
        health = maxHealth;
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
    }

    public void AddRating(int amount)
    {
        rating += amount;
        UpdateUI();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        if (moneyText != null) moneyText.text = "Money: " + money;
        if (ratingText != null) ratingText.text = "Rating: " + rating;
        if (healthText != null) healthText.text = "Health: " + health.ToString("F0");
    }
}
