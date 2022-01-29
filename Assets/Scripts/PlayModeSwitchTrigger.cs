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
        if (collision.GetComponent<PlatformerPlayer>())
        {
            FindObjectOfType<CreateSpriteFromCamera>().TogglePlayMode();
            Destroy(gameObject);
        }
    }
}
