using UnityEngine;
using System.Collections;


public class StartScene : MonoBehaviour
{
    [Header("Lunch new Game")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private AnimationCurve smoothCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Turn Right")]
    [SerializeField] private Transform cameraTarget2;
    [SerializeField] private float moveDuration2 = 2f;
    [SerializeField] private AnimationCurve smoothCurve2 = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Turn Left")]
    [SerializeField] private Transform cameraTarget3;
    [SerializeField] private float moveDuration3 = 2f;
    [SerializeField] private AnimationCurve smoothCurve3 = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Get Up")]
    [SerializeField] private Transform cameraTarget4;
    [SerializeField] private float moveDuration4 = 2f;
    [SerializeField] private Transform playerEndPosition;
    [SerializeField] private AnimationCurve smoothCurve4 = AnimationCurve.EaseInOut(0, 0, 1, 1);


    private playerScript playerScript;
    
    void Start()
    {
        playerScript = Object.FindFirstObjectByType<playerScript>();

        StartCoroutine(SequenceComplete());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private IEnumerator SequenceComplete()
    {
        playerScript.Instance.SetCinematic(true);
        playerScript.Instance.canMove = false;

        // Étape 1: Position sur le lit
        yield return StartCoroutine(SmoothCameraMove());

        // Étape 2: Tourne à droite
        yield return StartCoroutine(TurnRight());

        // Étape 3: Tourne à gauche  
        yield return StartCoroutine(TurnLeft());

        // Étape 4: Se lève
        yield return StartCoroutine(GetUp());

        // Fin de cinématique (réactive le joueur)
        playerScript.Instance.SetCinematic(false);
        playerScript.Instance.canMove = true;
        Debug.Log("Cinématique finie, joueur libre !");
    }


    private IEnumerator SmoothCameraMove()
    {
        yield return new WaitForSeconds(2);

        playerScript.Instance.SetCinematic(true);

        Transform playerCam = playerScript.Instance.camera;
        Transform player = playerScript.Instance.transform;
        GameObject interactionUI = playerScript.Instance.GetInteractionUI();

        // Désactive contrôle + UI
        playerScript.Instance.canMove = false;

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
        yield return new WaitForSeconds(3);
    }

    private IEnumerator TurnRight()
    {
        Transform playerCam = playerScript.Instance.camera;
        Transform player = playerScript.Instance.transform;
        GameObject interactionUI = playerScript.Instance.GetInteractionUI();

        // Désactive contrôle + UI
        playerScript.Instance.canMove = false;

        Vector3 startPos = playerCam.position;
        Quaternion startRot = playerCam.rotation;
        Vector3 targetPos = cameraTarget2.position;

        // ✅ UTILISE la rotation du CameraTarget directement
        Quaternion targetRot = cameraTarget2.rotation;

        float elapsed = 0f;

        while (elapsed < moveDuration2)
        {
            elapsed += Time.deltaTime;
            float t = smoothCurve2.Evaluate(elapsed / moveDuration2);

            playerCam.position = Vector3.Lerp(startPos, targetPos, t);
            playerCam.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        // ✅ FORCE position/rotation finale (anti-drift)
        playerCam.position = targetPos;
        playerCam.rotation = targetRot;
    }

    private IEnumerator TurnLeft()
    {
        Transform playerCam = playerScript.Instance.camera;
        Transform player = playerScript.Instance.transform;
        GameObject interactionUI = playerScript.Instance.GetInteractionUI();

        // Désactive contrôle + UI
        playerScript.Instance.canMove = false;

        Vector3 startPos = playerCam.position;
        Quaternion startRot = playerCam.rotation;
        Vector3 targetPos = cameraTarget3.position;

        // ✅ UTILISE la rotation du CameraTarget directement
        Quaternion targetRot = cameraTarget3.rotation;

        float elapsed = 0f;

        while (elapsed < moveDuration3)
        {
            elapsed += Time.deltaTime;
            float t = smoothCurve3.Evaluate(elapsed / moveDuration3);

            playerCam.position = Vector3.Lerp(startPos, targetPos, t);
            playerCam.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        // ✅ FORCE position/rotation finale (anti-drift)
        playerCam.position = targetPos;
        playerCam.rotation = targetRot;
    }

    private IEnumerator GetUp()
    {
        Transform playerCam = playerScript.Instance.camera;
        Transform player = playerScript.Instance.transform;
        GameObject interactionUI = playerScript.Instance.GetInteractionUI();

        // Désactive contrôle + UI
        playerScript.Instance.canMove = false;

        Vector3 startPos = playerCam.position;
        Quaternion startRot = playerCam.rotation;
        Vector3 targetPos = cameraTarget4.position;
        Quaternion targetRot = cameraTarget4.rotation;

        float elapsed = 0f;

        while (elapsed < moveDuration4)
        {
            elapsed += Time.deltaTime;
            float t = smoothCurve4.Evaluate(elapsed / moveDuration4);

            playerCam.position = Vector3.Lerp(startPos, targetPos, t);
            playerCam.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        // Force caméra finale
        playerCam.position = targetPos;
        playerCam.rotation = targetRot;

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
        }

        player.position = playerEndPosition.position;

        // ✅ Rotation Y du player = orientation horizontale finale
        Vector3 playerRot = player.eulerAngles;
        playerRot.y = playerEndPosition.eulerAngles.y;
        player.eulerAngles = playerRot;

        // ✅ Garde la rotation finale de la caméra (pas reset !)
        playerCam.rotation = targetRot;  // Ou copie la rotation de cameraTarget4

        Physics.SyncTransforms();

        if (cc != null)
        {
            cc.enabled = true;
        }

        Debug.Log("Player téléporté proprement !");
    }

}
