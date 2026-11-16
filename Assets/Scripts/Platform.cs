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
    public float minimumSpacing = 0.8f;
    [Range(0.1f, 0.8f)]
    public float jumpHeightPercentage = 0.3f;

    [Header("Generation")]
    public int startingPlatforms = 5;
    public float spawnAheadDistance = 3f;

    [Header("Horizontal Spacing")]
    [Range(0.1f, 1f)]
    public float horizontalReachPercentage = 0.65f;

    private float highestPlatformY;
    private float previousPlatformX;

    private float maxJumpHeight;
    private float gravity;
    private float bounceForce;
    private float horizontalSpeed;

    void Start() {
        if (mainCamera == null)
            mainCamera = Camera.main;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        Knight controller = player.GetComponent<Knight>();

        gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);

        bounceForce = controller.bounceForce;
        horizontalSpeed = controller.maxHorizontalSpeed;

        // Calculate theoretical jump height
        maxJumpHeight = (bounceForce * bounceForce) / (2f * gravity);

        highestPlatformY = player.position.y;
        previousPlatformX = player.position.x;

        for (int i = 0; i < startingPlatforms; i++) {
            GeneratePlatform();
        }
    }

    void Update() {
        float cameraTop = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        // Generate platforms above the camera
        while (highestPlatformY < cameraTop + spawnAheadDistance) {
            GeneratePlatform();
        }
    }

    void GeneratePlatform() {
        // Use a conservative portion of jump height
        float maxSpacing = maxJumpHeight * jumpHeightPercentage;

        float spacing = Random.Range(
            minimumSpacing,
            maxSpacing
        );

        spacing = Mathf.Min(spacing, maxSpacing);

        // Calculate the time needed to reach the next height
        float discriminant = (bounceForce * bounceForce) - (2f * gravity * spacing);

        discriminant = Mathf.Max(0f, discriminant);

        float flightTime = (bounceForce + Mathf.Sqrt(discriminant)) / gravity;

        // Calculate safe horizontal travel distance
        float horizontalReach = horizontalSpeed * flightTime * horizontalReachPercentage;

        // Get camera boundaries
        float leftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

        float rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        float margin = 0.5f;

        // Limit spawning to reachable horizontal positions
        float minX = Mathf.Max(
            leftEdge + margin,
            previousPlatformX - horizontalReach
        );

        float maxX = Mathf.Min(
            rightEdge - margin,
            previousPlatformX + horizontalReach
        );

        float xPosition = Random.Range(minX, maxX);

        // Move to next platform height
        highestPlatformY += spacing;

        Vector3 spawnPosition = new Vector3(
            xPosition,
            highestPlatformY,
            0f
        );

        Instantiate(
            platformPrefab,
            spawnPosition,
            Quaternion.identity
        );

        previousPlatformX = xPosition;
    }
}