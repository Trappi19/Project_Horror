using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    public Animator doorAnimator; // Tu vas drag & drop l'Animator dans l'Inspector

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimator.SetBool("isOpen", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimator.SetBool("isOpen", false);
        }
    }
}
