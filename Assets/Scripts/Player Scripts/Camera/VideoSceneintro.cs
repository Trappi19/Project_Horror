using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoSceneIntro : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private bool testSceneLoaded = false;
    public GameObject WhiteScreen;
    public Animator TransitionWhiteScreen;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;  // Écoute TOUS les loads
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // Nettoie
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "HPRoom" && !testSceneLoaded)
        {
            videoPlayer.Play();
            testSceneLoaded = true;  // One-shot
            //WhiteScreen.SetActive(false);
            TransitionWhiteScreen.SetTrigger("Pass");
            Debug.Log("VHS Intro lancée sur TestScene !");
            StartCoroutine(WhiteScreenToBlack());
        }
    }

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer == null) videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer vp)
    {
        vp.enabled = false;
    }

    IEnumerator WhiteScreenToBlack()
    {
        yield return new WaitForSeconds(10);
        TransitionWhiteScreen.SetTrigger("SlowDefault");
    }
}
