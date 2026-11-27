using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDistanceTracker : MonoBehaviour
{
    [Header("Distance UI")]
    public Text distanceText;

    private Rigidbody2D rb;

    private float startingY;
    private float highestY;

    private bool scoreSaved = false;

    public float CurrentDistance
    {
        get { return Mathf.Max(0, highestY - startingY); }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        startingY = GetPlayerY();
        highestY = startingY;

        UpdateDistanceUI();
    }

    void Update()
    {
        float currentY = GetPlayerY();

        // Track the highest position reached
        if (currentY > highestY)
        {
            highestY = currentY;
        }

        UpdateDistanceUI();
    }

    float GetPlayerY()
    {
        if (rb != null)
        {
            return rb.position.y;
        }

        return transform.position.y;
    }

    void UpdateDistanceUI()
    {
        if (distanceText != null)
        {
            distanceText.text = 
            "Distance: " +
            Mathf.RoundToInt(CurrentDistance) +
            " Units";
        }
    }

    public void EndRun()
    {
        if (scoreSaved)
            return;

        scoreSaved = true;

        // Get the player's saved name
        string playerName = PlayerPrefs.GetString(
            "PlayerName", "Player"
        );

        int finalScore = Mathf.RoundToInt(CurrentDistance);

        Debug.Log("Saving Score: " + finalScore);

        // Save the player's score
        LeaderboardManager.SaveScore(
            playerName,
            finalScore
        );
    }
}

