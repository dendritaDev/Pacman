using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager sharedInstance;

    public TMP_Text titleLabel;
    public TMP_Text scoreLabel;

    private int totalScore;

    private void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        totalScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
     
        if(GameManager.sharedInstance.gamePaused || !GameManager.sharedInstance.gameStarted)
        {
            titleLabel.enabled = true;
        }
        else
        {
            titleLabel.enabled = false;
        }
    }

    public void ScorePoints(int points)
    {
        totalScore += points;
        scoreLabel.text = "Score : " + totalScore;
    }
}
