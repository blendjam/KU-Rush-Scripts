using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireLogic : MonoBehaviour
{
  public List<Wire> wires;
  [SerializeField] GameObject wireScenePrefab;
  [SerializeField] Door door;
  [SerializeField] AudioClip completeSound;

  private float counter;
  private bool completed;

  private void ShuffleWires()
  {
    List<Vector3> endWirePos = new List<Vector3>();
    foreach (Wire w in wires)
    {
      Vector3 pos = w.endWire.transform.position;
      endWirePos.Add(pos);
    }
    foreach (Wire w in wires)
    {
      int randomIndex = Random.Range(0, endWirePos.Count);
      w.endWire.transform.position = endWirePos[randomIndex];
      endWirePos.RemoveAt(randomIndex);
    }
  }

  private void Start()
  {
    ShuffleWires();
  }

  private void Update()
  {
    int connectedWires = 0;
    foreach (Wire w in wires)
    {
      if (w.IsConnected())
        connectedWires++;
    }
    if (connectedWires == wires.Count && !completed)
    {
      completed = true;
    }
    if (completed && counter < 1.5f)
    {
      counter += Time.deltaTime;
    }
    else if (counter >= 1.5f)
      CloseScene();

  }

  private void DisconnectWires()
  {
    foreach (Wire w in wires)
    {
      w.ResetPosition();
    }
  }

  private void CloseScene()
  {
    completed = false;
    counter = 0;
    AudioSource.PlayClipAtPoint(completeSound, Camera.main.transform.position);
    DisconnectWires();
    ShuffleWires();
    wireScenePrefab.SetActive(false);
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
    door.OpenDoor(false);
  }
}
