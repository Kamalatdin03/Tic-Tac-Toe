using UnityEngine;

[CreateAssetMenu(fileName = "Sound Libary", menuName = "TicTac/SoundLibary", order = 0)]
public class SoundLibary : ScriptableObject
{
    public AudioClip buttonSound;
    public AudioClip winSound;
    public AudioClip drawSound;
    public AudioClip ticSound;
    public AudioClip tacSound;
    public AudioClip switchOn;
    public AudioClip switchOff;

    public AudioClip GetSoundByType(SoundType type)
    {
        switch (type)
        {
            case SoundType.Win:
                return winSound;
            case SoundType.Tic:
                return ticSound;
            case SoundType.Tac:
                return tacSound;
            case SoundType.SwitchOn:
                return switchOn;
            case SoundType.SwtichOff:
                return switchOff;
            default:
                return buttonSound;
        }

    }
}