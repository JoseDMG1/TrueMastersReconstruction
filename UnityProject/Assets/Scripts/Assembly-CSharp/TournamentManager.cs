using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Corrected ARM64 0x12CBDD0..0x12CD704.
public class TournamentManager : MonoBehaviour
{
 public static TournamentManager i;
 public List<PlayerCheckpointsProgress> players;
 public List<PlayerCheckpointsProgress> winners;
 public int round;
 public List<GameObject> roundVS;
 public GameObject sun;
 public CharactersTournament charactersTournament;
 public ZoomCanvas zoomCanvas;
 public AudioSource winnerMusic;
 public AudioSource music;
 public AudioSource finalMusic;
 public Canvas canvas;
 public GameObject stars;
 public GameObject menu;
 public GameObject alert;
 private void Start(){
  if(i==null)i=this;else if(i!=this){Destroy(this);return;}
  Notifications.i.Remove();charactersTournament.Init();StartMusic();SetCheckpoints();SetPositions();SetOpponents();SetLosers();
  if(TournamentController.i.round>=1){round=TournamentController.i.round;ShowVS();Invoke(nameof(Continue),2f);Invoke(nameof(RemoveVS),1.5f);}else NextRound();
 }
 private void OnDestroy(){if(i==this)i=null;}
 private void StartMusic(){
  if(TournamentController.i.loseTournament)return;
  if(TournamentController.i.round==3)finalMusic.Play();else if(TournamentController.i.round==5)winnerMusic.Play();else music.Play();
 }
 public void SetLosers(){
  foreach(var p in players){
   if(TournamentController.i.losers[p.player])p.Lose();
   if(p.opponent==null)continue;
   int choice=Random.Range(0,2);
   if(p.player==1){if(TournamentController.i.loseTournament){p.Lose();continue;}}
   else if(choice==0){p.Lose();continue;}
   p.opponent.GetComponent<PlayerCheckpointsProgress>().Lose();
  }
 }
 public void SetPositions(){foreach(var p in players)p.SetPosition();}
 public void SetCheckpoints(){foreach(var p in players)p.checkpoint=TournamentController.i.checkpoints[p.player];}
 public void Continue(){NextRound();Invoke(nameof(NextRound),1.5f);if(TournamentController.i.loseTournament)Invoke(nameof(showMenu),3f);}
 public void showMenu(){menu.SetActive(true);}
 public void DelayStart(){Invoke(nameof(NextRound),1f);}
 public void ShowVS(){
  if(round==1)roundVS[0].SetActive(true);else if(round==3)roundVS[1].SetActive(true);else if(round==5)roundVS[2].SetActive(true);else RemoveVS();
  Invoke(nameof(RemoveVS),2f);
 }
 private void RotateSun(){sun.GetComponent<RotateSelf>().z=.2f;CancelInvoke();Invoke(nameof(Zoom),1f);Invoke(nameof(ShowStars),1.5f);}
 private void ShowStars(){sun.GetComponent<RotateSelf>().z=.5f;stars.SetActive(true);}
 public void NextRound(){
  round++;TournamentController.i.round=round;if(round==7)return;StartCoroutine(NextTurn());
  if(!TournamentController.i.loseTournament&&round==6)Invoke(nameof(RotateSun),1f);
 }
 public void NextCheckpoint(){StartCoroutine(NextTurn());}
 private IEnumerator NextTurn(){
  float skip=0f;
  foreach(var p in players){
   if(p.loser)continue;
   float oldSkip=skip;skip=oldSkip<2f?oldSkip+1f:1f;
   yield return new WaitForSeconds(oldSkip<2f?0f:1f);p.NextCheckpoint();
  }
  SetOpponents();Invoke(nameof(ActivateVS),1f);
 }
 public void SetOpponents(){foreach(var p in players)p.SetOpponent();}
 public void ActivateVS(){
  if(round==1)roundVS[0].SetActive(true);else if(round==3)roundVS[1].SetActive(true);else if(round==5)roundVS[2].SetActive(true);else{RemoveVS();return;}
  Invoke(nameof(StartBattle),0f);
 }
 private void RemoveVS(){foreach(var vs in roundVS)if(vs.activeSelf)foreach(var size in vs.GetComponentsInChildren<IncreaseSize>())size.Decrease();}
 public void StartBattle(){
  if(TournamentController.i.loseTournament||players[0].opponent==null)return;
  PlayerPrefs.SetString("CharacterCPU",players[0].opponent.GetComponent<PlayerCheckpointsProgress>().playerName);Invoke(nameof(Zoom),1.5f);Invoke(nameof(GoToBattle),3f);
 }
 public void Zoom(){if(TournamentController.i.loseTournament)return;zoomCanvas.currentView=zoomCanvas.views[TournamentController.i.battle];foreach(var p in players)p.enabled=false;}
 public void GoToBattle(){
  var tournament=TournamentController.i;if(tournament.loseTournament)return;
  if(tournament.stage==7)tournament.stage=StagesManager.i.RemoveRandom(7);
  PlayerPrefs.SetInt("Stage",tournament.stage);LevelChanger.i.FadeToLevel(5);tournament.battle++;
 }
 public void DisableCanvas(){canvas.enabled=false;}
 public void EnableCanvas(){canvas.enabled=true;}
 public void PlayAgain(){TournamentController.i.PayTournament(50);}
 public void QuitAlert(){alert.SetActive(true);}
 public void CancelAlert(){alert.GetComponent<IncreaseSize>().Decrease();}
}
