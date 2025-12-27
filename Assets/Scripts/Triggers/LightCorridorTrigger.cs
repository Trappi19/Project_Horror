using System.Collections;
using UnityEngine;

[System.Serializable]
public class LampGroup
{
    public Light[] lightsInLamp; // Les 2 lights (ou plus) de cette lampe
}

public class LightCorridorTrigger : MonoBehaviour
{
    [SerializeField] private LampGroup[] lampGroups;   // Toutes tes lampes du couloir
    [SerializeField] private float delayBetweenLamps = 1.5f;
    private AudioSource audioSource;
    private bool hasTriggered = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Tout éteindre au début
        foreach (var group in lampGroups)
        {
            foreach (var l in group.lightsInLamp)
            {
                l.enabled = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {   
        if (!hasTriggered && other.CompareTag("Player"))
        {
            audioSource.Play();
            StartCoroutine(ActivateLampGroupsSequentially());
            hasTriggered = true;
        }
    }

    IEnumerator ActivateLampGroupsSequentially()
    {
        bool isFirst = true;

        foreach (var group in lampGroups)
        {
            if (!isFirst)
                yield return new WaitForSeconds(delayBetweenLamps);

            foreach (var l in group.lightsInLamp)
                l.enabled = true;

            isFirst = false;
        }
    }

}
