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

    Quaternion[] rotationsAtLastCheckpoint;
    Quaternion[] rotationsAtStart;

    private void Update()
    {
        if(PlayModeManager.Instance.CurrentMode == PlayModeManager.PlayMode.Puzzle)
            GlobalAxisKeybind();
    }

    private void Start()
    {
        SaveRotationsAtStart();
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

    [ContextMenu("Save from start")]
    public void SaveRotationsAtStart()
    {
        rotationsAtStart = new Quaternion[objectsToTransform.Length];
        for (int i = 0; i < rotationsAtStart.Length; i++)
        {
            rotationsAtStart[i] = objectsToTransform[i].rotation;
        }
    }

    [ContextMenu("Load from start")]
    public void LoadRotationsFromStart()
    {
        for (int i = 0; i < rotationsAtStart.Length; i++)
        {
            var rot = rotationsAtStart[i];
            objectsToTransform[i].rotation = rot;
        }
    }
    [ContextMenu("Save")]
    public void SaveRotationsAtCheckpoint()
    {
        rotationsAtLastCheckpoint = new Quaternion[objectsToTransform.Length];
        for (int i = 0; i < rotationsAtLastCheckpoint.Length; i++)
        {
            rotationsAtLastCheckpoint[i] = objectsToTransform[i].rotation;
        }
    }

    [ContextMenu("Load")]
    public void LoadRotationsFromCheckpoint()
    {
        for (int i = 0; i < rotationsAtLastCheckpoint.Length; i++)
        {
            var rot = rotationsAtLastCheckpoint[i];
            objectsToTransform[i].rotation = rot;
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
