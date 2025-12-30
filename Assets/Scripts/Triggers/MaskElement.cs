using UnityEngine;
using System.Collections;
public class MaskElement: MonoBehaviour
{
	public GameObject maskObject;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {  // Tag ton joueur "Player"
            maskObject.SetActive(false);
        }
    }
}