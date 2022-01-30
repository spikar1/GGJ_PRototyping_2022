using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    public static LevelLoadReason LastLevelLoadReason;

    private void Start()
    {
        switch (LastLevelLoadReason)
        {
            case LevelLoadReason.None:
                break;
            case LevelLoadReason.Finish:
                SoundManager.Instance.PlaySound(SoundManager.Sound.LevelClear);
                break;
            case LevelLoadReason.Reset:
                SoundManager.Instance.PlaySound(SoundManager.Sound.Reset);
                break;
            default:
                break;
        }

        LastLevelLoadReason = LevelLoadReason.None;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
            ResetToLastCheckpoint();

        if (Input.GetKeyDown(KeyCode.Escape))
            ResetLevel();

    }

    public static void ResetToLastCheckpoint()
    {
        if (PlayModeSwitchTrigger.HasCheckpoint())
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.Reset);
            PlayModeSwitchTrigger.ReloadLastCheckpoint();
            FindObjectOfType<PuzzleObjectInteraction>().LoadRotationsFromCheckpoint();
        }
            
        else
            ResetLevel();
    }

    public static void ResetLevel()
    {
        LastLevelLoadReason = LevelLoadReason.Reset;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public enum LevelLoadReason
    {
        None,
        Finish,
        Reset
    }
}
