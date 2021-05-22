using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
  //5/15/21 - removed SpawnManager, GameOver and ScoreManager scripts and put all the code in here

  // GameOver variables
  private bool isGameOver;
  private PlayerController player;
  private int scoreThreshold = 250;
  private int thresholdIncrease = 250;
  private int difficultyCounter = 1;
  private bool isGamePaused = false;
  public GameObject gameOverUI;
  public GameObject pauseScreen;
  public AudioClip gameOverMusic;

  // SpawnManager variables
  public float spawnXRange = 7f;
  public float spawnZRange = 7f;
  private float startTime = 1.5f;
  private float minRepeatTime = 8f;
  private float maxRepeatTime = 16f;
  private float powerUpSpawnTime = 10f;
  private float randRepeatTime;
  private bool decreasedSpawnTime = false; // Flag to only decrease spawn time only once a round
  private int maxPowerUps = 3;
  private int powerUpCount = 0;

  public GameObject[] customerPrefabs;
  public GameObject powerUpPrefab;
  public Transform[] powerUpSpawnPoints;
  public Transform[] spawnPoints;
  public AudioSource engine;

  // ScoreManager variables
  public int score;
  public TextMeshProUGUI scoreText;
  public Slider satisfactionGauge;

  // other variables
  public AudioSource source;
  
  public AudioClip gameMusic;
  public AudioClip pauseSound;

  public LayerMask layerMask;

  // Start is called before the first frame update
  void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    //spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
    source = GetComponent<AudioSource>();
    source.clip = gameMusic;
    source.Play();
    
    randRepeatTime = Random.Range(minRepeatTime, maxRepeatTime);
    InvokeRepeating("SpawnCustomers", startTime, randRepeatTime);
    InvokeRepeating("SpawnPowerUps", powerUpSpawnTime, powerUpSpawnTime);
    
    score = 0;
    scoreText.text = "Score: " + score.ToString();
  }

  // Update is called once per frame
  void Update()
  {
    ChangeMusic();
    CheckIfGameIsOver();
    UpdateDifficulty();
    PauseOrResumeGame();
    SpeedUpTimeBasedOnDifficulty();

    scoreText.text = "Score: " + score.ToString(); // Update the score
  }

  private void SpawnCustomers() {
    // Spawn a customer within a set area at a random location
    float randXSpawnPos = Random.Range(-spawnXRange, spawnXRange);
    float randZSpawnPos = Random.Range(-spawnZRange, spawnZRange);

    // Create random index based on length of array
    int customerIndex = Random.Range(0, customerPrefabs.Length);
    int spawnPointIndex = Random.Range(0, spawnPoints.Length);

    // Create a random position to spawn customers within an area of the spawnpoint
    Vector3 randPos = new Vector3(randXSpawnPos + spawnPoints[spawnPointIndex].position.x,
                                  customerPrefabs[customerIndex].transform.rotation.y,
                                  randZSpawnPos + spawnPoints[spawnPointIndex].position.z);

    if(DetectIfOkToSpawn(randPos) == 0) {
      Instantiate(customerPrefabs[customerIndex], randPos, customerPrefabs[customerIndex].transform.rotation);
    } else if (DetectIfOkToSpawn(randPos) != 0) {
      while(DetectIfOkToSpawn(randPos) != 0) {
        randXSpawnPos = Random.Range(-spawnXRange, spawnXRange);
        randZSpawnPos = Random.Range(-spawnZRange, spawnZRange);
        spawnPointIndex = Random.Range(0, spawnPoints.Length);
        randPos = new Vector3(randXSpawnPos + spawnPoints[spawnPointIndex].position.x,
                                  customerPrefabs[customerIndex].transform.rotation.y,
                                  randZSpawnPos + spawnPoints[spawnPointIndex].position.z);
        Debug.Log("checking");
      }
      Instantiate(customerPrefabs[customerIndex], randPos, customerPrefabs[customerIndex].transform.rotation);
    }
  }

  void ChangeMusic()
  {
    if(isGameOver)
    {
      // Change to game over music
      if (source.clip == gameMusic)
      {
        source.clip = gameOverMusic;
        source.Play();
      }
    }
  }

  void CheckIfGameIsOver()
  {
    // When the satisfaction gauge reaches 0 end the game!!
    if (satisfactionGauge.value == 0)
    {
      isGameOver = true;
      engine.Pause();
      player.enabled = false;
      CancelInvoke();
      gameOverUI.SetActive(true);
    }
  }
  void UpdateDifficulty()
  {
    // Increase difficulty based on score
    if (score >= scoreThreshold)
    {
      scoreThreshold += thresholdIncrease * difficultyCounter;
      difficultyCounter += 1;
    }
  }

  void PauseOrResumeGame()
  {
    // Press ESC to pause the game
    if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
    {
      source.Stop();
      engine.Pause();
      source.PlayOneShot(pauseSound);
      if (isGamePaused == true) 
      {
        // Re-enable controls and spawning
        isGamePaused = false;
        source.clip = gameMusic;
        source.Play();
        source.loop = true;
        player.enabled = true;
        pauseScreen.SetActive(false);
        if (!IsInvoking())
        {
          Resume();
        }
      }
      else if(isGamePaused == false)
      {
        // Halt spawning and controls
        isGamePaused = true;
        player.enabled = false;
        CancelInvoke();
        pauseScreen.SetActive(true);
      }
    } 
  }
  
  private void SpeedUpTimeBasedOnDifficulty() {
    // Decrease spawn time every other difficulty increase
    if (GetDifficulty() % 2 == 0 && !decreasedSpawnTime) {
      minRepeatTime -= 1;
      maxRepeatTime -= 1;
      randRepeatTime = Random.Range(minRepeatTime, maxRepeatTime);
      decreasedSpawnTime = true;
    }
    if (GetDifficulty() % 2 != 0) {
      decreasedSpawnTime = false;
    }
  }

  private void SpawnPowerUps() {
    // Only 3 Power ups will spawn at on of 5 set locations
    int randSpawnIndex = Random.Range(0, powerUpSpawnPoints.Length);
    Vector3 randPos = new Vector3(powerUpSpawnPoints[randSpawnIndex].position.x,
                                  powerUpSpawnPoints[randSpawnIndex].position.y,
                                  powerUpSpawnPoints[randSpawnIndex].position.z);



    if (powerUpCount < maxPowerUps) 
    {
      while(DetectIfOkToSpawn(randPos) != 0) 
      {
        randPos = new Vector3(powerUpSpawnPoints[randSpawnIndex].position.x,
                                    powerUpSpawnPoints[randSpawnIndex].position.y,
                                    powerUpSpawnPoints[randSpawnIndex].position.z);
      }
      Instantiate(powerUpPrefab, randPos, powerUpPrefab.transform.rotation);
      powerUpCount++;
    }
  }

  public void SubtractPowerUpCount()
  {
    powerUpCount--;
  }

  void Resume()
  {
    InvokeRepeating("SpawnCustomers", 5f, randRepeatTime);
    InvokeRepeating("SpawnPowerUps", powerUpSpawnTime, powerUpSpawnTime);
  }
  

  public void SetIsGameOver(bool value)
  {
    isGameOver = value;
  }

  public void AddToScore(int value)
  {
    score += value;
  }

  public int GetDifficulty()
  {
    return difficultyCounter;
  }

  public bool GetIfGamePaused()
  {
    return isGamePaused;
  }

  public bool GetIfGameOver() {
    return isGameOver;
  }

  private int DetectIfOkToSpawn(Vector3 pos)
  {
    Collider[] hit = Physics.OverlapSphere(pos, .5f, layerMask);
    return hit.Length;
  }
}
