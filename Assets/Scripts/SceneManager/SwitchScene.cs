using UnityEngine;

public class SwitchScene : MonoBehaviour
{
    [SerializeField] private GameObject WhiteScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WhiteScreen.SetActive(false);
    }
}
