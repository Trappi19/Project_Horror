using UnityEngine;
using UnityEngine.EventSystems;

public class SlideRightButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float slideDistance = 10f; // distance du glissement à droite (pixels)
    public float moveSpeed = 12f; // vitesse du glissement

    RectTransform rectTransform;
    Vector2 basePosition;
    bool isHovered = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        basePosition = rectTransform.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    void Update()
    {
        Vector2 targetPos = isHovered ? basePosition + Vector2.right * slideDistance : basePosition;
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPos, moveSpeed * Time.deltaTime);
    }
}
