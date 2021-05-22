using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
  private Animator anim;
  private CustomerWaypoint waypoint;
  private GameManager score;
  private float maxSatisfaction = 100f;
  private float decreaseAmount = 2f; // How fast satisfaction drains
  private float minDistance = 10f;
  private float currentSatisfaction;
  private float satisfactionMultiplier;
  private bool isFoodDelivered; // Flag used to stop decreasing satisfaction
  private Color orange = new Color(1f, .64f, 0f);
  private BoxCollider boxCollider;
  private AudioSource audioSource;
  private GameObject player;
  private GameObject satisfaction;
  private PlayerController playerController;

  public SpriteRenderer minimapIcon;
  public Image satisfactionBar;
  public Slider overallSatisfactionSlider;
  public AudioClip nomClip;
  public AudioClip annoyedClip;
  public ParticleSystem crumbParticle;

  // Start is called before the first frame update
  void Start()
  {
    // Initializing stuff
    player = GameObject.FindGameObjectWithTag("Player");
    playerController = player.GetComponent<PlayerController>();
    anim = GetComponent<Animator>();
    waypoint = GetComponent<CustomerWaypoint>();
    boxCollider = GetComponent<BoxCollider>();
    score = GameObject.Find("GameManager").GetComponent<GameManager>();
    satisfaction = GameObject.FindGameObjectWithTag("OverallSatisfaction");
    overallSatisfactionSlider = satisfaction.GetComponent<Slider>();
    audioSource = GetComponent<AudioSource>();
    currentSatisfaction = maxSatisfaction;
  }

  // Update is called once per frame
  void Update()
  {
    float temp = overallSatisfactionSlider.value;

    SatisfactionDecrease();
    ChangeSatisfactionBarColor(satisfactionBar);

    if (currentSatisfaction < 0)
    {   
      // Subtract from overall satisfaction
      currentSatisfaction = 0;
      audioSource.PlayOneShot(annoyedClip);
      temp -= (.05f * score.GetDifficulty());
      overallSatisfactionSlider.value = temp;
      anim.SetBool("CustomerPatience", true);
      Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
    }
    LookAtPlayer();
  }

  private void OnTriggerEnter(Collider other)
  {
    // Detects when food is shot at the customer
    if(other.CompareTag("Food"))
    {
      isFoodDelivered = true;

      // Do stuff when food is delivered to the customer
      minimapIcon.GetComponent<SpriteRenderer>().enabled = false;
      audioSource.PlayOneShot(nomClip);
      crumbParticle.Play();
      boxCollider.enabled = false;
      Destroy(other.gameObject);
      anim.SetBool("isFoodDelivered", true);
      CalculateScore();
      Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
    }
  }

  public void ChangeSatisfactionBarColor(Image img)
  {
    // Change the color of the fill bar depending on current amount
    // Also works as determining score multiplier
    if(currentSatisfaction >= 75f)
    {
      img.color = Color.green;
      satisfactionMultiplier = 1f;
    }
    else if(currentSatisfaction < 75f && currentSatisfaction >= 50f)
    {
      img.color = Color.yellow;
      satisfactionMultiplier = .75f;
    }
    else if (currentSatisfaction < 50f && currentSatisfaction >= 25f)
    {
      img.color = orange;
      satisfactionMultiplier = .5f;
    } 
    else if(currentSatisfaction < 25f)
    {
      img.color = Color.red;
      satisfactionMultiplier = .25f;
    }
  }

  private void OnDestroy()
  {
    // Destroy the waypoint pointing to the customer at the same time as the customer
    Destroy(waypoint.GetWaypointInstance());
  }

  private void SatisfactionDecrease()
  {
    // Decrease customers satisfaction
    if(!playerController.HasPowerUp() && !score.GetIfGamePaused()) {
      if (!isFoodDelivered && currentSatisfaction > 0) {
        currentSatisfaction -= (decreaseAmount + score.GetDifficulty() - 1) * Time.deltaTime;
        satisfactionBar.fillAmount = currentSatisfaction / maxSatisfaction;
      }
    }
  }

  private void LookAtPlayer()
  {
    // Face the player when they are within a certain distance
    float distance = Vector3.Distance(player.transform.position, transform.position);
    if (distance < minDistance)
    {
      transform.LookAt(player.transform.position);
    }
  }

  public float GetCurrentSatisfaction()
  {
    // Used for waypoint
    return currentSatisfaction;
  }

  private void CalculateScore()
  {
    score.AddToScore(Mathf.RoundToInt(currentSatisfaction * satisfactionMultiplier * score.GetDifficulty()));
  }

  public bool GetIsFoodDelivered()
  {
    return isFoodDelivered;
  }
}
