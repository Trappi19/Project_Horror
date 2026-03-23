using UnityEngine;

public class SoundTriggerMANUAL : MonoBehaviour
{
    public AudioSource audioSource;
    private bool hasPlayed = false;


    void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            audioSource.Play();
            hasPlayed = true;
        }
    }
}
