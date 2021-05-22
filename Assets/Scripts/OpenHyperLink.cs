using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class OpenHyperLink : MonoBehaviour, IPointerClickHandler
{
  private TextMeshProUGUI text;
  private void Start() {
    text = GetComponent<TextMeshProUGUI>();
  }


  public void OnPointerClick(PointerEventData eventData) {
    // I was pointed to this url - https://deltadreamgames.com/unity-tmp-hyperlinks/
    // on how to implement hyperlinks using TMPro
    int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, Camera.main);

    if(linkIndex != -1)
    {
      TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];

      for(int i = 0; i < linkInfo.linkTextLength; i++) {
        int charIndex = linkInfo.linkTextfirstCharacterIndex = i;
        TMP_CharacterInfo charInfo = text.textInfo.characterInfo[charIndex];
      }

      Application.OpenURL(linkInfo.GetLinkID());
    }
  }
}
