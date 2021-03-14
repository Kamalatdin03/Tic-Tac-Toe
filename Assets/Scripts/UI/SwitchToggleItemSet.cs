using UnityEngine;

public class SwitchToggleItemSet : MonoBehaviour
{
    [SerializeField] string key;

    private void OnEnable()
    {
        GetComponent<SwitchToggle>().IsOn = PlayerPrefs.GetInt(key) == 1;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt(key, GetComponent<SwitchToggle>().IsOn ? 1 : 0);
    }
}
