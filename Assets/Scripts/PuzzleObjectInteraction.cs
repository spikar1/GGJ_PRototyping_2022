using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObjectInteraction : MonoBehaviour
{
    public Transform objectToTransform;

    public bool useLocalAxis = true;

    public enum Axis { X, Y, Z}
    public Axis transformAxis = Axis.X;
    Vector3 rotationAxis => transformAxis switch
    {
        Axis.X => Vector3.right,
        Axis.Y => Vector3.up,
        Axis.Z => Vector3.forward,
        _ => throw new ArgumentOutOfRangeException(nameof(transformAxis), "Axis not defined ")
    };

    private void Update()
    {
        if(PlayModeManager.Instance.CurrentMode == PlayModeManager.PlayMode.Puzzle)
            GlobalAxisKeybind();
    }

    private void GlobalAxisKeybind()
    {
        objectToTransform.Rotate(Vector3.right, Input.GetAxisRaw("Vertical"), Space.World);
        transform.Rotate(Vector3.right, Input.GetAxisRaw("Vertical"), Space.World);

        objectToTransform.Rotate(Vector3.forward, -Input.GetAxisRaw("Horizontal"), Space.World);
        transform.Rotate(Vector3.forward, -Input.GetAxisRaw("Horizontal"), Space.World);

        objectToTransform.Rotate(Vector3.up, -Input.GetAxisRaw("Z Axis"), Space.World);
        transform.Rotate(Vector3.up, -Input.GetAxisRaw("Z Axis"), Space.World);

    }
}
