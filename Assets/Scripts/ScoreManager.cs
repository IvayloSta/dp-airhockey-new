using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int playerScore = 0;
    public int enemyScore = 0;

    public TMP_Text playerScoreText;
    public TMP_Text enemyScoreText;


    public static ScoreManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayerScores()
    {
        playerScore++;
        UpdateUI();
    }

    public void EnemyScores()
    {
        enemyScore++;
        UpdateUI();
    }

    void UpdateUI()
    {
        playerScoreText.text = playerScore.ToString();
        enemyScoreText.text = enemyScore.ToString();
    }
}

