using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public GameObject deathPanel;
    public TextMeshProUGUI deathText;
    public Transform respawnPoint;
    public GameObject player;
    public float respawnDelay = 3f;

    void Start()
    {
        deathPanel.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        deathPanel.SetActive(true);
        deathText.text = "YOU DIED";
        player.GetComponent<PlayerMovement>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(respawnDelay);

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false; // 🚫 Physics band karo teleport ke liye

        player.transform.position = respawnPoint.position;

        if (cc != null) cc.enabled = true;  // ✅ Physics wapas chalu

        player.GetComponent<PlayerHealth>().Respawn();
        player.GetComponent<PlayerMovement>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        deathPanel.SetActive(false);
    }
}
