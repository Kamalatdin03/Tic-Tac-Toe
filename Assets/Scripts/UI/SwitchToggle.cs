using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

[ExecuteInEditMode]
public class SwitchToggle : MonoBehaviour, IPointerClickHandler
{
    #region Properties

    [SerializeField] Image tumb;
    [SerializeField] Color activeColor;
    [SerializeField] Color deActiveColor;
    [SerializeField] float animationDuration;

    [SerializeField] State onState;
    [SerializeField] State offState;

    [SerializeField] bool isOn = false;
    [SerializeField] bool playSoundOnSwitch = true;
    [SerializeField] bool intreactable = true;

    public bool IsOn
    {
        get
        {
            return isOn;
        }
        set
        {
            isOn = value;

            if (isOn) On();
            else Off();
        }
    }

    #endregion

    #region Methods

    private void OnRectTransformDimensionsChange()
    {
        IsOn = isOn;
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) IsOn = isOn;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (intreactable) Toggle();
    }

    public void On()
    {
        SwitchTumb(onState);
        isOn = true;
    }

    public void Off()
    {
        SwitchTumb(offState);
        isOn = false;
    }

    public void Toggle()
    {
        if (isOn = !isOn) On();
        else Off();

        if (playSoundOnSwitch) SoundManager.Instance?.PlaySound(isOn ? SoundType.SwitchOn : SoundType.SwtichOff);
    }

    private void SwitchTumb(State state)
    {
        State deActiveItem = state == offState ? onState : offState;
        Ease ease = state.ease;

        if (!Application.isPlaying)
        {
            tumb.rectTransform.localPosition = new Vector2(state.switchItem.rectTransform.localPosition.x, 0);
            tumb.color = state.tumbColor;
            state.switchItem.color = activeColor;
            deActiveItem.switchItem.color = deActiveColor;
        }

        tumb.rectTransform.DOLocalMoveX(
                 state.switchItem.rectTransform.localPosition.x,
                 animationDuration).SetEase(ease);
        tumb.DOColor(state.tumbColor, animationDuration)
            .SetEase(ease);
        state.switchItem.DOColor(activeColor, animationDuration)
            .SetEase(ease);
        deActiveItem.switchItem.DOColor(deActiveColor, animationDuration)
            .SetEase(ease);

        state.OnSwitch?.Invoke();
    }

    #endregion

    #region Classes

    [System.Serializable]
    sealed class State
    {
        public Ease ease;
        public Graphic switchItem;
        public Color tumbColor;
        public UnityEngine.Events.UnityEvent OnSwitch;
    }

    #endregion
}
