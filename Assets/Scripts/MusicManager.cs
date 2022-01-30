using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Plays the game's soundtrack
/// </summary>
public class MusicManager : MonoBehaviour
{
    public AudioSource PlatformModeMusic;
    public AudioSource PuzzleModeMusic;

    private PlayModeManager.PlayMode _lastKnownPlayMode;

    public IEnumerator Start()
    {
        const float TRANSITION_SPEED = 0.01f;
        const float MAX_VOLUME = 0.5f;

        DontDestroyOnLoad(gameObject);

        while (true)
        {
            yield return new WaitUntil(() => PlayModeManager.Instance != null);

            if (PlayModeManager.Instance.CurrentMode != _lastKnownPlayMode)
            {
                _lastKnownPlayMode = PlayModeManager.Instance.CurrentMode;

                if (PlayModeManager.Instance.CurrentMode == PlayModeManager.PlayMode.Puzzle)
                {
                    while (PlatformModeMusic.volume > 0 && PuzzleModeMusic.volume < MAX_VOLUME)
                    {
                        PuzzleModeMusic.volume = Mathf.Min(MAX_VOLUME, PuzzleModeMusic.volume + TRANSITION_SPEED);
                        PlatformModeMusic.volume = Mathf.Max(0f, PlatformModeMusic.volume - TRANSITION_SPEED);

                        yield return new WaitForEndOfFrame();
                    }
                }
                else if (PlayModeManager.Instance.CurrentMode == PlayModeManager.PlayMode.Platform)
                {
                    while (PuzzleModeMusic.volume > 0 && PlatformModeMusic.volume < MAX_VOLUME)
                    {
                        PlatformModeMusic.volume = Mathf.Min(MAX_VOLUME, PlatformModeMusic.volume + TRANSITION_SPEED);
                        PuzzleModeMusic.volume = Mathf.Max(0f, PuzzleModeMusic.volume - TRANSITION_SPEED);

                        yield return new WaitForEndOfFrame();
                    }
                }
            }

            yield return new WaitForEndOfFrame();

            Debug.Log(Time.unscaledTime);
        }
    }
}
