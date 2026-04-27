using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class TVButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Renderer de la télé")]
    public Renderer tvRenderer;

    [Header("Index du slot de l'écran (voir Mesh Renderer → Materials)")]
    public int screenMaterialIndex = 1;

    [Header("Vidéo")]
    public VideoPlayer videoPlayer;

    [Header("Emission (glow de l'écran)")]
    public Color glowColor = new Color(0.2f, 0.6f, 1f);
    public float glowIntensity = 1.5f;

    [Header("Action au clic")]
    public UnityEvent onClick;

    private Material _screenMat;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    void Start()
    {
        // Fallback si rien assigné dans l'Inspector
        if (tvRenderer == null)
            tvRenderer = GetComponent<Renderer>();

        // Cloner les materials pour ne pas modifier les assets originaux
        Material[] mats = tvRenderer.materials;

        if (screenMaterialIndex >= mats.Length)
        {
            Debug.LogError($"[TVButton] screenMaterialIndex ({screenMaterialIndex}) invalide ! Ce mesh a seulement {mats.Length} material(s).");
            return;
        }

        _screenMat = mats[screenMaterialIndex];
        _screenMat.EnableKeyword("_EMISSION");
        _screenMat.SetColor(EmissionColor, Color.black); // éteint par défaut

        if (videoPlayer != null)
            videoPlayer.Stop();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("[TVButton] Hover ✅");
        SetScreen(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("[TVButton] Exit ✅");
        SetScreen(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("[TVButton] Clic ✅");
        SetScreen(false);
        onClick?.Invoke();
    }

    private void SetScreen(bool on)
    {
        if (_screenMat == null) return;

        if (videoPlayer != null)
        {
            if (on) videoPlayer.Play();
            else videoPlayer.Stop();
        }

        _screenMat.SetColor(EmissionColor, on ? glowColor * glowIntensity : Color.black);
    }
}