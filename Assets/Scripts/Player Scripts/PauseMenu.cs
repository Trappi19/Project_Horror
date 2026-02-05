using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;   // ← important

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPause = false;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

    [Header("Animator")]
    [SerializeField] private Animator transitionAnimator;


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

    public void GoBackMenu()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
    }

    public void ReturnToMenue()
    {
        Time.timeScale = 1f;
        GameIsPause = false;
        StartCoroutine(LoadReturnToMenu());
    }

    IEnumerator LoadReturnToMenu()
    {
        transitionAnimator.SetTrigger("Fading");
        yield return new WaitForSeconds(3); // Simule une courte pause pour la transition
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
