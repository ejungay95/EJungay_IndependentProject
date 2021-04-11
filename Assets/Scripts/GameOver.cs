using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
  private bool isGameOver;

  public PlayerController player;
  public SpawnManager spawnManager;
  public Slider satisfactionGauge;

  public GameObject gameOverUI;
  public AudioSource source;
  public AudioClip gameOverMusic;
  public AudioClip gameMusic;

  // Start is called before the first frame update
  void Start()
  {
    gameOverUI.SetActive(false);
    player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
    source = GetComponent<AudioSource>();
    source.clip = gameMusic;
    source.Play();
  }

  // Update is called once per frame
  void Update()
  {
    ChangeMusic();
    if(satisfactionGauge.value == 0)
    {
      isGameOver = true;
      player.enabled = false;
      spawnManager.CancelInvoke();
      spawnManager.enabled = false;
      gameOverUI.SetActive(true);
    }
  }

  void ChangeMusic()
  {
    if(isGameOver)
    {
      if(source.clip == gameMusic)
      {
        source.clip = gameOverMusic;
        source.Play();
      }
    }
  }

  public void SetIsGameOver(bool value)
  {
    isGameOver = value;
  }
}
