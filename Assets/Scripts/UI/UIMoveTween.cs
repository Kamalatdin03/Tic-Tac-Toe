using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UIMoveTween : MonoBehaviour
{
    [SerializeField] Ease ease;
    [SerializeField] float animationDuration;
    [SerializeField] Vector2 additionVector;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] RectTransform rect;
    Vector2 fristPos;
    Vector2 targetPos;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        canvasGroup.alpha = 0;
        rect.anchoredPosition = targetPos;
        canvasGroup.DOFade(1, animationDuration).SetEase(ease);
        rect.DOAnchorPos(fristPos, animationDuration).SetEase(ease);
    }

    private void Init()
    {
        fristPos = rect.anchoredPosition;
        canvasGroup = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
        targetPos = fristPos + additionVector;
    }
}
