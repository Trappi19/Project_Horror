using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Interact()
    {
        Debug.Log("Interagis avec " + gameObject.name);
        // Ici tes futures actions (caméra, etc.)
    }
}
