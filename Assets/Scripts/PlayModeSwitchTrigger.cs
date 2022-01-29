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

    private const float ENTER_DELAY_TIME = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasBeenTriggered)
            return;

        if (collision.gameObject.IsPlayer())
        {
            _hasBeenTriggered = true;
            StartCoroutine(nameof(CoEnterSwitch));
        }
    }

    IEnumerator CoEnterSwitch()
    {
        //Sorry...
        Time.timeScale = 0f;
        //End sorry

        yield return new WaitForSecondsRealtime(ENTER_DELAY_TIME);
        PlayModeManager.Instance.CurrentMode = PlayModeManager.PlayMode.Puzzle;

        yield return new WaitUntil(() => PlayModeManager.Instance.CurrentMode != PlayModeManager.PlayMode.Puzzle);
        Destroy(gameObject);
    }
}
