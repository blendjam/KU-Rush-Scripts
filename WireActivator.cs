using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireActivator : MonoBehaviour
{

  [SerializeField] GameObject wirePrefab;
  [SerializeField] GameObject firstDialogue;
  [SerializeField] GameObject secondDialogue;
  private Player player;

  private void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
  }

  // private void OnTriggerEnter(Collider other)
  // {
  //   UIManager.instance.ShowInteract();
  // }
  // private void OnTriggerExit(Collider other)
  // {
  //   UIManager.instance.HideInteract();
  // }

  // private void OnTriggerStay(Collider other)
  // {
  //   if (Input.GetKeyDown(KeyCode.E))
  //     ShowDialogue();
  // }

  private void Update()
  {
    if (player.hasKey)
    {
      Object.Destroy(firstDialogue);
      if (secondDialogue)
        secondDialogue.SetActive(true);
    }
  }

  public void ShowWireScene()
  {
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
    wirePrefab.SetActive(true);
  }

  // public void ShowDialogue()
  // {
  //   UIManager.instance.HideInteract();
  //   if (!player.hasKey)
  //   {
  //     GameObject.Destroy(firstDialogue);
  //     return;
  //   }
  // }
}
