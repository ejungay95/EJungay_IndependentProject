using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerWaypoint : MonoBehaviour
{
  private float minDistance = 5f;
  private float imageSize = 50f;
  private Vector2 screenMax = new Vector2(Screen.width, Screen.height);
  private Vector2 screenMin = Vector2.zero;
  private CustomerController customerController;

  // Reference to instantiated waypoint so I can do stuff to it
  private RectTransform temp;
  private Image tempImage;
  private Text tempText;

  private Canvas canvas;
  private Transform player;
  public RectTransform prefab;

  // Start is called before the first frame update
  void Start()
  {
    // Initializing stuff
    player = GameObject.FindGameObjectWithTag("Player").transform;
    canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
    customerController = GetComponent<CustomerController>();
    temp = Instantiate(prefab, canvas.transform);
    temp.position = Camera.main.WorldToScreenPoint(transform.position); // Place waypoint above customer
    tempImage = temp.GetComponentInChildren<Image>();
    tempText = temp.GetComponentInChildren<Text>();
  }

  // Update is called once per frame
  void Update()
  {
    // Do waypoint manipulation when its still exists
    if(temp != null)
    {
      WaypointDistanceChanges();
      MoveAndRotateWaypoint();
      CheckIfWaypointOutOfScreen();

      // Change distance text
      tempText.text = Mathf.Round(Vector3.Distance(player.position, transform.position)).ToString() + "m";
      customerController.ChangeSatisfactionBarColor(tempImage);

      if(customerController.GetCurrentSatisfaction() <= 0) {
        temp.gameObject.SetActive(false);
      }
    }
  }

  private void WaypointDistanceChanges()
  {
    // Disable the waypoint when the player is Close to see object
    float distance = Vector3.Distance(player.position, transform.position);

    if (distance < minDistance)
    {
      temp.gameObject.SetActive(false);
    } 
    else 
    {
      temp.gameObject.SetActive(true);

      // Change size of the waypoint, bigger when farther, smaller when closer
      tempImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, distance + imageSize);
      tempImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, distance + imageSize);
      //tempImage.rectTransform.sizeDelta = new Vector2(distance + imageSize, distance + imageSize);
    }
  }

  private void CheckIfWaypointOutOfScreen()
  {
    // Restrict UI image within the camera view
    if(temp.position.x > screenMax.x)
    {
      temp.position = new Vector2(screenMax.x - prefab.rect.width / 2, temp.position.y);
    } 
    else if (temp.position.x < screenMin.x)
    {
      temp.position = new Vector2(screenMin.x + prefab.rect.width / 2, temp.position.y);
    }
    if (temp.position.y > screenMax.y)
    {
      temp.position = new Vector2(temp.position.x, screenMax.y - prefab.rect.height / 2);
    } 
    else if (temp.position.y < screenMin.y) 
    {
      temp.position = new Vector2(temp.position.x, screenMin.y + prefab.rect.height / 2);
    }
  }

  private void MoveAndRotateWaypoint()
  {
    // The distance between the center of the screen and the waypoint
    Vector3 difference = new Vector3(screenMax.x / 2 - temp.transform.position.x, screenMax.y / 2 - temp.transform.position.y, temp.transform.position.z);

    // Rotate the waypoint to face towards the customer
    temp.transform.rotation = Quaternion.Euler(0f, 0f, (Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg) - 90);
    temp.position = Camera.main.WorldToScreenPoint(transform.position); // Place waypoint above customer
  }

  public GameObject GetWaypointInstance()
  {
    // Get the instantiated gameobject
    // Used to destroy temp when customer object is destroyed
    if(temp != null)    
    {
      return temp.gameObject;
    }
    return null;
  }

  public float SubtractFromSatisfactionRating()
  {
    // Use for later
    return 0;
  }

  public float PointsToAddBasedOnRemainingSatisfaction()
  {
    // Use for later
    return 0;
  }
}
