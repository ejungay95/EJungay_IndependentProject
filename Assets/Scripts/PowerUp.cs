using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
  private PlayerController playerController;
  private AudioSource audioSource;
  private GameManager gameManager;

  public GameObject icon;
  public AudioClip powerUpClip;
  public AudioClip tickTockClip;

  private void Start() {
    playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    audioSource = GetComponent<AudioSource>();
  }

  private void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Player")) {
      // Enable the power up effect and its effects
      // Freeze time for 7 seconds because the clip is 7 seconds long
      audioSource.PlayOneShot(powerUpClip);   
      gameManager.SubtractPowerUpCount();
      audioSource.PlayOneShot(tickTockClip);
      StartCoroutine("PowerUpTimer");
    }
  }
  IEnumerator PowerUpTimer() {
    // Make power up last 7 seconds
    MeshRenderer[] meshRenderers = new MeshRenderer[4];
    gameObject.GetComponent<BoxCollider>().enabled = false;
    meshRenderers = GetComponentsInChildren<MeshRenderer>();
    foreach(MeshRenderer mesh in meshRenderers) {
      mesh.enabled = false;
    }
    icon.GetComponent<SpriteRenderer>().enabled = false;
    playerController.SetHasPowerUp(true);
    yield return new WaitForSeconds(7);
    playerController.SetHasPowerUp(false);
    Destroy(gameObject);
  }
}
