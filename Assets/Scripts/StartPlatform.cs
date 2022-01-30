using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlatform : MonoBehaviour
{
    [SerializeField]
    GameObject playerPrefab, textDisplayPrefab;

    Vector3 playerStartPosition => Vector3.up;

    private void Awake()
    {
        //todo: OnScene loaded effect
        Instantiate(textDisplayPrefab);

        GameObject player = this.GetPlayer();

        if (!player)
            player = Instantiate(playerPrefab);

        player.transform.position = transform.position + playerStartPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + playerStartPosition, .3f);
    }
}
