using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
  [SerializeField] float playerSpeed = 10.0f;
  [SerializeField] float turnSpeed = 60.0f;
  [SerializeField] float cannonRotateSpeed = 250f;

  public GameObject cannon;
  public GameObject newCenterOfMass;
  public Transform projectileSpawn;
  public GameObject projectilePrefab;
  public int foodAmmo = 10;

  private float horizontalInput;
  private float verticalInput;
  
  private int cannonRotateDirection = 1;

  // Start is called before the first frame update
  void Start()
  {
    // Make center of mass lower to try and prevent flipping the car over
    GetComponent<Rigidbody>().centerOfMass = newCenterOfMass.transform.localPosition;
  }

  // Update is called once per frame
  void Update()
  {
    // ---- Input Stuff ----
    // Note: unmapped arrow keys for use of cannon rotation
    // Use W,S to move and A,D to turn
    horizontalInput = Input.GetAxis("Horizontal");
    verticalInput = Input.GetAxis("Vertical");

    // Note: unmapped "Jump" to utilize use for spacebar
    if(Input.GetButtonDown("Fire1"))
    {
      Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.transform.rotation);
      if(foodAmmo > 0) {
        foodAmmo -= 1;
      }     
    }

    // Cannon rotation using left and right arrow keys
    if(Input.GetKey(KeyCode.LeftArrow))
    {
      cannon.transform.Rotate(Vector3.up * cannonRotateSpeed * -cannonRotateDirection * Time.deltaTime);
    } 
    else if(Input.GetKey(KeyCode.RightArrow)) 
    {
      cannon.transform.Rotate(Vector3.up * cannonRotateSpeed * cannonRotateDirection * Time.deltaTime);
    }

    // Move and rotate the player
    transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime * verticalInput);
    transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime * horizontalInput);  
  }
}
