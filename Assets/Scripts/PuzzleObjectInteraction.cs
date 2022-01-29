using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObjectInteraction : MonoBehaviour
{
    public Transform objectToTransform;

    PlatformerPlayer player;

    private void Awake()
    {
        player = FindObjectOfType<PlatformerPlayer>();
    }

    private void Update()
    {
        //Test
        //todo: Change to timescale? 
        if(player.frozen)
            GlobalAxisKeybind();

    }

    private void GlobalAxisKeybind()
    {
        objectToTransform.Rotate(Vector3.right, Input.GetAxis("Vertical"), Space.World);
        transform.Rotate(Vector3.right, Input.GetAxis("Vertical"), Space.World);

        objectToTransform.Rotate(Vector3.forward, -Input.GetAxis("Horizontal"), Space.World);
        transform.Rotate(Vector3.forward, -Input.GetAxis("Horizontal"), Space.World);

        objectToTransform.Rotate(Vector3.up, -Input.GetAxis("Z Axis"), Space.World);
        transform.Rotate(Vector3.up, -Input.GetAxis("Z Axis"), Space.World);

    }
}
