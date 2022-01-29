using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObjectInteraction : MonoBehaviour
{
    public Transform[] objectsToTransform;

    [SerializeField]
    float rotationSpeed = 180;

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

    private void Awake()
    {
        NudgeObjects();
    }

    private void Update()
    {
        if(PlayModeManager.Instance.CurrentMode == PlayModeManager.PlayMode.Puzzle)
            GlobalAxisKeybind();
    }

    private void GlobalAxisKeybind()
    {

        foreach (var objectToTransform in objectsToTransform)
        {
            objectToTransform.Rotate(Vector3.right, Input.GetAxisRaw("Vertical") * Time.unscaledDeltaTime * rotationSpeed, Space.World);

            objectToTransform.Rotate(Vector3.forward, -Input.GetAxisRaw("Horizontal") * Time.unscaledDeltaTime * rotationSpeed, Space.World);

            objectToTransform.Rotate(Vector3.up, -Input.GetAxisRaw("Z Axis") * Time.unscaledDeltaTime * rotationSpeed, Space.World);
        }

    }
    private void NudgeObjects()
    {

        foreach (var objectToTransform in objectsToTransform)
        {
            objectToTransform.Rotate(Vector3.right, 1, Space.World);

            objectToTransform.Rotate(Vector3.forward, 1, Space.World);

            objectToTransform.Rotate(Vector3.up, 1, Space.World);
        }

    }
}
