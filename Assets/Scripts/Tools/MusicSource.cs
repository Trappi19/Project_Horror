// MusicSource.cs — colle ce script sur chaque objet qui joue de la musique
using UnityEngine;

public class MusicSource : MonoBehaviour
{
    void Awake()
    {
        GetComponent<AudioSource>().ignoreListenerPause = true;
    }
}