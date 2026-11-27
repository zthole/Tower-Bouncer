using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    private const int MaxScores = 5;

    public static void SaveScore(string playerName, int score)
    {
        int[] scores = new int[MaxScores];
        string[] names = new string[MaxScores];

        // Load existing leaderboard
        for (int i = 0; i < MaxScores; i++)
        {
            scores[i] = PlayerPrefs.GetInt("Score" + i, 0);
            names[i] = PlayerPrefs.GetString("Name" + i, "");
        }

        // Find where the new score belongs
        for (int i = 0; i < MaxScores; i++)
        {
            if (score > scores[i])
            {
                // Shift lower scores down
                for (int j = MaxScores - 1; j > i; j--)
                {
                    scores[j] = scores[j - 1];
                    names[j] = names[j - 1];
                }

                scores[i] = score;
                names[i] = playerName;

                break;
            }
        }

        // Save leaderboard
        for (int i = 0; i < MaxScores; i++)
        {
            PlayerPrefs.SetInt("Score" + i, scores[i]);
            PlayerPrefs.SetString("Name" + i, names[i]);
        }

        PlayerPrefs.Save();
    }

    public static string GetLeaderboard()
    {
        string leaderboard = "";

        for (int i = 0; i < MaxScores; i++)
        {
            int score = PlayerPrefs.GetInt("Score" + i, 0);
            string name = PlayerPrefs.GetString("Name" + i, "");

            if (name != "")
            {
                leaderboard += (i + 1) + ". " +
                    name + " - " + score + " units\n";
            }
        }

        return leaderboard;
    }
}

