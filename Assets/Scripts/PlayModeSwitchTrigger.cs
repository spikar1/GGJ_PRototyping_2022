using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class PlayModeSwitchTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.IsPlayer())
        {
            //TODO: Field should probably (maybe?) disappear after the player exits puzzle mode
            PlayModeManager.Instance.CurrentMode = PlayModeManager.PlayMode.Puzzle;
            Destroy(gameObject);
        }
    }
}
