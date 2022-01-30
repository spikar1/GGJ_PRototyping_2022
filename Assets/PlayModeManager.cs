using UnityEngine;

public class PlayModeManager : MonoBehaviour
{
    private static RenewableLazy<PlayModeManager> _instance = new RenewableLazy<PlayModeManager>(
        () => FindObjectOfType<PlayModeManager>()
    );

    public static PlayModeManager Instance => _instance.Value;


    private PlayMode _lastKnownMode;
    public PlayMode CurrentMode;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (_lastKnownMode != CurrentMode)
        {
            if (CurrentMode == PlayMode.Puzzle)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
                this.GetPlayer().GetComponent<Rigidbody2D>().velocity = default;
            }
                

            _lastKnownMode = CurrentMode;
        }
    }

    public void SwitchModes()
    {
        if (CurrentMode == PlayMode.Puzzle)
            CurrentMode = PlayMode.Platform;

        else
            CurrentMode = PlayMode.Puzzle;
    }

    public enum PlayMode
    {
        Platform,
        Puzzle
    }
}
