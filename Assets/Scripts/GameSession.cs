using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    void Awake()
    {
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;

        if (numberOfGameSessions > 1)
        {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }

    }

    void Start() {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

   public void ProcessPlayerDeath()
   {
       if (GameManager.gameManager.playerHealth.Health == 0)
       {
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentLevel + 1);
       } else {
            ResetGameSession();
       }
   }

    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    void TakeLife()
    {
        playerLives--;
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void PointIncrease(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }
}