using UnityEngine;

public class BodycamHeadBob : MonoBehaviour
{
    private Vector3 startPos;

    [Header("Head Bob Settings")]
    public float walkBobSpeed = 14f;
    public float walkBobAmount = 0.05f;
    public float runBobSpeed = 18f;
    public float runBobAmount = 0.11f;

    private float defaultYPos = 0f;
    private float timer = 0f;

    // Référence au controller pour connaître la vitesse
    public CharacterController controller;  // ou ton script player

    void Start()
    {
        startPos = transform.localPosition;
        defaultYPos = transform.localPosition.y;
    }

    void Update()
    {
        // Récupère la vitesse (adapte selon ton controller)
        float speed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;
        bool isMoving = speed > 0.1f;

        if (isMoving)
        {
            // Détermine si on marche ou court
            bool isRunning = speed > 5f;  // ajuste selon ta vitesse de sprint
            float bobSpeed = isRunning ? runBobSpeed : walkBobSpeed;
            float bobAmount = isRunning ? runBobAmount : walkBobAmount;

            // Incrémente le timer
            timer += Time.deltaTime * bobSpeed;

            // Calcule le mouvement avec sin/cos pour un mouvement fluide
            float wavesliceVertical = Mathf.Sin(timer);
            float wavesliceHorizontal = Mathf.Cos(timer * 0.5f);  // plus lent sur X

            Vector3 newPos = startPos;

            // Mouvement vertical (haut/bas)
            if (Mathf.Abs(wavesliceVertical) > 0.01f)
            {
                newPos.y = startPos.y + wavesliceVertical * bobAmount;
            }

            // Mouvement horizontal (gauche/droite) - plus subtil
            if (Mathf.Abs(wavesliceHorizontal) > 0.01f)
            {
                newPos.x = startPos.x + wavesliceHorizontal * bobAmount * 0.5f;
            }

            // Applique le smooth
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, Time.deltaTime * 10f);
        }
        else
        {
            // Retour à la position initiale smooth
            timer = 0f;
            Vector3 resetPos = new Vector3(startPos.x, defaultYPos, startPos.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, resetPos, Time.deltaTime * 10f);
        }
    }
}
