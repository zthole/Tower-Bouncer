using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    [Header("Movement")]
    public float horizontalSpeed = 15f;
    public float maxHorizontalSpeed = 12f;

    [Header("Jump")]
    public float bounceForce = 12f;

    [Header("References")]
    public Camera mainCamera;

    private Rigidbody2D rb;
    private float targetX;

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        if (mainCamera == null)
            mainCamera = Camera.main;

        targetX = transform.position.x;
    }

    void Update() {
        // Get mouse position in world coordinates
        Vector3 mousePosition = Input.mousePosition;

        mousePosition.z = -mainCamera.transform.position.z;

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        targetX = worldPosition.x;
    }

    void FixedUpdate() {
        // Calculate horizontal movement toward mouse
        float difference = targetX - rb.position.x;

        float horizontalMovement = Mathf.Clamp(
            difference * horizontalSpeed,
            -maxHorizontalSpeed,
            maxHorizontalSpeed
        );

        // Preserve vertical bouncing velocity
        rb.velocity = new Vector2(
            horizontalMovement,
            rb.velocity.y
        );

        // Keep player inside camera boundaries
        float leftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

        float rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        float playerRadius = 0.3f;

        float clampedX = Mathf.Clamp(
            rb.position.x,
            leftEdge + playerRadius,
            rightEdge - playerRadius
        );

        if (clampedX != rb.position.x) {
            rb.position = new Vector2(
                clampedX,
                rb.position.y
            );
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Platform")) {
            foreach (ContactPoint2D contact in collision.contacts) {
                // Only bounce when landing on top
                if (contact.normal.y > 0.5f &&
                    rb.velocity.y <= 0.1f) {
                    rb.velocity = new Vector2(
                        rb.velocity.x,
                        bounceForce
                    );

                    break;
                }
            }
        }
    }
}