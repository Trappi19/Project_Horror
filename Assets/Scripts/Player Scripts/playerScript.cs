using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playerScript : MonoBehaviour
{

    [Header("Player Settings")]
    [SerializeField] public Input Action;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float speedRun = 2f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private Vector2 mouseSensitivity = Vector2.one;
    [SerializeField] public new Transform camera;

    [Header("Camera Smooth Settings")]
    [SerializeField] private float smoothSpeed = 8f;  // Ajuste pour plus/moins de smooth
    [SerializeField] private bool enableTilt = true;
    [SerializeField] private float tiltAmount = 2f;

    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionLayer = 1; // Crée un layer "Interactable"
    [SerializeField] private GameObject interactionPointUI;
    private Interactable currentInteractable;
    private bool showPoint = false;

    [Header("Movements")]
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 15f;

    private Vector3 currentHorizontalVelocity = Vector3.zero;

    public bool canMove = true;
    public bool isInCinematic = false;

    private Vector3 velocity;

    public static playerScript Instance;
    private Vector2 moveInputs, lookInputs;
    private bool jumpPerformed;
    private bool isSprinting = false;

    private CharacterController characterController;

    // Variables pour le smooth
    private float currentYRotation = 0f;
    private float targetYRotation = 0f;
    private float currentXRotation = 0f;
    private float targetXRotation = 0f;
    private float currentTilt = 0f;



    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (camera != null)
            currentXRotation = camera.localEulerAngles.x;
        else
            Debug.LogError("Camera non assignée !");

        currentYRotation = transform.eulerAngles.y; // Garde ça

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Le reste
    private void Update()
    {
        if (!canMove) return;

        Look();
        CheckInteraction();

    }

    //Toute la physique
    private void FixedUpdate()
    {
        float currentSpeed = isSprinting ? speedRun : speed;
        Vector3 inputDir = new Vector3(moveInputs.x, 0f, moveInputs.y);

        // direction locale -> monde (pour que ça suive l'orientation du player)
        Vector3 targetHorizontalVelocity = Vector3.zero;
        if (inputDir.sqrMagnitude > 0.001f)
        {
            inputDir = inputDir.normalized;
            targetHorizontalVelocity = (transform.right * inputDir.x + transform.forward * inputDir.z) * currentSpeed;

            // accélération quand on input
            currentHorizontalVelocity = Vector3.MoveTowards(
                currentHorizontalVelocity,
                targetHorizontalVelocity,
                acceleration * Time.fixedDeltaTime
            );
        }
        else
        {
            // pas d’input -> on freine doucement (glissade)
            currentHorizontalVelocity = Vector3.MoveTowards(
                currentHorizontalVelocity,
                Vector3.zero,
                deceleration * Time.fixedDeltaTime
            );
        }

        // gravité + saut comme avant
        float _gravityVelocity = Gravity(velocity.y);
        velocity = new Vector3(
            currentHorizontalVelocity.x,
            _gravityVelocity,
            currentHorizontalVelocity.z
        );

        TryJump();

        Vector3 _move = velocity * Time.fixedDeltaTime;
        characterController.Move(_move);

    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "StartScene")
        {
            if (camera != null)
                camera.localRotation = Quaternion.identity;
            SetCinematic(false);
            canMove = true;
            Debug.Log("Cam reset auto sur nouvelle scène !");
        }

        if (scene.name == "Menu")
        {
            // Force destroy de la caméra d'abord (pour l'AudioListener)
            var mainCam = transform.Find("Main Camera")?.gameObject;
            if (mainCam != null) Destroy(mainCam);

            // Puis le reste
            Destroy(gameObject);
        }
    }



    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Cleanup
    }

    public GameObject GetInteractionUI()
    {
        return interactionPointUI;
    }

    private void CheckInteraction()
    {
        if (interactionPointUI == null || camera == null) return;

        // ✅ CENTRÉ : Ray depuis centre écran viewport
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactionLayer))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);

            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null && !showPoint)
            {
                showPoint = true;
                interactionPointUI.SetActive(true);
                currentInteractable = interactable;
            }
        }
        else if (showPoint)
        {
            showPoint = false;
            interactionPointUI.SetActive(false);
            currentInteractable = null;
        }
    }


    public void SetCinematic(bool active)
    {
        isInCinematic = active;
        canMove = !active;

        // ✅ Désactive BodycamHeadBob pendant cinématique
        BodycamHeadBob headBob = camera.GetComponent<BodycamHeadBob>();
        if (headBob != null)
            headBob.enabled = !active;
    }

    public void SprintPerformed(InputAction.CallbackContext _ctx)
    {
        if (_ctx.performed)
            isSprinting = true;
        else if (_ctx.canceled)
            isSprinting = false;
    }


    private void Look()
    {
        if (isInCinematic) return;

        // Calcule la rotation target du player (gauche/droite)
        targetYRotation += lookInputs.x * Time.deltaTime * mouseSensitivity.x;

        // Calcule la rotation target de la caméra (haut/bas)
        targetXRotation -= lookInputs.y * Time.deltaTime * mouseSensitivity.y;

        // Clamp AVANT le smooth (important!)
        targetXRotation = Mathf.Clamp(targetXRotation, -85f, 85f);

        // Smooth les rotations avec Lerp
        currentYRotation = Mathf.LerpAngle(currentYRotation, targetYRotation, Time.deltaTime * smoothSpeed);
        currentXRotation = Mathf.Lerp(currentXRotation, targetXRotation, Time.deltaTime * smoothSpeed);

        // Applique la rotation smooth au player
        transform.rotation = Quaternion.Euler(0f, currentYRotation, 0f);

        // Tilt bodycam (inclinaison lors des rotations rapides)
        if (enableTilt)
        {
            float targetTiltValue = -lookInputs.x * tiltAmount;
            currentTilt = Mathf.Lerp(currentTilt, targetTiltValue, Time.deltaTime * smoothSpeed);
        }
        else
        {
            currentTilt = Mathf.Lerp(currentTilt, 0f, Time.deltaTime * smoothSpeed);
        }

        // Applique la rotation smooth + tilt à la caméra
        camera.localRotation = Quaternion.Euler(currentXRotation, 0f, currentTilt);
    }

    private float Gravity(float _verticalVelocity)
    {
        if (characterController.isGrounded) return 0f;

        _verticalVelocity += Physics.gravity.y * Time.fixedDeltaTime;

        return _verticalVelocity;
    }

    private void TryJump()
    {
        if (!jumpPerformed || !characterController.isGrounded) return;

        velocity.y += jumpForce;
        jumpPerformed = false;
    }

    public void MovePerformed(InputAction.CallbackContext _ctx) => moveInputs = _ctx.ReadValue<Vector2>();
    public void JumpPerformed(InputAction.CallbackContext _ctx) => jumpPerformed = _ctx.performed;
    public void LookPerformed(InputAction.CallbackContext _ctx) => lookInputs = _ctx.ReadValue<Vector2>();
    public void InteractPerformed(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

}

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance { get; private set; }

    public bool canMove = true;  // Déjà public comme avant
}

