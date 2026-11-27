using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFallDetector : MonoBehaviour
{
    [Header("Settings")]
    public float fallDistance = 2f;

    [Header("Scene Settings")]
    public string startMenuSceneName = "StartScene";

    private Camera mainCamera;
    private bool hasFallen = false;

    private PlayerDistanceTracker distanceTracker;

    void Start()
    {
        mainCamera = Camera.main;

        distanceTracker = GetComponent<PlayerDistanceTracker>();

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            enabled = false;
            return;
        }

        if (distanceTracker == null)
        {
            Debug.LogError("PlayerDistanceTracker not found on player");
        }
    }

    void Update()
    {
        if (hasFallen)
            return;

        // Calculate the bottom of the camera
        float cameraDistance = Mathf.Abs(
            transform.position.z - mainCamera.transform.position.z
        );

        float cameraBottom = mainCamera.ViewportToWorldPoint(
            new Vector3(0, 0, cameraDistance)
        ).y;

        // Check whether player has fallen below the screen
        if (transform.position.y < cameraBottom - fallDistance)
        {
            hasFallen = true;

            ReturnToStartMenu();
        }
    }

    void ReturnToStartMenu()
    {
        Debug.Log("Player fell. Returning to start screen");

        // PlayerDistanceTracker distanceTracker =
        // GetComponent<PlayerDistanceTracker>();

        // Save the player's distance before changing scenes
        if (distanceTracker != null)
        {
            distanceTracker.EndRun();
        }
      
        SceneManager.LoadScene(startMenuSceneName);
    }
}

