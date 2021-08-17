using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
  [SerializeField] GameObject[] popUps;
  [SerializeField] float fadeTime;
  private int popupIndex;
  private float waitCounter;
  private bool startCounting;

  private void Awake()
  {
    foreach (var popup in popUps)
    {
      popup.SetActive(false);
    }
  }

  private void Update()
  {
    if (!gameObject.activeInHierarchy) return;

    CanvasGroup canvasGroup;
    for (int i = 0; i < popUps.Length; i++)
    {
      if (i == popupIndex)
      {
        canvasGroup = popUps[i].GetComponent<CanvasGroup>();
        popUps[i].SetActive(true);
        if (canvasGroup.alpha <= 1)
          canvasGroup.alpha += Time.deltaTime;
      }
      else
      {
        canvasGroup = popUps[i].GetComponent<CanvasGroup>();
        if (canvasGroup.alpha >= 0)
          canvasGroup.alpha -= Time.deltaTime;
        else if (canvasGroup.alpha <= 0)
          popUps[i].SetActive(false);
      }
    }
    switch (popupIndex)
    {
      case 0:
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A))
        {
          waitCounter = 0;
          startCounting = true;
        }

        if (startCounting && waitCounter < fadeTime)
          waitCounter += Time.deltaTime;
        else if (waitCounter >= fadeTime)
        {
          waitCounter = 0;
          startCounting = false;
          popupIndex++;
        }
        break;

      case 1:
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
          startCounting = true;
        }
        if (startCounting && waitCounter < fadeTime)
          waitCounter += Time.deltaTime;
        else if (waitCounter >= fadeTime)
        {
          waitCounter = 0;
          startCounting = false;
          popupIndex = -1;
        }
        break;
      case 2:
        if (Input.GetKeyDown(KeyCode.Space))
          popupIndex = -1;
        break;
    }
  }
}
