using System.Collections;
using UnityEngine;

public class IsometricCameraControl : MonoBehaviour
{
    public Transform target;
    public float distance = 10f;
    public float height = 5.7f;
    public float transitionDuration = 0.5f;
    int _currentAngleIndex;
    readonly float[] _angles = { 45, 135, 225, 315 };

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeAngle(1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeAngle(-1);
        }
    }

    void ChangeAngle(int direction)
    {
        _currentAngleIndex += direction;
        if (_currentAngleIndex >= _angles.Length) _currentAngleIndex = 0;
        else if (_currentAngleIndex < 0) _currentAngleIndex = _angles.Length - 1;

        StopAllCoroutines();
        StartCoroutine(TransitionCameraPosition());
    }

    IEnumerator TransitionCameraPosition()
    {
        float timeElapsed = 0;
        var startPosition = transform.position;
        var startRotation = transform.rotation;
        var angleRadians = _angles[_currentAngleIndex] * Mathf.Deg2Rad;

        var x = Mathf.Sin(angleRadians) * distance;
        var z = Mathf.Cos(angleRadians) * distance;
        var endPosition = new Vector3(x, 0, z) + target.position;
        endPosition.y += height;

        var endRotation = Quaternion.LookRotation(target.position - endPosition);

        while (timeElapsed < transitionDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, timeElapsed / transitionDuration);
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, timeElapsed / transitionDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        transform.rotation = endRotation;
    }
}
