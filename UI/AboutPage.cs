using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutPage : MonoBehaviour
{
  public void onBackPressed()
  {
    AudioManager.instance.Play("Button");
    FindObjectOfType<UIManager>().hideAllPages();
    FindObjectOfType<UIManager>().onMainMenu();
  }
}
