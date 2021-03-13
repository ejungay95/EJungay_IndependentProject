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

  public GameObject[] customerPrefabs;
  public Transform[] spawnPoints;

  // Start is called before the first frame update
  void Start()
  {
    float randRepeatTime = Random.Range(minRepeatTime, maxRepeatTime);
    InvokeRepeating("SpawnCustomers", startTime, randRepeatTime);
  }

  // Update is called once per frame
  void Update()
  {
    
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

  private float SpeedUpTimeBasedOnScore(float time)
  {
    // Use for later. Figure out how to decrease repeat times based on current score
    return 0;
  }
}
