using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatUpUI : MonoBehaviour
{
  Vector2 startPos;
  RectTransform rectTransform;
  Vector2 offset = new Vector2(0, 30);

  // Start is called before the first frame update
  void Start()
  {
    rectTransform = GetComponent<RectTransform>();
    startPos = rectTransform.anchoredPosition; // Store original position of the UI
    rectTransform.anchoredPosition -= offset; // Move UI a set amount
    StartCoroutine("FloatUp");
  }

  IEnumerator FloatUp()
  {
    // Move the UI towards the original position
    while(rectTransform.anchoredPosition != startPos)
    {
      rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, startPos, 2f);
      yield return null;
    }
  }
}
