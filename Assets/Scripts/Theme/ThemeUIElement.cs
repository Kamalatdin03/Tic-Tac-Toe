using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class ThemeUIElement : MonoBehaviour
{
    public string id;
    [SerializeField] Graphic graphic;

    private void Start()
    {
        ThemeManager.Insatance.Add(this);
    }

    public void ChangeColor(Color targetColor, float duration, Ease ease = Ease.InOutCirc)
    {
        if (gameObject.active) graphic.DOColor(targetColor, duration).SetEase(ease);
        else graphic.color = targetColor;
    }

    public void FastChangeColor(Color targetColor)
    {
        graphic.color = targetColor;
    }
}
