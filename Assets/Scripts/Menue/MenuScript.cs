using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject menuPanel;
    [SerializeField] AudioSource click;
    [SerializeField] AudioSource StartSound;
    [SerializeField] Animator fader;
    

    void Start()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    //private void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}

    public void LunchSceneIntro()
    {
        StartSound.Play();
        StartCoroutine(LunchNewGame());
    }

    public void OpenSettings()
    {
        click.Play();
        menuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        click.Play();
        menuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    IEnumerator LunchNewGame()
    {
        fader.SetTrigger("Fading");
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("StartScene");
    }

    IEnumerator ContinnueGame()
    {
        yield break;
    }

}
