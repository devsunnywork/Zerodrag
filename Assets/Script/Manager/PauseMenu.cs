using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; 
    public static bool isPaused = false;

    void Start()
    {
        if (pauseMenuUI != null) 
        {
            pauseMenuUI.SetActive(false);
        }
        isPaused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    void Pause()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; 
        isPaused = true;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(0); 
    }
}