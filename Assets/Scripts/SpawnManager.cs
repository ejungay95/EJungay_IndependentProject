using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
  public float spawnXRange = 2f;
  public float spawnZRange = 2f;
  private float startTime = 1.5f;
  private float minRepeatTime = 8f;
  private float maxRepeatTime = 16f;
  private float randRepeatTime;
  private bool decreasedSpawnTime = false; // Flag to only decrease spawn time only once a round

  public GameObject[] customerPrefabs;
  public Transform[] spawnPoints;
  public ScoreManager scoreManager;

  // Start is called before the first frame update
  void Start()
  {
    randRepeatTime = Random.Range(minRepeatTime, maxRepeatTime);
    InvokeRepeating("SpawnCustomers", startTime, randRepeatTime);
  }

  // Update is called once per frame
  void Update()
  {
    SpeedUpTimeBasedOnDifficulty(); 
  }

  private void SpawnCustomers()
  {
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
    Instantiate(customerPrefabs[customerIndex], randPos, customerPrefabs[customerIndex].transform.rotation);
  }

  private void SpeedUpTimeBasedOnDifficulty()
  {
    // Decrease spawn time every other difficulty increase
    if(scoreManager.GetDifficulty() % 2 == 0 && !decreasedSpawnTime)
    {
      minRepeatTime -= 1;
      maxRepeatTime -= 1;
      randRepeatTime = Random.Range(minRepeatTime, maxRepeatTime);
      decreasedSpawnTime = true;
      Debug.Log("even" + minRepeatTime + " " + maxRepeatTime);
    }
    if(scoreManager.GetDifficulty() % 2 != 0)
    {
      decreasedSpawnTime = false;
      Debug.Log("odd" +  minRepeatTime + " " + maxRepeatTime);
    }

  }
}
