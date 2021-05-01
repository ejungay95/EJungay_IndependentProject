using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInUI : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    // Determine what UI element to fade in
    if (gameObject.CompareTag("Button"))
    {
      Image img = GetComponent<Image>();
      Text text = img.GetComponentInChildren<Text>();
      StartCoroutine(FadeInButton(img, text));
    }
    if (gameObject.CompareTag("Text"))
    {
      Text text = GetComponent<Text>();
      StartCoroutine(FadeInText(text));
    }
  }

  IEnumerator FadeInButton(Image img, Text text) {
    while (img.color.a < 1.0f && text.color.a < 1.0f) {
      // Fade in button
      img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a + (Time.deltaTime / .8f));
      text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / .8f));
      yield return null;
    }
  }

  IEnumerator FadeInText(Text text)
  {  
    while(text.color.a < 1.0f)
    {
      // Fade in text
      text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / .8f));
      yield return null;
    }
  }
}
