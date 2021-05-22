using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillAmmo : MonoBehaviour
{
  private const int MAX_FOOD_AMMO = 10;
  private PlayerController playerController;
  private AudioSource audioSource;

  public AudioClip refillClip;

  // Start is called before the first frame update
  void Start()
  {
    playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    audioSource = GetComponent<AudioSource>();
  }

  private void OnTriggerEnter(Collider other)
  {
    // Refill ammo when player drives over the zone
    if(other.tag == "Player")
    {
      if(playerController.foodAmmo != MAX_FOOD_AMMO)
      {
        playerController.foodAmmo = MAX_FOOD_AMMO;
        audioSource.PlayOneShot(refillClip);
      }
    }
  }
}
