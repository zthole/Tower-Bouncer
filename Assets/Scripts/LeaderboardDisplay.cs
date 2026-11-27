using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardDisplay : MonoBehaviour
{
    public Text leaderboardText;

    void Start()
    {
        leaderboardText.text =
            "HIGH SCORES\n\n" +
            LeaderboardManager.GetLeaderboard();
    }
}

