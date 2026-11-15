using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollows : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 5f;

    private float highestPlayerPosition;

    void Start() {
        highestPlayerPosition = transform.position.y;
    }

    void LateUpdate() {
        // Only follow when player reaches a new height
        if (player.position.y > highestPlayerPosition) {
            highestPlayerPosition = player.position.y;
        }

        Vector3 targetPosition = new Vector3(
            transform.position.x,
            highestPlayerPosition,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }
}

