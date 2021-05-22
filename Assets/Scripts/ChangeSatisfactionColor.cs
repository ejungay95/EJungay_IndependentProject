using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSatisfactionColor : MonoBehaviour
{
  public Image fillColor;
  public Image handle;
  public Slider satisfactionSlider;
  public Sprite[] handleImages;

  private Color orange = new Color(1f, .64f, 0f);
  // Start is called before the first frame update
  void Start()
  {
        
  }

  // Update is called once per frame
  void Update()
  {
    ChangeOverallSatisfactionStatus();
  }

  void ChangeOverallSatisfactionStatus()
  {
    int index = 0;

    if (satisfactionSlider.value >= .75f) {
      fillColor.color = Color.green;
      handle.sprite = handleImages[index];
    } else if (satisfactionSlider.value < .75f && satisfactionSlider.value >= .50f) {
      fillColor.color = Color.yellow;
      handle.sprite = handleImages[index + 1];
    } else if (satisfactionSlider.value < .50f && satisfactionSlider.value >= .25f) {
      fillColor.color = orange;
      handle.sprite = handleImages[index + 2];
    } else if (satisfactionSlider.value < .25f) {
      fillColor.color = Color.red;
      handle.sprite = handleImages[index + 3];
    }
  }
}
