using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
  [SerializeField] GameObject keyImage;
  public static Inventory instance;

  Player player;

  private void Start()
  {
    if (instance == null)
      instance = this;
    else
    {
      Object.Destroy(instance);
    }
    player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
    keyImage.SetActive(false);
  }
  public void ShowKey()
  {
    keyImage.SetActive(true);
    player.hasKey = true;
  }
  public void HideKey()
  {
    keyImage.SetActive(false);
    player.hasKey = false;
  }
}
