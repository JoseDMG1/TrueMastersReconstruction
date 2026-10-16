using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Corrected ARM64 0x1250F9C..0x1251494; IDs index persistent nine-slot lists.
public class PlayerCheckpointsProgress : MonoBehaviour
{
 public List<GameObject> checkpoints;
 public Vector3 nextCheckpoint;
 public float moveSpeed=.01f;
 public int checkpoint;
 public bool loser;
 public int player;
 public string playerName;
 public GameObject currentCheckpoint;
 public GameObject opponent;
 public void SetPosition(){
  currentCheckpoint=checkpoints[checkpoint];nextCheckpoint=currentCheckpoint.GetComponent<RectTransform>().localPosition;
  currentCheckpoint.GetComponent<BattleCheckpoint>().player=gameObject;GetComponent<RectTransform>().localPosition=nextCheckpoint;
 }
 private void FixedUpdate(){var rect=GetComponent<RectTransform>();rect.localPosition=Vector3.Lerp(rect.localPosition,nextCheckpoint,moveSpeed);}
 public void NextCheckpoint(){
  checkpoint++;currentCheckpoint=checkpoints[checkpoint];nextCheckpoint=currentCheckpoint.GetComponent<RectTransform>().localPosition;
  currentCheckpoint.GetComponent<BattleCheckpoint>().player=gameObject;TournamentController.i.checkpoints[player]=checkpoint;
 }
 public void Lose(){loser=true;GetComponentInChildren<Image>().color=new Color(100f/255f,100f/255f,100f/255f,1f);TournamentController.i.losers[player]=true;}
 public void SetOpponent(){var other=currentCheckpoint.GetComponent<BattleCheckpoint>().cpointOpponent;opponent=other!=null?other.GetComponent<BattleCheckpoint>().player:null;}
}
