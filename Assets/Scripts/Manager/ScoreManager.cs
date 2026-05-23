using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private int kills = 0;
    private int enemiesLeft;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    public void EnemyKilled()
    {
        kills++;
        enemiesLeft--;
    }
}
