using UnityEngine;

public class TriggerBigDoor : MonoBehaviour
{
    public Animator anim;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {  // Tag ton joueur "Player"
            anim.SetTrigger("TriggerPlayer");
        }
    }
}
