using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class DialogueActivator : MonoBehaviour
{
  [SerializeField] bool destoryOnTrigger;
  [SerializeField] bool activateOnTrigger;
  [SerializeField] bool hasKey;
  [SerializeField] string endAnimationName;
  [SerializeField] UnityEvent endDialogueEvent;
  [SerializeField] CharacterName speakerName;
  [SerializeField] string[] dialogLines;
  [SerializeField] string[] dialogLinesAfterAnimation;

  private bool canActivate;

  private void Update()
  {
    if (DialogueManager.Instance.canvas.activeInHierarchy) return;

    if (canActivate && Input.GetKeyDown(KeyCode.E))
    {
      SayTheLines();
      canActivate = false;
    }
  }

  public void SayTheLines()
  {
    if (UIManager.instance != null)
      UIManager.instance.HideInteract();
    DialogueManager.Instance.ShowDialogue(speakerName, dialogLines, dialogLinesAfterAnimation, endAnimationName, hasKey);
    DialogueManager.Instance.SetEvent(endDialogueEvent);
    if (destoryOnTrigger)
      Object.Destroy(gameObject);

  }

  public void HideDialogueBox()
  {
    DialogueManager.Instance.NextSentence();
    DialogueManager.Instance.HideDialogue();
    canActivate = false;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Player")
    {
      if (activateOnTrigger)
        SayTheLines();
      else if (!DialogueManager.Instance.canvas.activeInHierarchy)
      {
        canActivate = true;
        UIManager.instance.ShowInteract();
      }
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.tag == "Player")
    {
      canActivate = false;
      UIManager.instance.HideInteract();
    }
  }
}
