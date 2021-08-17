using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueManager : MonoBehaviour
{
  public static DialogueManager Instance;


  public TextMeshProUGUI speakerName, dialogueText;
  public RawImage imageHolder;
  public GameObject canvas;
  public float dialogueSpeed;


  private Player playerRef;
  private int currentLine;
  private string[] dialogueLines;
  private string[] dialogueLinesAfterAnimation;
  private bool isTyping;
  private bool animationPlayed;
  private bool lastDialogueSaid;
  private string endAnimationName;
  private bool hasKey;
  private UnityEvent endEvent;


  private void Start()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
      Destroy(gameObject);
    currentLine = 0;
    canvas.SetActive(false);
    playerRef = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
  }

  private void OnEnable()
  {
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    if (scene.buildIndex != 0)
      playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
  }

  private void Update()
  {
    if (!canvas.activeInHierarchy) return;

    if (!isTyping && currentLine == 0)
    {
      NextSentence();
    }
    else if (!isTyping && Input.GetKeyDown(KeyCode.E))
    {
      NextSentence();
    }
  }

  public void NextSentence()
  {
    if (currentLine < dialogueLines.Length)
    {
      dialogueText.text = "";
      if (canvas.activeInHierarchy)
        StartCoroutine(WriteSentence());
    }
    else
    {
      if (endEvent != null)
        endEvent.Invoke();
      if (!animationPlayed && endAnimationName != "" && canvas.activeInHierarchy)
      {
        StartCoroutine(PlayAnimation());
      }
      else
        HideDialogue();
    }
  }

  public void SetEvent(UnityEvent unityEvent)
  {
    endEvent = unityEvent;
  }

  IEnumerator PlayAnimation()
  {
    playerRef.StopMove();
    playerRef.PlayAnimation(endAnimationName);
    yield return new WaitForSeconds(playerRef.GetAnimationLength(endAnimationName) - 2f);
    animationPlayed = true;
    if (dialogueLinesAfterAnimation.Length > 0)
    {
      currentLine = 0;
      isTyping = false;
      dialogueLines = dialogueLinesAfterAnimation;
      lastDialogueSaid = true;
    }
    if (lastDialogueSaid)
    {
      playerRef.StartMove();
      if (hasKey)
        Inventory.instance.ShowKey();
    }
  }


  IEnumerator WriteSentence()
  {
    isTyping = true;
    foreach (char lineCharacter in dialogueLines[currentLine].ToCharArray())
    {
      dialogueText.text += lineCharacter;
      yield return new WaitForSeconds(dialogueSpeed);
    }
    isTyping = false;
    currentLine++;
  }

  public void ShowDialogue(CharacterName characterName, string[] lines, string[] endlines, string animationName, bool hasKey)
  {
    this.hasKey = hasKey;
    endAnimationName = animationName;
    currentLine = 0;
    dialogueLines = lines;
    dialogueLinesAfterAnimation = endlines;
    isTyping = false;
    speakerName.text = characterName.ToString();
    imageHolder.texture = GetImage(characterName);
    canvas.SetActive(true);
    playerRef.StopMove();
  }

  public void HideDialogue()
  {
    currentLine = 0;
    isTyping = false;
    dialogueText.text = "";
    canvas.SetActive(false);
    playerRef.StartMove();
    lastDialogueSaid = false;
    animationPlayed = false;
  }

  private Texture GetImage(CharacterName charName)
  {
    Texture2D t;
    // #if UNITY_EDITOR
    //     t = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Resourses/Textures/Characters/" + charName + ".png", typeof(Texture2D));
    //     return t;
    // #endif
    t = Resources.Load<Texture2D>("Characters/" + charName) as Texture2D;
    return t;
  }

  private void OnDisable()
  {
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }
}
