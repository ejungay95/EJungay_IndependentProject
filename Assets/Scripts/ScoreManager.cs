using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
  public int score;
  private int scoreThreshold = 250;
  private int thresholdIncrease = 250;
  private int difficultyCounter = 1;
  public Text scoreText;
  

  // Start is called before the first frame update
  void Start()
  {
    score = 0;
    scoreText.text = "Score: " + score.ToString();
  }

  // Update is called once per frame
  void Update()
  {
    scoreText.text = "Score: " + score.ToString();

    if(score >= scoreThreshold)
    {
      scoreThreshold += thresholdIncrease * difficultyCounter;
      difficultyCounter += 1;
    }
  }

  public void AddToScore(int value)
  {
    score += value;
  }

  public int GetDifficulty()
  {
    return difficultyCounter;
  }
}
