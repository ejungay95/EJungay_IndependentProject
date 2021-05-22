using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
  public Vector3 offset;
  private Vector3 boostOffset = new Vector3(0f, 5f, 0f);

  private PlayerController playerController;

  public GameObject player;
  // Start is called before the first frame update
  void Start()
  {
    playerController = GameObject.Find("Player").GetComponent<PlayerController>();
  }

  // Update is called once per frame
  void LateUpdate()
  {
    if(playerController.IsBoosting()) {
      transform.position = player.transform.position + offset + boostOffset;
    } else {     
      transform.position = transform.position = player.transform.position + offset; ;
    }  
  }
}
