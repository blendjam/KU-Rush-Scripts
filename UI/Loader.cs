using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Loader : MonoBehaviour
{
  public GameObject loadingScreen;
  public Slider progressBarSlider;
  public TMP_Text progressText;

  public static Loader instance;

  private void Start()
  {
    if (instance == null)
      instance = this;
    else
    {
      Object.Destroy(gameObject);
    }
  }

  public void loadLevel()
  {
    // int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
    StartCoroutine(LoadAsynchronously(1));
  }

  IEnumerator LoadAsynchronously(int sceneIndex)
  {
    AsyncOperation loadProgress = SceneManager.LoadSceneAsync(sceneIndex);

    FindObjectOfType<UIManager>().onLoadingScreen();
    float currentProgress = 0f;
    while (!loadProgress.isDone)
    {
      currentProgress = Mathf.Clamp01((loadProgress.progress / 0.9f));
      progressBarSlider.value = currentProgress;
      progressText.text = (currentProgress * 100f).ToString("n2") + " %";
      yield return null;
    }
    FindObjectOfType<UIManager>().hideAllPages();
    FindObjectOfType<UIManager>().mainMenuCamera.GetComponent<AudioListener>().enabled = false;
    FindObjectOfType<UIManager>().mainMenuCamera.SetActive(false);
    FindObjectOfType<UIManager>().aboutPageCamera.SetActive(false);
  }
}
