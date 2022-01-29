using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObjectInteraction : MonoBehaviour
{
    public Transform objectToTransform;

    public bool useLocalAxis = true;

    [SerializeField]
    PlatformerPlayer player;

    public enum Axis { X, Y, Z}
    public Axis transformAxis = Axis.X;
    Vector3 rotationAxis => transformAxis switch
    {
        Axis.X => Vector3.right,
        Axis.Y => Vector3.up,
        Axis.Z => Vector3.forward,
        _ => throw new ArgumentOutOfRangeException(nameof(transformAxis), "Axis not defined ")
    };

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

        //objectToTransform.Rotate(rotationAxis, Input.GetAxis("Vertical"), useLocalAxis ? Space.Self : Space.World);
        //transform.Rotate(rotationAxis, Input.GetAxis("Vertical"), Space.Self);
    }

    private void GlobalAxisKeybind()
    {
        objectToTransform.Rotate(Vector3.right, -Input.GetAxis("Vertical"), Space.World);
        transform.Rotate(Vector3.right, -Input.GetAxis("Vertical"), Space.World);

        objectToTransform.Rotate(Vector3.forward, -Input.GetAxis("Horizontal"), Space.World);
        transform.Rotate(Vector3.forward, -Input.GetAxis("Horizontal"), Space.World);

        objectToTransform.Rotate(Vector3.up, -Input.GetAxis("Z Axis"), Space.World);
        transform.Rotate(Vector3.up, -Input.GetAxis("Z Axis"), Space.World);

    }
}
