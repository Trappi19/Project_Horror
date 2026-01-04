using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnOnSceneLoad : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        player.transform.position = transform.position;
    }
}
