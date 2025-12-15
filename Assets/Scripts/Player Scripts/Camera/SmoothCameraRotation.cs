using UnityEngine;

public class SmoothCameraRotation : MonoBehaviour
{
    [Header("Smooth Rotation Settings")]
    public float smoothSpeed = 10f;  // Plus c'est bas, plus c'est smooth/lent
    public bool enableTilt = true;
    public float tiltAmount = 2f;    // Inclinaison lors des rotations rapides

    private Vector3 targetRotation;
    private Vector3 currentRotation;
    private float previousMouseX;

    void Start()
    {
        targetRotation = transform.localEulerAngles;
        currentRotation = transform.localEulerAngles;
    }

    void LateUpdate()
    {
        // Récupère la rotation "brute" de ta caméra (après ton script de rotation souris)
        targetRotation = transform.localEulerAngles;

        // Smooth vers la target rotation
        currentRotation = new Vector3(
            Mathf.LerpAngle(currentRotation.x, targetRotation.x, Time.deltaTime * smoothSpeed),
            Mathf.LerpAngle(currentRotation.y, targetRotation.y, Time.deltaTime * smoothSpeed),
            currentRotation.z
        );

        // Applique le tilt (inclinaison) selon la vitesse de rotation horizontale
        if (enableTilt)
        {
            float mouseVelocity = Input.GetAxis("Mouse X");
            float tilt = -mouseVelocity * tiltAmount;
            currentRotation.z = Mathf.Lerp(currentRotation.z, tilt, Time.deltaTime * smoothSpeed);
        }

        transform.localEulerAngles = currentRotation;
    }
}
