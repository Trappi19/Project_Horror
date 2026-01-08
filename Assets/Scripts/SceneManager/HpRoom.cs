using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



public class HpRoom : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private AnimationCurve smoothCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private playerScript playerScript;
    void Start()
    {
        playerScript = Object.FindFirstObjectByType<playerScript>();
        // canMove False
        //if (playerScript != null)
        //{
        //    playerScript.canMove = false;
        //}
        //else
        //{
        //    Debug.LogError("playerScript est null !");
        //}
        StartCoroutine(SmoothCameraMove());
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }
}
