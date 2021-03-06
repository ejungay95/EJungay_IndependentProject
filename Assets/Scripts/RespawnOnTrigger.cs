﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnOnTrigger : MonoBehaviour
{
  public Transform respawnPoint;

  private void OnTriggerEnter(Collider other)
  {
    // In the case when truck clips under the play area
    // Happened a few times during testing
    if (other.gameObject.CompareTag("Player"))
    {
      other.gameObject.transform.position = respawnPoint.transform.position;
    }
  }
}
