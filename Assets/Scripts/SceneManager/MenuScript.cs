using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LunchSceneIntro()
    {
        SceneManager.LoadScene("StartScene");
    }

}
