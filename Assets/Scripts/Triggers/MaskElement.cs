using UnityEngine;
using System.Collections;
public class MaskElement: MonoBehaviour
{
	public GameObject maskObject;
    public GameObject maskObjectLight;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {  // Tag ton joueur "Player"
            maskObject.SetActive(false);
            maskObjectLight.SetActive(false);
        }
    }
}