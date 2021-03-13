using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
  private Animator anim;
  private CustomerWaypoint waypoint;
  private float maxSatisfaction = 100f;
  private float decreaseAmount = 5f; // How fast satisfaction drains
  private float minDistance = 10f;
  private float currentSatisfaction;
  private bool isFoodDelivered; // Flag used to stop decreasing satisfaction
  private Color orange = new Color(1f, .64f, 0f);

  private GameObject player;
  public Image satisfactionBar;


  // Start is called before the first frame update
  void Start()
  {
    // Initializing stuff
    player = GameObject.FindGameObjectWithTag("Player");
    anim = GetComponent<Animator>();
    waypoint = GetComponent<CustomerWaypoint>();
    currentSatisfaction = maxSatisfaction;
  }

  // Update is called once per frame
  void Update()
  {
    SatisfactionDecrease();
    ChangeSatisfactionBarColor(satisfactionBar);
    if (currentSatisfaction < 0)
    {
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

      Destroy(other.gameObject);
      anim.SetBool("isFoodDelivered", true);
      Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
    }
  }

  public void ChangeSatisfactionBarColor(Image img)
  {
    // Change the color of the fill bar depending on current amount
    if(currentSatisfaction >= 75f)
    {
      img.color = Color.green;
    }
    else if(currentSatisfaction < 75f && currentSatisfaction >= 50f)
    {
      img.color = Color.yellow;
    }
    else if (currentSatisfaction < 50f && currentSatisfaction >= 25f)
    {
      img.color = orange;
    } 
    else if(currentSatisfaction < 25f)
    {
      img.color = Color.red;
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
    if (!isFoodDelivered && currentSatisfaction > 0)
    {
      currentSatisfaction -= decreaseAmount * Time.deltaTime;
      satisfactionBar.fillAmount = currentSatisfaction / maxSatisfaction;
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
    // Might use for later when calculating score
    return currentSatisfaction;
  }
}
