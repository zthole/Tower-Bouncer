using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    [Header("Movement")]
    public float horizontalSpeed = 20f;

    [Header("Jump")]
    public float bounceForce = 12f;

    [Header("References")]
    public Camera mainCamera;

    private Rigidbody2D rb;
    private float targetX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (mainCamera == null)
            mainCamera = Camera.main;

        targetX = transform.position.x;
    }

    void Update()
    {
        // Convert mouse position to world position
        Vector3 mousePosition = Input.mousePosition;

        mousePosition.z = Mathf.Abs(
            mainCamera.transform.position.z -
            transform.position.z
        );

        Vector3 worldPosition =
            mainCamera.ScreenToWorldPoint(mousePosition);

        targetX = worldPosition.x;
    }

    void FixedUpdate()
    {
        // Camera boundaries
        float leftEdge = mainCamera.ViewportToWorldPoint(
            new Vector3(0, 0, 0)
        ).x;

        float rightEdge = mainCamera.ViewportToWorldPoint(
            new Vector3(1, 0, 0)
        ).x;

        float playerWidth = 0.3f;

        targetX = Mathf.Clamp(
            targetX,
            leftEdge + playerWidth,
            rightEdge - playerWidth
        );

        // Horizontal movement only
        float horizontalVelocity = Mathf.Clamp(
            (targetX - rb.position.x) * horizontalSpeed,
            -horizontalSpeed,
            horizontalSpeed
        );

        // Preserve vertical velocity
        rb.velocity = new Vector2(
            horizontalVelocity,
            rb.velocity.y
        );
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Platform"))
            return;

        // Check whether player landed on top
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                // Apply bounce
                rb.velocity = new Vector2(
                    rb.velocity.x,
                    bounceForce
                );

                return;
            }
        }
    }
}
