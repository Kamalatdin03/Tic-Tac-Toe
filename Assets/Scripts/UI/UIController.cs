using UnityEngine;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    #region Properties

    [SerializeField] RectTransform headerText;
    [SerializeField] Ease ease;
    [SerializeField] float headerAnimationDuration;
    [SerializeField] UIPanel[] panels;
    bool isMute;

    #endregion

    #region Methods

    private void Start()
    {
        ActivePanelByName("Menu");
        isMute = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ActivePanelByName("Menu");
    }

    public void ActivePanelByName(string name)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i].name == name)
            {
                panels[i].panel.SetActive(true);
                headerText.DOAnchorPos(
                            panels[i].headerTextPos.anchoredPosition, 
                            headerAnimationDuration)
                            .SetEase(ease);

                if (panels[i].playSoundOnEnable && isMute) SoundManager.Instance?.PlaySound(SoundType.Buttton);
                continue;
            }

            panels[i].panel.SetActive(false);
        }
    }

    public void OpenLink(string url)
    {
        Application.OpenURL(url);
    }

    #endregion

    #region Classes

    [System.Serializable]
    class UIPanel
    {
        public string name;
        public RectTransform headerTextPos;
        public GameObject panel;
        public bool playSoundOnEnable = true;
    }

    #endregion
}
