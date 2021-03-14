using UnityEngine;

public enum SoundType
{
    Buttton, Tic, Tac, Win, Draw, SwitchOn, SwtichOff
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] SoundLibary sounds;
    [SerializeField] Vector2 randomVolumeRange;
    AudioSource source;
    bool isMute;

    private void Awake()
    {
        Instance = this;
        Init();
    }

    private void Init()
    {
        source = GetComponent<AudioSource>();

        if (randomVolumeRange == Vector2.zero)
        {
            randomVolumeRange = new Vector2(.9f, 1.1f);
        }

        if (sounds == null)
        {
            sounds = Resources.Load<SoundLibary>("Audio Clips");
        }
    }

    public void PlaySound(SoundType soundType)
    {
        if (isMute) return;

        AudioClip clip;
        clip = sounds.GetSoundByType(soundType);

        source.volume = Random.Range(randomVolumeRange.x, randomVolumeRange.y);
        source.PlayOneShot(clip);
    }

    public void Mute()
    {
        isMute = false;
    }

    public void OnMute()
    {
        isMute = true;
    }
}

