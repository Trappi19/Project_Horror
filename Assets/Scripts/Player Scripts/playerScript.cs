using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerScript : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private Vector2 mouseSensitivity = Vector2.one;
    [SerializeField] public new Transform camera;

    private Vector3 velocity;

    private Vector2 moveInputs, lookInputs;
    private bool jumpPerformed;

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    //Le reste
    private void Update()
    {
        Look();
    }

    //Toute la physique
    private void FixedUpdate()
    {
        Vector3 _horizontalVelocity = speed * new Vector3(moveInputs.x, 0f, moveInputs.y);
        float _gravityVelocity = Gravity(velocity.y);

        velocity = _horizontalVelocity + _gravityVelocity * Vector3.up;

        TryJump();

        Vector3 _move = transform.forward * velocity.z + transform.right * velocity.x + transform.up * velocity.y;

        characterController.Move(_move * Time.deltaTime);
    }

    private void Look()
    {
        //Rotation de gauche a droite.
        transform.Rotate(lookInputs.x * Time.deltaTime * mouseSensitivity.x * Vector3.up);

        //Rotation de haut en bas.
        float _eyeAngleX = camera.localEulerAngles.x - lookInputs.y * Time.deltaTime * mouseSensitivity.y;

        //Clamp la rotation pour pas faire nimporte quoi avec la tete.
        if (_eyeAngleX <= 90) _eyeAngleX = _eyeAngleX > 0 ? Mathf.Clamp(_eyeAngleX, 0, 85) : _eyeAngleX;
        if (_eyeAngleX > 270) _eyeAngleX = Mathf.Clamp(_eyeAngleX, 275, 360);

        camera.localEulerAngles = Vector3.right * _eyeAngleX;
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



}