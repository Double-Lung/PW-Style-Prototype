using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCamera : MonoBehaviour
{
    private Camera eventCamera;
    private Canvas canvas;

    private void Awake() {
        eventCamera = FindObjectOfType<Camera>();
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = eventCamera;
    }
}
