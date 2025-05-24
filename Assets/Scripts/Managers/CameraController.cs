using System;
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform target;
    
    [SerializeField] private Vector2 minMaxXY;

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position;
        targetPosition.z = -10;

        targetPosition.x = Mathf.Clamp(targetPosition.x,-minMaxXY.x,minMaxXY.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y,-minMaxXY.y,minMaxXY.y);

        transform.position = targetPosition;
    }
}
