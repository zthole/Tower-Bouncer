using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCleanup : MonoBehaviour
{
    public float destroyDistance = 8f;
    private Camera mainCamera;

    void Start() {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            enabled = false;
        }
    }

    void Update() {
        if (mainCamera == null)
            return;

        float cameraBottom = mainCamera.ViewportToWorldPoint(
            new Vector3(0, 0, 0)
        ).y;

        if (transform.position.y < cameraBottom - destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}