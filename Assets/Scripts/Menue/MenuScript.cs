using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject menuPanel;
    [SerializeField] AudioSource click;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LunchSceneIntro()
    {
        click.Play();
        SceneManager.LoadScene("StartScene");
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

}
