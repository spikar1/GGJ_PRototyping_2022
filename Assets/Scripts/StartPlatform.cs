using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlatform : MonoBehaviour
{
    [SerializeField]
    GameObject playerPrefab;

    PlatformerPlayer player;

    Vector3 playerStartPosition = Vector3.up * 1.5f;

    private void Awake()
    {
        //todo: OnScene loaded effect

        player = GetComponent<PlatformerPlayer>();
        if (!player)
            player = Instantiate(playerPrefab).AddComponent<PlatformerPlayer>();
        player.transform.position = transform.position + playerStartPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + playerStartPosition, .3f);
    }
}
