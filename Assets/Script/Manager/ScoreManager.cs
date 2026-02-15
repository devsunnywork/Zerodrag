using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    
    public TextMeshProUGUI enemiesLeftText;
    public TextMeshProUGUI killsText;

    private int kills = 0;
    private int enemiesLeft;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;
        UpdateUI();
    }

    public void EnemyKilled()
    {
        kills++;
        enemiesLeft--;
        UpdateUI();
    }

    void UpdateUI()
    {
        enemiesLeftText.text = "Alive " + enemiesLeft;
        killsText.text = "Kills: " + kills;
    }
}
