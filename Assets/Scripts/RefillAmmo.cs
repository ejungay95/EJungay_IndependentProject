using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillAmmo : MonoBehaviour {
  private const int MAX_FOOD_AMMO = 10;
  private PlayerController playerController;

  // Start is called before the first frame update
  void Start() {
    playerController = gameObject.GetComponent<PlayerController>();
  }

  private void OnTriggerEnter(Collider other) {
    if(other.tag == "RefillZone") {
      if(playerController.foodAmmo != MAX_FOOD_AMMO) {
        playerController.foodAmmo = MAX_FOOD_AMMO;
      }
    }
  }
}
