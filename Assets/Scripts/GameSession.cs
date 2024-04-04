using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    int coinAmount = 0;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoresText;
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        livesText.text = playerLives.ToString();
        scoresText.text = (coinAmount*100).ToString();
    }

    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    private void TakeLife()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevelIndex);
        playerLives--;
        livesText.text = playerLives.ToString();

    }

    private void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().SelfDestruct();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void increaseCoin()
    {
        coinAmount++;
        scoresText.text = (coinAmount*100).ToString();
    }

    public int getCoin()
    {
        return coinAmount;
    }
}
