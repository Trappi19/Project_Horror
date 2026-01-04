using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HospitalBedInteractable : Interactable
{
    [Header("Camera Movement")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private AnimationCurve smoothCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Transition")]
    [SerializeField] public Animator transition;

    private bool hasInteracted = false;

    public override void Interact()
    {
        if (hasInteracted) return; // ✅ Usage unique
        hasInteracted = true;

        Debug.Log("Interaction lit - Caméra bloquée");
        StartCoroutine(SmoothCameraMove());

    }

    private IEnumerator SmoothCameraMove()
    {
        playerScript.Instance.SetCinematic(true);

        Transform playerCam = playerScript.Instance.camera;
        Transform player = playerScript.Instance.transform;
        GameObject interactionUI = playerScript.Instance.GetInteractionUI();

        // Désactive contrôle + UI
        playerScript.Instance.canMove = false;
        if (interactionUI != null) interactionUI.SetActive(false);

        Vector3 startPos = playerCam.position;
        Quaternion startRot = playerCam.rotation;
        Vector3 targetPos = cameraTarget.position;

        // ✅ UTILISE la rotation du CameraTarget directement
        Quaternion targetRot = cameraTarget.rotation;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = smoothCurve.Evaluate(elapsed / moveDuration);

            playerCam.position = Vector3.Lerp(startPos, targetPos, t);
            playerCam.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        // ✅ FORCE position/rotation finale (anti-drift)
        playerCam.position = targetPos;
        playerCam.rotation = targetRot;

        Debug.Log("Caméra figée sur le lit !");

        // ❌ SUPPRIME OU COMMENTE ÇA pour garder figé :
        // yield return new WaitForSeconds(5f);
        // playerScript.Instance.canMove = true;

        yield return new WaitForSeconds(2);
        StartCoroutine(transitionLevel());


        // ✅ Pour revenir après 5 sec, AJOUTE aussi reset position :
        /*
        yield return new WaitForSeconds(5f);
        playerScript.Instance.canMove = true;
        playerScript.Instance.SetCinematic(false);
        // Reset position caméra
        */
    }


    IEnumerator transitionLevel()
    {
        transition.SetTrigger("Pass");
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("HospitalRoom");
    }

}
