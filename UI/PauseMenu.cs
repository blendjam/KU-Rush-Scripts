using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
  public static bool isPaused = false;

  private void Update()
  {

    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
    {
      isPaused = !isPaused;
      if (isPaused)
      {
        pause();
      }
      else
      {
        resume();
      }
    }
  }

  public void resume()
  {
    FindObjectOfType<UIManager>().hideAllPages();
    Time.timeScale = 1f;
    hideAndLockCursor(true);
  }

  private void pause()
  {
    FindObjectOfType<UIManager>().onPauseMenu();
    Time.timeScale = 0f;
    hideAndLockCursor(false);
  }

  public void loadSetting()
  {
    FindObjectOfType<UIManager>().onSettingsMenu();
  }

  public void loadMainMenu()
  {
    SceneManager.LoadScene(0);
    FindObjectOfType<UIManager>().mainMenuCamera.GetComponent<AudioListener>().enabled = true;
    FindObjectOfType<UIManager>().onMainMenu();
    Time.timeScale = 1f;
    isPaused = false;
    hideAndLockCursor(false);
  }

  public void exitGame()
  {
    if (!Application.isPlaying)
    {
      Application.Quit();
    }
    else
    {
      Application.Quit();
    }
  }

  private void hideAndLockCursor(bool lockCursor)
  {
    if (lockCursor)
    {
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
    }
    else
    {
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
    }
  }
}
