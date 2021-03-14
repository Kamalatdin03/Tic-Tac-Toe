using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Insatance;
    [SerializeField] private DG.Tweening.Ease ease;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] Item[] items;
    private List<ThemeUIElement> uiItems = new List<ThemeUIElement>();

    private void Awake()
    {
        Insatance = this;
    }

    public void Add(ThemeUIElement item)
    {
        uiItems.Add(item);
        SwitchColor(PlayerPrefs.GetInt("Theme") == 1, true);
    }

    public void SwitchToDay()
    {
        SwitchColor(true);
    }

    public void SwitchToNight()
    {
        SwitchColor(false);
    }

    private void SwitchColor(bool isDay, bool isFast = false)
    {
        foreach (var item in items)
        {
            foreach (var t in uiItems)
            {
                if (t.id == item.id)
                {
                    Color targetColor = isDay ? item.dayColor : item.nigthColor;

                    if (isFast) t.FastChangeColor(targetColor);
                    else t.ChangeColor(targetColor, animationDuration, ease);
                }
            }
        }
    }

    [System.Serializable]
    class Item
    {
        public string id;
        public Color dayColor;
        public Color nigthColor;
    }
}
