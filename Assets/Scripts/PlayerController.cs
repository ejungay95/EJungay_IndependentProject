using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
  public float playerSpeed = 0f;
  public float turnSpeed = 200f;
  public float cannonRotateSpeed = 250f;

  public float playerBoostSpeed = 3000f;
  public float playerNormalSpeed = 2500f;

  private bool isBoosting = false;

  public GameObject cannon;
  public GameObject newCenterOfMass;
  public Transform projectileSpawn;
  public GameObject projectilePrefab;
  public AudioClip cannonShotClip;
  public AudioClip emptyShotClip;
  public AudioClip powerUpClip;
  public AudioClip tickTockClip;
  public AudioClip engineClip;
  public int foodAmmo = 10;
  public ParticleSystem smoke;
  public Text ammoCountText;

  private AudioSource audioSource;
  private AudioSource rbAudioSource;
  private float horizontalInput;
  private float verticalInput;
  private bool hasPowerUp;
  private int cannonRotateDirection = 1;
  private ParticleSystem temp;
  public Rigidbody rb;

  public ParticleSystem[] carTrail;
  public Slider boostGauge;

  private bool coroutineIsRunning;

  // Start is called before the first frame update
  void Start()
  {
    playerSpeed = playerNormalSpeed;
    rbAudioSource = rb.gameObject.GetComponent<AudioSource>();
    // unparent the sphere for better control
    rb.transform.parent = null;
    audioSource = GetComponent<AudioSource>();
    ammoCountText.text = "Ammo: " + foodAmmo.ToString() + "/10";
  }

  // Update is called once per frame
  void Update()
  {
    // ---- Input Stuff ----
    // Note: unmapped arrow keys for use of cannon rotation
    // Use W,S to move and A,D to turn
    horizontalInput = Input.GetAxis("Horizontal");
    verticalInput = Input.GetAxis("Vertical");

    EngineSound();

    // Note: unmapped "Jump" to utilize use for spacebar
    // Fire the projectile
    if (Input.GetButtonDown("Fire1") && foodAmmo > 0)
    {
      audioSource.PlayOneShot(cannonShotClip);
      Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.transform.rotation);
      temp = Instantiate(smoke, projectileSpawn.position, projectileSpawn.transform.rotation);
      temp.Play();
      
      if(foodAmmo > 0) {
        foodAmmo -= 1;
      }
    }

    if (Input.GetButtonDown("Fire1") && foodAmmo == 0)
    {
      audioSource.PlayOneShot(emptyShotClip);
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

    if (Input.GetKey(KeyCode.LeftShift)) {
      
      isBoosting = true;
    } else {
      StartCoroutine("RefillBoostGauge");
      isBoosting = false;
    }
    Boost();
    DecreaseBoostGauge();

    // Can also use mouse to aim the cannon
    Vector3 pos = Camera.main.WorldToScreenPoint(cannon.transform.position);
    Vector3 dir = Input.mousePosition - pos;
    float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90f;
    cannon.transform.rotation = Quaternion.AngleAxis(-angle, Vector3.up);

    //transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime * verticalInput);
    // 5/15/21 - Now the player can only turn while the car is moving forward
    transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime * horizontalInput * verticalInput);

    // Match the transform of this gameObject with the sphere that we are moving
    transform.position = new Vector3(rb.transform.position.x, rb.transform.position.y - rb.GetComponent<SphereCollider>().radius, rb.transform.position.z);

    ammoCountText.text = foodAmmo.ToString() + "/10";
  }

  private void FixedUpdate() {
    // Using a sphere instead of transform.translate for better collision between colliders
    // tried using a box collider but it wasn't able to go over the sidewalk

    // Move the sphere
    rb.AddForce(transform.forward * verticalInput * playerSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
  }

  private void EngineSound() {
    if(rb.velocity.magnitude >= 2) {
      rbAudioSource.clip = engineClip;
      if(!rbAudioSource.isPlaying) {
        rbAudioSource.Play();
        rbAudioSource.loop = true;
      }
      // play trail when moving
      foreach(ParticleSystem trail in carTrail) {
        if(!trail.isEmitting) {
          trail.Play();
        }
      }
    } else if(rb.velocity.magnitude < 2) {
      rbAudioSource.Pause();
      rbAudioSource.loop = false;

      // Stop emitting the trail when stationary
      foreach (ParticleSystem trail in carTrail) {
        if (trail.isEmitting) {
          trail.Stop();
        }
      }
    }
  }

  public bool HasPowerUp() {
    // Use to stop/start satisfaction decrease
    return hasPowerUp;
  }

  public bool IsBoosting() {
    // Use to stop/start satisfaction decrease
    return isBoosting;
  }

  public void SetHasPowerUp(bool value) {
    // Use to stop/start satisfaction decrease
    hasPowerUp = value;
  }

  private void Boost() {
    if(isBoosting && boostGauge.value > 0) {
      playerSpeed = playerBoostSpeed;
    } else {
      playerSpeed = playerNormalSpeed;
    }

    if(boostGauge.value == 0) {
      isBoosting = false;
    }
  }

  private void DecreaseBoostGauge() {
    if(isBoosting && boostGauge.value > 0) {
      boostGauge.value -= 0.5f * Time.deltaTime;
    }
  }

  IEnumerator RefillBoostGauge() {
    if (coroutineIsRunning) {
      // flag to only execute coroutine once
      yield break;
    }
    coroutineIsRunning = true;
    yield return new WaitForSeconds(5f);

    while(boostGauge.value < 1 && isBoosting == false) {
      boostGauge.value += .2f * Time.deltaTime;
      yield return null;
    }

    coroutineIsRunning = false;
    yield return 0;
  }
}
