using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoSceneIntro : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private bool testSceneLoaded = false;
    public GameObject WhiteScreen;

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
        if (scene.name == "TestScene" && !testSceneLoaded)
        {
            videoPlayer.Play();
            testSceneLoaded = true;  // One-shot
            WhiteScreen.SetActive(false);
            Debug.Log("VHS Intro lancée sur TestScene !");
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
}
