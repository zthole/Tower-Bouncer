using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCleanup : MonoBehaviour
{
    [Header("Platform Settings")]
    public float disappearDelay = 0.3f;

    private bool hasBeenJumpedOn = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player landed on the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Only trigger when player lands on top
                if (contact.normal.y < -0.5f)
                {
                    if (!hasBeenJumpedOn)
                    {
                        hasBeenJumpedOn = true;
                        StartCoroutine(Disappear());
                    }

                    break;
                }
            }
        }
    }

    IEnumerator Disappear()
    {
        // Wait before destroying platform
        yield return new WaitForSeconds(disappearDelay);

        Destroy(gameObject);
    }
}
