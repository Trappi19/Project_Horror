using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;   // ← important

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPause = false;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public AudioMixerGroup musicMixerGroup;

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

    void Pause()
    {
        GameIsPause = true;
        AudioListener.pause = true;
        Time.timeScale = 0f;
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    void Resume()
    {
        AudioListener.pause = false;
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
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
