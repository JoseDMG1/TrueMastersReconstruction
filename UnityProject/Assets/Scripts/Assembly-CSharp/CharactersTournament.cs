using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Corrected ARM64 0x121F85C..0x1220418.
public class CharactersTournament : MonoBehaviour
{
 public static CharactersTournament i;
 public List<string> characters;
 public List<GameObject> players;
 private void Awake(){if(i==null)i=this;else if(i!=this)Destroy(this);}
 private void OnDestroy(){if(i==this)i=null;}
 public void Init(){
  if(TournamentController.i.characters.Count<2){SetCharacters();Suffle();SetPlayer();SaveCharacters();}
  else characters=new List<string>(TournamentController.i.characters);
  SetIcons();
 }
 private void SetCharacters(){characters.AddRange(new[]{"Sora","Vinea","Diana","Amon","Aura","Eliot","Ren","Rina","Dante","Agata","Raven","Maya"});}
 private void SetPlayer(){
  string selected=CharactersManager.i.RemoveRandom(PlayerPrefs.GetString("Character"));PlayerPrefs.SetString("Character",selected);
  characters.Remove(selected);characters.Insert(0,selected);
 }
 private void Suffle(){
  for(int index=0;index<characters.Count;index++){string saved=characters[index];int next=Random.Range(index,characters.Count);characters[index]=characters[next];characters[next]=saved;}
 }
 public void SetIcons(){
  for(int index=0;index<players.Count;index++){
   var icon=GameObject.Find(characters[index]);icon.GetComponent<Image>().enabled=true;icon.GetComponent<RectTransform>().SetParent(players[index].transform,false);
   players[index].GetComponent<PlayerCheckpointsProgress>().playerName=characters[index];
  }
 }
 public void SaveCharacters(){TournamentController.i.characters=new List<string>(characters);}
}
