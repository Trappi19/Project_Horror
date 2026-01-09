using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoSceneIntro : MonoBehaviour
{
    [Header("Assignés Inspector")]
    public VideoPlayer videoPlayer;
    public GameObject WhiteScreen;
    public Animator TransitionWhiteScreen;
    public Animator Logo;
    public AudioSource audioSource;

    
    private bool HPRoom = false;


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Awake()
    {
        // Lazy init si pas assigné
        if (videoPlayer == null) videoPlayer = GetComponent<VideoPlayer>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (WhiteScreen == null) WhiteScreen = GameObject.Find("WhiteScreen");  // Ou tag
        if (TransitionWhiteScreen == null) TransitionWhiteScreen = WhiteScreen?.GetComponent<Animator>();
        if (Logo == null) Logo = GameObject.Find("Logo")?.GetComponent<Animator>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "HospitalRoom" || HPRoom) return;

        // Checks null avant tout !
        if (TransitionWhiteScreen != null) TransitionWhiteScreen.SetBool("Pass", false);
        else Debug.LogError("TransitionWhiteScreen null !");

        if (audioSource != null) audioSource.Play();
        else Debug.LogError("AudioSource null !");


        if (videoPlayer != null) videoPlayer.Play();
        else Debug.LogError("VideoPlayer null !");

        Debug.Log("VHS Intro lancée !");
        HPRoom = true;
        StartCoroutine(LogoApparitionCloseVideo());

    }

    void EndReached(VideoPlayer vp)
    {
        vp.enabled = false;
    }

    IEnumerator LogoApparitionCloseVideo()
    {
        yield return new WaitForSeconds(6);
        if (Logo != null) Logo.SetTrigger("Active");
        yield return new WaitForSeconds(8);
        if (Logo != null) Logo.SetBool("Active", false);
        yield return new WaitForSeconds(2);
        TransitionWhiteScreen.SetBool("Black", true);
        yield return new WaitForSeconds(2);
        videoPlayer.Stop();
        TransitionWhiteScreen.SetBool("Black", true);   
        yield return new WaitForSeconds(0.5f);
        TransitionWhiteScreen.SetBool("Black", false);
    }
}
