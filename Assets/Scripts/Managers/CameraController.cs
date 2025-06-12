// ===================================
// Author: Andrada Joaquín Guillermo
// Script: CameraController
// Type: MonoBehaviour
//
// Description:
// This script controls the camera to follow a target within specified X and Y boundaries,
// ensuring the camera position stays clamped to a defined rectangular area.
//
// Course: Tabsil Unity 2D Game - Kawaii Survivor - The Coolest Roguelike Ever
//
// Date: 18/05/2025
// ===================================

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
