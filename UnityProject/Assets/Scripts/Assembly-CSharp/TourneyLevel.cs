using TMPro;
using UnityEngine;
// Corrected ARM64 0x12CD708..0x12CD81C.
public class TourneyLevel : MonoBehaviour
{
 public TMP_Text level;
 private int actualLevel;
 private int maxLv=2;
 private void Start(){actualLevel=0;SetLevel();}
 public void NextLevel(){if(actualLevel!=maxLv){actualLevel++;SetLevel();}}
 public void BackLevel(){if(actualLevel!=0){actualLevel--;SetLevel();}}
 private void SetLevel(){level.text="Level "+(actualLevel+1);TournamentController.i.tourneyLevel=actualLevel+1;}
}
