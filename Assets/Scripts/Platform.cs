using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
    [Header("References")]
    public GameObject platformPrefab;
    public Transform player;
    public Camera mainCamera;

    [Header("Platform Generation")]
    public float minimumPlatformSpacing = 1.5f;
    public float jumpHeightPercentage = 0.55f;
    public float spawnAheadDistance = 5f;

    [Header("Horizontal Settings")]
    public float horizontalSpawnRange = 3f;

    private float highestPlatformY;
    private float maxJumpHeight;

    void Start() {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Calculate maximum jump height
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

        float gravity = Mathf.Abs(
            Physics2D.gravity.y * playerRb.gravityScale
        );

        Knight controller =
            player.GetComponent<Knight>();

        maxJumpHeight =
            (controller.bounceForce * controller.bounceForce) / (2f * gravity);

        // Start generating from the player's starting position
        highestPlatformY = player.position.y;

        // Generate initial platforms
        GenerateInitialPlatforms();
    }

    void Update() {
        float cameraTop = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        // Keep generating platforms above the camera
        while (highestPlatformY < cameraTop + spawnAheadDistance) {
            GeneratePlatform();
        }
    }

    void GenerateInitialPlatforms() {
        // Generate several platforms above the starting position
        for (int i = 0; i < 5; i++) {
            GeneratePlatform();
        }
    }

    void GeneratePlatform() {
        // Keep vertical spacing within reachable jump height
        float maxSpacing =
            maxJumpHeight * jumpHeightPercentage;

        float spacing = Random.Range(
            minimumPlatformSpacing,
            maxSpacing
        );

        highestPlatformY += spacing;

        // Calculate camera's horizontal boundaries
        float leftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

        float rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        // Keep platform inside camera view
        float xPosition = Random.Range(
            leftEdge + horizontalSpawnRange,
            rightEdge - horizontalSpawnRange
        );

        Vector3 spawnPosition = new Vector3(
            xPosition,
            highestPlatformY,
            0
        );

        Instantiate(
            platformPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }
}

