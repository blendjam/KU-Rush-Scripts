using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{

  public void OnLevelOne()
  {
    AudioManager.instance.Play("Button");
    FindObjectOfType<UIManager>().hideAllPages();
    SceneManager.LoadScene(1);
  }

  public void OnLevelTwo()
  {
    AudioManager.instance.Play("Button");
    FindObjectOfType<UIManager>().hideAllPages();
    SceneManager.LoadScene(2);
  }
}
