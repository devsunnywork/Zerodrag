using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Scale Settings")]
    public float hoverScale = 1.1f;       // Hover pe kitna bada ho
    public float clickScale = 0.95f;      // Click pe thoda chhota ho
    public float animSpeed = 8f;          // Animation ki speed

    private Vector3 originalScale;
    private Coroutine scaleCoroutine;

    void Start()
    {
        originalScale = transform.localScale;
    }

    // Mouse button ke upar aaya
    public void OnPointerEnter(PointerEventData eventData)
    {
        ScaleTo(originalScale * hoverScale);
    }

    // Mouse button se bahar gaya
    public void OnPointerExit(PointerEventData eventData)
    {
        ScaleTo(originalScale);
    }

    // Button press kiya
    public void OnPointerDown(PointerEventData eventData)
    {
        ScaleTo(originalScale * clickScale);
    }

    // Button release kiya
    public void OnPointerUp(PointerEventData eventData)
    {
        ScaleTo(originalScale * hoverScale);
    }

    void ScaleTo(Vector3 targetScale)
    {
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(SmoothScale(targetScale));
    }

    IEnumerator SmoothScale(Vector3 targetScale)
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.001f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animSpeed);
            yield return null;
        }
        transform.localScale = targetScale;
    }
}
