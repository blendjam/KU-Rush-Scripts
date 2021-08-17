using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
  public TextMeshProUGUI sensValue;
  public GameObject fullScreenToggleKnob;
  public Resolution[] availResolutions;
  public TMP_Dropdown resolutionDropDown;

  private CameraController cameraController;

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    cameraController = FindObjectOfType<CameraController>();
  }

  private void OnEnable()
  {
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  private void Start()
  {
    cameraController = FindObjectOfType<CameraController>();

    availResolutions = Screen.resolutions;

    List<string> availResolutionsList = new List<string>();

    int currentResolutionIndex = 0;
    for (int i = 0; i < availResolutions.Length; i++)
    {
      string resoultionInString = availResolutions[i].width + " * " + availResolutions[i].height;
      availResolutionsList.Add(resoultionInString);

      if ((availResolutions[i].width == Screen.currentResolution.width) &&
         (availResolutions[i].height == Screen.currentResolution.height))
      {
        currentResolutionIndex = i;
      }
    }

    resolutionDropDown.AddOptions(availResolutionsList);
    resolutionDropDown.value = currentResolutionIndex;
    resolutionDropDown.RefreshShownValue();
  }
  public void setResolution(int resolutionIndex)
  {
    Resolution resolution = availResolutions[resolutionIndex];
    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
  }
  public void setFullScreen(bool isFullScreen)
  {
    Screen.fullScreen = isFullScreen;
    fullScreenToggleKnob.SetActive(isFullScreen);
  }
  public void setGraphicsQuality(int qualityIndex)
  {
    QualitySettings.SetQualityLevel(qualityIndex);
  }
  public void SetSensitivity(float value)
  {
    sensValue.text = value.ToString("F2");
    if (cameraController)
      cameraController.SetSensitivity(value);
  }
  public void onBackPressed()
  {
    AudioManager.instance.Play("Button");
    FindObjectOfType<UIManager>().hideAllPages();
    if (SceneManager.GetActiveScene().buildIndex == 0)
    {
      FindObjectOfType<UIManager>().onMainMenu();
    }
    else
    {
      FindObjectOfType<UIManager>().onPauseMenu();
    }
  }
  private void OnDisable()
  {
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }
}
