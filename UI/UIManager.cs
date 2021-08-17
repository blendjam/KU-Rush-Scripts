
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
  public enum UIPages
  {
    MainMenu = 0,
    SettingsMenu = 1,
    PauseMenu = 2,
    AboutPage = 3,
    LoadingScreen = 4,
    RespawnMenu = 5,
    Levels = 6
  }


  public List<GameObject> uiPages;
  public TextMeshProUGUI screenText;
  public GameObject mainMenuCamera;
  public GameObject aboutPageCamera;
  public GameObject canvas;
  public static UIManager instance;
  public bool isMainMenu = false;

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  private void Start()
  {
    HideInteract();
    hideAllPages();
    if (isMainMenu)
    {
      onMainMenu();
    }
    else
    {
      mainMenuCamera.SetActive(false);
      aboutPageCamera.SetActive(false);
    }
    GuardController.OnGuardHasSpottedPlayer += onRespawnMenu;
  }

  private void OnEnable()
  {
    SceneManager.sceneLoaded += OnSceneLoad;
  }

  private void OnSceneLoad(Scene scene, LoadSceneMode mode)
  {
    if (scene.buildIndex != 0)
    {
      mainMenuCamera.SetActive(false);
      aboutPageCamera.SetActive(false);
    }
  }


  public void showUIPages(UIPages pageType)
  {
    for (int i = 0; i < uiPages.Count; i++)
    {
      if (i == (int)pageType)
      {
        uiPages[i].SetActive(true);
      }
    }
  }

  public void hideAllPages()
  {
    foreach (GameObject o in uiPages)
    {
      o.SetActive(false);
    }
  }

  public void onRespawnMenu()
  {
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
    showUIPages(UIPages.RespawnMenu);
  }

  public void onMainMenu()
  {
    mainMenuCamera.SetActive(true);
    aboutPageCamera.SetActive(false);
    hideAllPages();
    showUIPages(UIPages.MainMenu);
  }

  public void onSettingsMenu()
  {
    hideAllPages();
    showUIPages(UIPages.SettingsMenu);
  }

  public void onPauseMenu()
  {
    hideAllPages();
    showUIPages(UIPages.PauseMenu);
  }

  public void onAboutPage()
  {
    hideAllPages();
    showUIPages(UIPages.AboutPage);
    aboutPageCamera.SetActive(true);
  }

  public void onLoadLevel()
  {
    hideAllPages();
    showUIPages(UIPages.Levels);
  }

  public void onLoadingScreen()
  {
    hideAllPages();
    showUIPages(UIPages.LoadingScreen);
  }

  public void ShowInteract()
  {
    screenText.text = "Press E to interact";
    canvas.SetActive(true);
  }

  public void TypeInteract(string text)
  {
    screenText.text = text;
    canvas.SetActive(true);
  }

  public void HideInteract()
  {
    canvas.SetActive(false);
    screenText.text = "";
  }

  public void onCloseAllMenu()
  {
    hideAllPages();
  }
}

public enum CharacterName
{
  Undefined,
  Player,
  Ramesh,
  NPC2,
  Kyle,
  Jumping,
  Crouching

}
