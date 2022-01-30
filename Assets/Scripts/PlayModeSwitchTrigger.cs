using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class PlayModeSwitchTrigger : MonoBehaviour
{
    private bool _hasBeenTriggered;

    private static PlayModeSwitchTrigger _lastCheckpoint;

    private const float CENTERING_TIME_INITIAL_DELAY = 0.25f;
    private const float CENTERING_TIME = 0.25f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasBeenTriggered)
            return;

        if (collision.gameObject.IsPlayer())
        {
            _hasBeenTriggered = true;
            StartCoroutine(CoEnterSwitch(this.GetPlayer().transform));
        }
    }

    IEnumerator CoEnterSwitch(Transform player)
    {
        //Sorry...
        Time.timeScale = 0f;
        //End sorry

        yield return new WaitForSecondsRealtime(CENTERING_TIME_INITIAL_DELAY);

        Vector2 startPos = player.transform.position;
        Vector2 endPos = transform.position;

        for (float i = 0; i < CENTERING_TIME; i += Time.unscaledDeltaTime)
        {
            player.transform.position = Vector3.Lerp(startPos, endPos, i / CENTERING_TIME);
            yield return new WaitForEndOfFrame();
        }

        player.transform.position = endPos;

        PlayModeManager.Instance.CurrentMode = PlayModeManager.PlayMode.Puzzle;

        yield return new WaitUntil(() => PlayModeManager.Instance.CurrentMode != PlayModeManager.PlayMode.Puzzle);


        FindObjectOfType<PuzzleObjectInteraction>().SaveRotationsAtCheckpoint();
        _lastCheckpoint = this;
        gameObject.SetActive(false);
    }

    public static bool HasCheckpoint()
    {
        return _lastCheckpoint;
    }

    public static void ReloadLastCheckpoint()
    {
        if (_lastCheckpoint == null)
            return;

        _lastCheckpoint.gameObject.SetActive(true);
        _lastCheckpoint._hasBeenTriggered = false;
        CommonExtensions.GetPlayer().transform.position = _lastCheckpoint.transform.position;
    }
}
