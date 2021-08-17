using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

  [SerializeField] CanvasGroup blackImage;
  [SerializeField] float speed;

  private Player player;
  private bool canFade;
  private AudioSource musicPlayer;

  private void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
    musicPlayer = GameObject.FindGameObjectWithTag("Music Player")?.GetComponent<AudioSource>();
    SceneManager.sceneLoaded += OnSceneLoad;
  }

  private void OnSceneLoad(Scene scene, LoadSceneMode mode)
  {
    if (scene.buildIndex != 0)
      player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
    else
      blackImage.alpha = 0;
  }


  //Loader.cs wil load and Start new game not needed here 
  public void startNewGame()
  {
    AudioManager.instance.Play("Button");
    StartCoroutine(StartFadeOut());
  }

  private void Update()
  {
    if (!canFade) return;

    blackImage.alpha += Time.deltaTime * speed;
    if (musicPlayer)
      musicPlayer.volume -= Time.deltaTime * speed;
  }

  IEnumerator StartFadeOut()
  {
    canFade = true;
    yield return new WaitForSeconds(3);
    if (musicPlayer)
      musicPlayer.Stop();
    Loader.instance.loadLevel();
    canFade = false;
  }

  public void respawnPlayer()
  {
    AudioManager.instance.Play("Button");
    UIManager.instance.hideAllPages();
    player.Respawn();
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
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

  public void loadSetting()
  {
    AudioManager.instance.Play("Button");
    FindObjectOfType<UIManager>().onSettingsMenu();
  }

  public void onLoadLevel()
  {
    AudioManager.instance.Play("Button");
    FindObjectOfType<UIManager>().onLoadLevel();
  }

  public void loadAboutPage()
  {
    AudioManager.instance.Play("Button");
    FindObjectOfType<UIManager>().onAboutPage();
  }
}
