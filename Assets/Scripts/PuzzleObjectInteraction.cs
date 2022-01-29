using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObjectInteraction : MonoBehaviour
{
    public GameObject objectToTransform;

    public bool useLocalAxis = true;

    private void Update()
    {
        if (useLocalAxis)
            TransformLocalAxis();
        else
            TransformGlobalAxis();
    }

    private void TransformGlobalAxis()
    {
        
    }

    private void TransformLocalAxis()
    {
        
    }
}
