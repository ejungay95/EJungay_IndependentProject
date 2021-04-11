using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
  private GameOver gameOverController;

  private void Start() {
    gameOverController = GetComponent<GameOver>();
  }
  public void ResetScene()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    gameOverController.SetIsGameOver(false);
  }

  public void ExitGame()
  {
    Application.Quit();
  }
}
