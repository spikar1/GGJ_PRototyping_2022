using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public float xAngle, yAngle, zAngle;
    public Space space;

    float XAngle => xAngle * Time.deltaTime;
    float YAngle => yAngle * Time.deltaTime;
    float ZAngle => zAngle * Time.deltaTime;
    


    void Update()
    {
        transform.Rotate(XAngle, YAngle, ZAngle, space);
    }
}
