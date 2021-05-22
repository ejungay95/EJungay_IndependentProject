using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
  private GameManager gameController;
  private GameObject creditScreen;
  private GameObject titleScreen;

  private void Start()
  {
    gameController = GameObject.Find("GameManager").GetComponent<GameManager>();
    creditScreen = GameObject.Find("Credit Screen");
    titleScreen = GameObject.Find("Title Screen");
    if (creditScreen != null)
    {
      creditScreen.SetActive(false);
    }
    if (titleScreen != null)
    {
      titleScreen.SetActive(true);
    }
  }
  public void ResetScene()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    if(gameController != null)
    {
      gameController.SetIsGameOver(false);
    }
  }

  public void LoadMainLevel()
  {
    SceneManager.LoadScene("MAIN_GameScene");
  }

  public void LoadMainMenu()
  {
    SceneManager.LoadScene("MainMenu");
  }

  public void ShowHideCredits()
  {
    if (creditScreen != null)
    {
      if(!creditScreen.activeSelf)
      {
        creditScreen.SetActive(true);
        titleScreen.SetActive(false);
      }
      else if(creditScreen.activeSelf)
      {
        creditScreen.SetActive(false);
        titleScreen.SetActive(true);
      }
    }
  }

  public void ExitGame()
  {
    Application.Quit();
  }
}
