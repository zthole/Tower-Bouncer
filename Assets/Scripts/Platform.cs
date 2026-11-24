using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("References")]
    public GameObject platformPrefab;
    public Transform player;
    public Camera mainCamera;

    [Header("Vertical Spacing")]
    public float minimumSpacing = 0.5f;

    [Range(0.1f, 0.5f)]
    public float jumpHeightPercentage = 0.2f;

    [Header("Platform Generation")]
    public int startingPlatforms = 5;
    public float spawnAheadDistance = 3f;

    [Header("Horizontal Spacing")]
    [Range(0.1f, 1f)]
    public float horizontalReachPercentage = 0.35f;

    [Header("Platform Size")]
    public float horizontalMargin = 0.75f;

    private float highestPlatformY;
    private float previousPlatformX;

    private float maxJumpHeight;
    private float gravity;
    private float bounceForce;
    private float horizontalSpeed;

    void Start() {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (platformPrefab == null)
        {
        Debug.LogError("Assign a Platform Prefab in the Inspector!");
        enabled = false;
        return;
        }

        Knight controller = player.GetComponent<Knight>();
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        // Read movement settings from Knight.cs
        bounceForce = controller.bounceForce;
        horizontalSpeed = controller.horizontalSpeed;

        // Calculate actual gravity
        gravity = Mathf.Abs(
            Physics2D.gravity.y * rb.gravityScale
        );

        // Calculate maximum theoretical jump height
        maxJumpHeight =
            (bounceForce * bounceForce) / (2f * gravity);

        highestPlatformY = player.position.y;
        previousPlatformX = player.position.x;

        // Generate starting platforms
        for (int i = 0; i < startingPlatforms; i++)
        {
            GeneratePlatform();
        }
    }

    void Update() {
        float cameraTop = mainCamera.ViewportToWorldPoint(
            new Vector3(0, 1, 0)
        ).y;

        // Generate platforms above the camera
        while (highestPlatformY <
               cameraTop + spawnAheadDistance)
        {
            GeneratePlatform();
        }
    }

    void GeneratePlatform() {
        // Use only 20% of maximum jump height
        float maxSpacing =
            maxJumpHeight * jumpHeightPercentage;

        // Avoid spacing larger than the jump limit
        float spacing = Random.Range(
            Mathf.Min(minimumSpacing, maxSpacing * 0.5f),
            maxSpacing
        );

        // Calculate time until player descends to next platform
        float discriminant =
            bounceForce * bounceForce -
            2f * gravity * spacing;

        discriminant = Mathf.Max(0f, discriminant);

        float flightTime =
            (bounceForce + Mathf.Sqrt(discriminant)) / gravity;

        // Calculate conservative horizontal travel distance
        float horizontalReach =
            horizontalSpeed *
            flightTime *
            horizontalReachPercentage;

        // Get camera boundaries
        float leftEdge = mainCamera.ViewportToWorldPoint(
            new Vector3(0, 0, 0)
        ).x;

        float rightEdge = mainCamera.ViewportToWorldPoint(
            new Vector3(1, 0, 0)
        ).x;

        // Restrict horizontal platform positions
        float minX = Mathf.Max(
            leftEdge + horizontalMargin,
            previousPlatformX - horizontalReach
        );

        float maxX = Mathf.Min(
            rightEdge - horizontalMargin,
            previousPlatformX + horizontalReach
        );

        // Choose a reachable horizontal position
        float xPosition = (minX <= maxX)
            ? Random.Range(minX, maxX)
            : Mathf.Clamp(
                previousPlatformX,
                leftEdge + horizontalMargin,
                rightEdge - horizontalMargin
            );

        // Update platform height
        highestPlatformY += spacing;

        Vector3 spawnPosition = new Vector3(
            xPosition,
            highestPlatformY,
            0f
        );

        // Make sure the prefab still exists
        if (platformPrefab == null)
        {
            Debug.LogError("Platform Prefab is missing or destroyed!");
            return;
        }

        Instantiate(
            platformPrefab,
            spawnPosition,
            Quaternion.identity
        );

        previousPlatformX = xPosition;
    }
}