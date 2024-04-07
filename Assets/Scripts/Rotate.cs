using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Rotate : MonoBehaviour
{
    [SerializeField, Tooltip("Rotation speed")]
    float rotationSpeed;

    [SerializeField, Tooltip("Rotation direction")]
    RotationDirection rotationDirection;

    [Serializable]
    enum RotationDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    void Update()
    {
        var currentRotationDirection = rotationDirection switch
        {
            RotationDirection.Up => Vector3.up,
            RotationDirection.Down => Vector3.down,
            RotationDirection.Left => Vector3.left,
            RotationDirection.Right => Vector3.right,
            _ => throw new Exception("Rotation direction not defined")
        };

        transform.Rotate(currentRotationDirection, rotationSpeed * Time.deltaTime);
    }
}
