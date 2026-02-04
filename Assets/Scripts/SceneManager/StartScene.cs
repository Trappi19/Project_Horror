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
        Transform playerCam = playerScript.Instance.camera;
        Transform player = playerScript.Instance.transform;

        // ✅ DÉTACHE la caméra du player pendant la cinématique
        Transform originalParent = playerCam.parent;
        Vector3 originalLocalPos = playerCam.localPosition;
        Quaternion originalLocalRot = playerCam.localRotation;

        playerCam.SetParent(null);  // La cam devient indépendante

        playerScript.Instance.SetCinematic(true);
        playerScript.Instance.canMove = false;

        // Toutes tes étapes de cinématique
        yield return StartCoroutine(SmoothCameraMove());
        yield return new WaitForSeconds(2);

        yield return StartCoroutine(TurnRight());
        yield return new WaitForSeconds(2);

        yield return StartCoroutine(TurnLeft());
        yield return new WaitForSeconds(1);

        yield return StartCoroutine(GetUp());  // TP du player ici

        // ✅ RÉATTACHE la caméra au player APRÈS le TP
        playerCam.SetParent(originalParent);
        playerCam.localPosition = originalLocalPos;  // Position locale par défaut
        playerCam.localRotation = Quaternion.Euler(
            playerScript.Instance.GetType()
                .GetField("currentXRotation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(playerScript.Instance) as float? ?? 0f,
            0f,
            0f
        );

        // Réactive le joueur
        playerScript.Instance.SetCinematic(false);
        playerScript.Instance.canMove = true;

        Debug.Log("Cinématique finie, joueur libre !");
    }



    private IEnumerator SmoothCameraMove()
    {

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

        Vector3 startPos = playerCam.position;
        Quaternion startRot = playerCam.rotation;
        Vector3 targetPos = cameraTarget4.position;
        Quaternion targetRot = cameraTarget4.rotation;

        float elapsed = 0f;

        // Animation caméra (se lever)
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

        // --- TÉLÉPORTATION PLAYER ---

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = false;

        // Position finale du joueur (Empty au sol)
        player.position = playerEndPosition.position;

        // On veut que le joueur regarde dans la même direction horizontale
        // que la caméra finale (ou que playerEndPosition)
        Vector3 finalCamEuler = playerCam.rotation.eulerAngles;
        Vector3 playerEuler = player.eulerAngles;
        playerEuler.y = finalCamEuler.y;          // même Y que la caméra
        player.eulerAngles = playerEuler;

        Physics.SyncTransforms();

        if (cc != null)
            cc.enabled = true;

        // --- SYNC AVEC LE SYSTÈME DE LOOK ---

        // On met les valeurs internes de playerScript
        playerScript ps = playerScript.Instance;

        // Y = rotation du joueur
        ps.GetType()
          .GetField("currentYRotation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
          .SetValue(ps, player.eulerAngles.y);

        ps.GetType()
          .GetField("targetYRotation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
          .SetValue(ps, player.eulerAngles.y);

        // X = rotation verticale de la caméra (prendre l'angle X de la cam locale)
        float camLocalX = playerCam.localEulerAngles.x;

        ps.GetType()
          .GetField("currentXRotation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
          .SetValue(ps, camLocalX);

        ps.GetType()
          .GetField("targetXRotation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
          .SetValue(ps, camLocalX);

        Debug.Log("Player + rotations synchro avec playerScript !");

        yield return null;
    }


}
