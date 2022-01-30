using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static RenewableLazy<SoundManager> _instance = new RenewableLazy<SoundManager>(
        () => FindObjectOfType<SoundManager>()
    );

    public static SoundManager Instance => _instance.Value;

    public enum Sound
    {
        EnterZone,
        Jump,
        Reset,
        MoveX,
        MoveY,
        MoveZ,
        LevelClear
    }

    public void PlaySound(Sound sound, bool overrideCurrentPlayback = true)
    {
        AudioSource src = GameObject.Find(sound.ToString() + "SFX").GetComponent<AudioSource>();

        if (!src.isPlaying || overrideCurrentPlayback)
            src.Play();
    }

    public void StopSound(Sound sound)
    {
        GameObject.Find(sound.ToString() + "SFX").GetComponent<AudioSource>().Stop();
    }
}
