using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 12f;

    [Header("Jump Settings")]
    public float bounceForce = 12f;

    private Rigidbody2D rb;
    private Camera mainCamera;

    private float mouseX;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void Update() {
        // Get mouse position in world coordinates
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseX = mousePosition.x;
    }

    void FixedUpdate() {
        // Move player horizontally toward mouse
        float direction = mouseX - transform.position.x;

        float horizontalVelocity = Mathf.Clamp(
            direction * moveSpeed,
            -moveSpeed,
            moveSpeed);

        rb.velocity = new Vector2(
            horizontalVelocity,
            rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Only bounce when landing on top of a platform
        if (collision.gameObject.CompareTag("Platform")) {
            foreach (ContactPoint2D contact in collision.contacts) {
                if (contact.normal.y > 0.5f && rb.velocity.y <= 0.1f) {
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

