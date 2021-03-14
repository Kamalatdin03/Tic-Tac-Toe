using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UIScaleTween : MonoBehaviour
{
    [SerializeField] Ease ease = Ease.InOutCirc;
    [SerializeField] float animationDuration = 0.3f;
    [SerializeField] Vector2 startScale;
    [SerializeField] Vector2 targetScale;
    CanvasGroup canvasGroup;
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        ReadyToAnimate();
        canvasGroup.DOFade(1, animationDuration).SetEase(ease);
        rect.DOScale(targetScale, animationDuration).SetEase(ease);
    }

    private void ReadyToAnimate()
    {
        canvasGroup.alpha = 0;
        rect.localScale = startScale;
    }
}
