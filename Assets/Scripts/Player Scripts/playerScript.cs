using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private bool wasInteractableLastFrame = false;
    private bool showPoint = false;


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
    }


    //Le reste
    private void Update()
    {
        Look();
        CheckInteraction();

    }

    private void CheckInteraction()
    {
        if (interactionPointUI == null || camera == null) return;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));

        // Debug ray depuis ORIGINE du raycast
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








    //Toute la physique
    private void FixedUpdate()
    {
        float currentSpeed = isSprinting ? speedRun : speed;
        Vector3 _horizontalVelocity = currentSpeed * new Vector3(moveInputs.x, 0f, moveInputs.y);
        float _gravityVelocity = Gravity(velocity.y);

        velocity = _horizontalVelocity + _gravityVelocity * Vector3.up;

        TryJump();

        Vector3 _move = transform.forward * velocity.z + transform.right * velocity.x + transform.up * velocity.y;

        characterController.Move(_move * Time.deltaTime);
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
