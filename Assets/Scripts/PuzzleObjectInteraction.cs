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

    public Axis availableAxes = Axis.Everything;

    private void Update()
    {
        if(PlayModeManager.Instance.CurrentMode == PlayModeManager.PlayMode.Puzzle)
            GlobalAxisKeybind();
    }

    private void GlobalAxisKeybind()
    {

        foreach (var objectToTransform in objectsToTransform)
        {
            if (availableAxes.HasFlag(Axis.Y))
                objectToTransform.Rotate(Vector3.right, Input.GetAxisRaw("Vertical") * Time.unscaledDeltaTime * rotationSpeed, Space.World);

            if (availableAxes.HasFlag(Axis.X))
                objectToTransform.Rotate(Vector3.forward, -Input.GetAxisRaw("Horizontal") * Time.unscaledDeltaTime * rotationSpeed, Space.World);

            if (availableAxes.HasFlag(Axis.Z))
                objectToTransform.Rotate(Vector3.up, -Input.GetAxisRaw("Z Axis") * Time.unscaledDeltaTime * rotationSpeed, Space.World);
        }

    }

    [Flags]
    public enum Axis
    {
        None = 0x0,
        Everything = X | Y | Z,

        X = 0x1,
        Y = 0x2,
        Z = 0x4,
    }
}
