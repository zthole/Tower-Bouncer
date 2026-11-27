using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [Header("Player Name")]
    public InputField playerNameInput;

    [Header("Scene Settings")]
    public string gameSceneName = "SampleScene";

    void Start()
    {
        // Load previously saved player name
        if (playerNameInput != null)
        {
            playerNameInput.text =
            PlayerPrefs.GetString("PlayerName", "");
        }
    }

    public void StartGame()
    {
        string playerName = "Player";

        // Get player name if an input field is assigned
        if (playerNameInput != null)
        {
            playerName = playerNameInput.text.Trim();
        }

        // Use default name if nothing is entered
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Player";
        }

        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        SceneManager.LoadScene(gameSceneName);
    }
}
