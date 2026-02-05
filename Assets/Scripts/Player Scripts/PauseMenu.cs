using UnityEngine;
using UnityEngine.InputSystem;   // ← important

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPause = false;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

    // Cette méthode sera appelée par le nouveau Input System
    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (GameIsPause)
            Resume();
        else
            Pause();
    }

    void Resume()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
    }

    void Pause()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;
    }

    public void LoadSettings()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }
}
