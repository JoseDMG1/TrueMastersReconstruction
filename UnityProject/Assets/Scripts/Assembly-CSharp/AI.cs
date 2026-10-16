using UnityEngine;

public class AI : MonoBehaviour
{
	public enum AttacksAI
	{
		none = 0,
		basic = 1,
		up = 2,
		down = 3,
		left = 4,
		right = 5,
		special = 6
	}

	public delegate void TestDelegate();

	private Animator anim;

	private Animator animPlayer;

	private GameObject player;

	private int direction = 1;

	private string centerPos;

	private Score Score;

	private int easyMove;

	public bool on;

	private string cpuElement;

	public string playerElement;

	private bool HardMode;

	private bool SpecialDone;

	private bool Special2Done;

	private int modeCpu;

	private bool moving;

	public TestDelegate callback;

	public bool onlyJump;

	private bool blockActive;

	public AttacksAI atkType;

	public bool Active;

	private void Start()
	{
        anim = GetComponent<Animator>();
         player = BattleSceneSingleton.i ? BattleSceneSingleton.i.MainCharacter : null;
         animPlayer = player ? player.GetComponent<Animator>() : null;
         Score = global::Score.score;
         var own = GetComponent<Description>();
         var other = player ? player.GetComponent<Description>() : null;
         cpuElement = own ? own.element : "";
         playerElement = other ? other.element : "";
         if (cpuElement == "Leaf") cpuElement = "Fire";
         if (playerElement == "Leaf") playerElement = "Fire";
         modeCpu = PlayerPrefs.GetInt("CpuMode", 0);
         HardMode = PlayerPrefs.HasKey("Aura") && PlayerPrefs.HasKey("Eliot") && PlayerPrefs.HasKey("Ren") && PlayerPrefs.HasKey("Rina");
         if (own && own.moveset == 1) SetMoveset(1);
         InvokeRepeating(nameof(ChangeDirection), 4f, 10f);
         InvokeRepeating(nameof(NewAI), 0f, .1f);
	}

	public void NewAI()
	{
        if (!on || !anim || !animPlayer) return;
         switch (atkType) {
         case AttacksAI.basic: BasicAtk(); break;
         case AttacksAI.up: UpAtk(); break;
         case AttacksAI.down: DownAtk(); break;
         case AttacksAI.left: SideAtkL(); break;
         case AttacksAI.right: SideAtkR(); break;
         case AttacksAI.special: anim.Play("Special"); break;
         }
         if (onlyJump) { if (Random.Range(0, 2) == 1) Roll(); else BackFlip(); return; }
         if (DistanceFromCenter() > 24f) { if (Random.Range(0, 2) == 1) Roll(); else Jump(); return; }
         if (SystemManager.i && SystemManager.i.mode == GameMode.Survival) {
         if (Score != null && Score.playerPoints > 9) Lv3(); else if (Score != null && Score.playerPoints > 5) Lv2(); else Lv1();
         } else if (SystemManager.i && SystemManager.i.mode == GameMode.Tournament) {
         var tournament = TournamentController.i;
         if (!tournament) return;
         if (tournament.tourneyLevel == 1) { if (tournament.battle == 2) Lv1(); else if (tournament.battle == 3) EasyTourney(); else if (tournament.battle == 4) NormalTourney(); }
         else if (tournament.tourneyLevel == 2) { if (tournament.battle == 2) EasyTourney(); else if (tournament.battle == 3) NormalTourney(); else if (tournament.battle == 4) ProTourney(); }
         else if (tournament.tourneyLevel == 3 && tournament.battle >= 2 && tournament.battle <= 4) ProTourney();
         } else CPUSelection();
	}

	private void Lv1()
	{
        if (blockActive) return;
         if (anim.GetBool("Stunned")) { restore(); return; }
         moving = true;
         if (direction == 0) direction = 1;
         if (animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Freeze")) { UpAtk(); return; }
         if (DistanceFromPlayer() < 5f) {
         switch (Random.Range(0, 3)) { case 0: RandomAtk(); break; case 1: if (animPlayer.GetBool("Attack")) { callback = Block; DoInTime(callback, .2f, .6f); } else RandomAtk(); break; case 2: BackFlip(); break; }
         return;
         }
         switch (Random.Range(1, 50)) {
         case 1: case 2: RandomAtk(); break;
         case 3: Jump(); break;
         case 4: if (DistanceFromPlayer() > 10f) Roll(); else BackFlip(); break;
         default: moving = true; break;
         }
	}

	private void Lv2()
	{
        // Native 0x125331C. Each window tests the player's normalized time,
        // synchronously; DoInTime does not schedule an action for later.
        if (blockActive) return;
        if (anim.GetBool("Stunned")) { restore(); return; }
        if (GetComponent<AnimatorStatesManager>().awakening && DistanceFromCenter() < 5f) { RandomAtk(); return; }
        float charge = GetComponent<DamageOnCollision>().specialCharge;
        if (charge > 10f && ((cpuElement == "Fire" && anim.GetInteger("NumSpecials") == 1) || animPlayer.GetBool("Weak"))) { Special(); return; }
        if (Random.Range(0, 11) == 10) { RandomAtk(); return; }
        moving = true;
        if (PlayerState("Freeze")) { UpAtk(); return; }
        if (DistanceFromPlayer() < 5f)
        {
            if (Random.Range(0, 3) == 2) BackFlip(); else RandomAtk();
            return;
        }
        if (PlayerState("BasicAttack"))
        {
            if (playerElement == "Shadow") EvadeAttack();
            else if (playerElement == "Fire" || playerElement == "Water" || playerElement == "Earth" || playerElement == "Light") Window(Roll, .4f, .8f);
            else Lv2StationaryDefense();
            return;
        }
        if (PlayerState("BasicAttack2"))
        {
            if (playerElement == "Light") { Window(RandomAtk, 0f, .2f); Window(Block, .3f, .6f); }
            else if (playerElement == "Fire") Window(Block, 0f, .6f);
            else if (playerElement == "Air") Window(Roll, .2f, .6f);
            else if (playerElement == "Shadow") { if (DistanceFromPlayer() < 15f) Window(Roll, 0f, .4f); Window(FastAttack, 0f, .3f); }
            else Lv2StationaryDefense();
            return;
        }
        if (PlayerState("UpAttack"))
        {
            if (playerElement == "Light")
            {
                if (DistanceFromPlayer() < 10f) { direction = 1; Window(BackFlip, 0f, .3f); }
                Window(Jump, .3f, .6f); Window(BasicAtk, .6f, .8f);
            }
            else if (playerElement == "Fire")
            {
                if (DistanceFromPlayer() < 10f) { direction = 1; Window(BackFlip, 0f, .4f); }
                Window(BasicAtk, .6f, .8f);
            }
            else if (playerElement == "Air" || playerElement == "Earth") { Window(Roll, .25f, .5f); Window(RandomAtk, .6f, .8f); }
            else { Window(BasicAtk, 0f, .1f); Window(Jump, .3f, .6f); Window(RandomAtk, .8f, 1f); }
            return;
        }
        if (PlayerState("UpAttack2"))
        {
            Window(BasicAtk, 0f, .1f); Window(Jump, .3f, .8f); Window(RandomAtk, .8f, 1f);
            // This native branch falls through to the later state checks.
        }
        if (PlayerState("RightAttack") || PlayerState("LeftAttack"))
        {
            if (playerElement == "Shadow")
            {
                Window(FastAttack, .1f, .3f);
                if (PlayerState("LeftAttack")) { if (DistanceFromPlayer() < 10f) Window(Roll, .6f, .8f); else RandomAtk(); }
            }
            else if (playerElement == "Air" || playerElement == "Earth") Window(Roll, .3f, .8f);
            else if (playerElement == "Water") { Window(UpAtk, 0f, .3f); Window(RandomAtk, .6f, .8f); }
            else { if (DistanceFromPlayer() < 6f) Window(Block, 0f, .5f); else Window(RandomAtk, 0f, .4f); }
            return;
        }
        if (PlayerState("RightAttack2") || PlayerState("LeftAttack2"))
        {
            if (playerElement == "Light") Window(Roll, 0f, .5f);
            else if (playerElement == "Fire") { if (DistanceFromPlayer() < 15f) Window(DownAtk, .2f, .8f); else Window(Roll, .2f, .8f); }
            else if (playerElement == "Air") { Window(BasicAtk, 0f, .3f); Window(Roll, .6f, .8f); }
            else Window(RandomAtk, 0f, .4f);
            return;
        }
        if (PlayerState("DownAttack"))
        {
            if (playerElement == "Light") { if (DistanceFromPlayer() > 15f) Window(UpAtk, 0f, .3f); else Window(Block, .3f, .8f); }
            else if (playerElement == "Water" || playerElement == "Fire") { if (DistanceFromPlayer() > 10f) Window(UpAtk, 0f, .5f); else Window(Block, .5f, 1f); }
            else if (playerElement == "Shadow") { if (DistanceFromPlayer() < 5f) Window(BackFlip, 0f, .3f); Window(UpAtk, .3f, 1f); }
            else Window(Jump, .3f, .8f);
            return;
        }
        if (PlayerState("DownAttack2"))
        {
            if (playerElement == "Fire") { if (DistanceFromPlayer() > 15f) Window(UpAtk, 0f, .6f); else Window(Block, .3f, 1f); }
            else if (playerElement == "Water")
            {
                Window(BasicAtk, 0f, .2f);
                if (DistanceFromPlayer() < 7f) { callback = DownAtk; BackFlip(); }
                else Window(Jump, .4f, .8f);
            }
            else if (playerElement == "Air") Window(Block, .4f, .8f);
            else if (playerElement == "Shadow") Window(RandomAtk, 0f, .6f);
            else Window(Jump, .3f, .8f);
            return;
        }
        if (PlayerState("JumpR") || PlayerState("JumpL")) { Window(RandomAtk, 0f, .5f); return; }
        if (animPlayer.GetBool("Weak")) direction = 0;
	}

    private bool PlayerState(string name) { return animPlayer.GetCurrentAnimatorStateInfo(0).IsName(name); }
    private void Window(TestDelegate action, float start, float end) { callback = action; DoInTime(callback, start, end); }
    private void Lv2StationaryDefense()
    {
        if (direction != 0) return;
        Window(Jump, .4f, .8f);
        Block();
    }

	private void Lv3()
	{
        // Native 0x12548BC. Shadow opponents deliberately use Lv2.
        if (playerElement == "Shadow") { Lv2(); return; }
        if (blockActive) return;
        if (anim.GetBool("Stunned")) { restore(); return; }
        moving = true;
        if (GetComponent<AnimatorStatesManager>().awakening && DistanceFromCenter() < 10f) { RandomAtk(); return; }
        if (PlayerState("Movement"))
        {
            int roll = Random.Range(0, 40);
            if (roll == 10) { RandomSideOrBasic(); return; }
            if (roll >= 31) direction = 0;
        }
        if (PlayerState("Freeze") || PlayerState("PrisonFree")) { UpAtk(); return; }
        if (PlayerState("Special")) { Window(playerElement == "Air" ? (TestDelegate)Block : Jump, 0f, .8f); return; }
        float charge = GetComponent<DamageOnCollision>().specialCharge;
        if (charge > 10f && ((cpuElement == "Fire" && anim.GetInteger("NumSpecials") == 1) || animPlayer.GetBool("Weak"))) { Special(); return; }
        if (DistanceFromPlayer() < 5f)
        {
            if (PlayerState("Movement") || animPlayer.GetBool("Blocking")) RandomAtk();
            int roll = Random.Range(0, 30);
            if (roll == 0) BackFlip(); else if (roll >= 1 && roll <= 3) RandomAtk();
        }
        if (PlayerState("BasicAttack")) { Lv3Basic(); return; }
        if (PlayerState("BasicAttack2")) { Lv3Basic2(); return; }
        if (PlayerState("UpAttack")) { Lv3Up(); return; }
        if (PlayerState("UpAttack2")) { Lv3Up2(); return; }
        if (PlayerState("RightAttack") || PlayerState("LeftAttack")) { Lv3Side(); return; }
        if (PlayerState("RightAttack2") || PlayerState("LeftAttack2")) { Lv3Side2(); return; }
        if (PlayerState("DownAttack")) { Lv3Down(); return; }
        if (PlayerState("DownAttack2")) { Lv3Down2(); return; }
        bool reversed = name == "Diana(Clone)" || name == "Aura(Clone)";
        if (PlayerState("JumpR")) { Window(reversed ? (TestDelegate)SideAtkR : SideAtkL, 0f, .5f); return; }
        if (PlayerState("JumpL")) { Window(reversed ? (TestDelegate)SideAtkL : SideAtkR, 0f, .5f); return; }
        if (animPlayer.GetBool("Weak")) Lv3Weak();
	}

    private void Lv3Basic()
    {
        switch (playerElement)
        {
            case "Air": if (DistanceFromPlayer() < 7f) Window(Block, .2f, .6f); else direction = 1; Window(RandomAtk, .4f, .8f); break;
            case "Water": EvadeAttack(); Window(Roll, .3f, .7f); break;
            case "Earth":
                if (DistanceFromPlayer() < 10f) EvadeAttack();
                else if (DistanceFromPlayer() < 15f) { EvadeAttack(); Window(Roll, .3f, .7f); }
                else direction = 1;
                break;
            case "Fire": EvadeAttack(); if (DistanceFromPlayer() > 10f) direction = 1; else Window(Roll, .3f, .5f); Window(RandomAtk, .5f, .9f); break;
            case "Light":
                if (DistanceFromPlayer() < 10f) { EvadeAttack(); Window(RandomAtk, .7f, .9f); }
                else { Window(RandomAtk, 0f, .1f); Window(EvadeAttackDelay, .3f, .7f); Window(RandomAtk, .7f, .9f); }
                break;
        }
    }

    private void Lv3Basic2()
    {
        switch (playerElement)
        {
            case "Air": EvadeAttack(); if (DistanceFromPlayer() < 10f) Window(Roll, .2f, .6f); else direction = 1; Window(RandomAtk, .5f, .9f); break;
            case "Water": if (DistanceFromPlayer() < 5f) Window(Block, .2f, .4f); else { direction = -1; Window(RandomAtk, .4f, .8f); } break;
            case "Earth": if (DistanceFromPlayer() < 5f) { Window(Block, .2f, .6f); Window(RandomAtk, .6f, .9f); } else { direction = -1; Window(RandomAtk, .5f, .7f); } break;
            case "Fire": if (DistanceFromPlayer() < 10f) Window(BackFlip, .1f, .2f); else direction = -1; Window(RandomAtk, .5f, .9f); break;
            case "Light": Window(FastAttack, 0f, .1f); Window(Jump, .3f, .7f); Window(FastAttack, .7f, .9f); break;
        }
    }

    private void Lv3Up()
    {
        switch (playerElement)
        {
            case "Air":
                if (DistanceFromPlayer() < 10f) { Window(BasicAtk, 0f, .1f); EvadeAttack(); Window(Roll, .1f, .4f); Window(RandomAtk, .5f, .9f); }
                else { EvadeAttack(); Window(Roll, .2f, .6f); Window(RandomAtk, .6f, .9f); }
                break;
            case "Water":
                if (DistanceFromPlayer() < 15f) { Window(BasicAtk, 0f, .1f); Window(Roll, .3f, .5f); Window(RandomAtkFront, .5f, .7f); }
                else { Window(UpAtk, 0f, .1f); Window(Roll, .3f, .5f); }
                break;
            case "Earth": EvadeAttack(); Window(Roll, .3f, .5f); Window(RandomAtk, .5f, .8f); break;
            case "Fire":
                if (DistanceFromPlayer() > 10f) direction = 1;
                else { Window(BasicAtk, 0f, .1f); Window(Jump, .2f, .5f); }
                Window(RandomAtk, .5f, .9f); break;
            case "Light": if (DistanceFromPlayer() < 10f) Window(FastAttack, 0f, .1f); Window(Jump, .3f, .5f); Window(RandomAtk, .5f, .9f); break;
        }
    }

    private void Lv3Up2()
    {
        switch (playerElement)
        {
            case "Air":
                if (DistanceFromPlayer() < 5f) { Window(DownAtk, .1f, .3f); Window(Block, .3f, .6f); }
                else Window(Jump, .3f, .6f);
                Window(RandomAtk, .6f, .9f); break;
            case "Water":
                if (DistanceFromPlayer() < 15f) { Window(BasicAtk, 0f, .15f); Window(Jump, .2f, .5f); Window(RandomAtk, .5f, .7f); }
                else { Window(Jump, .3f, .6f); Window(RandomAtk, .5f, .9f); }
                break;
            case "Earth": Window(BasicAtk, 0f, .15f); Window(Jump, .3f, .6f); Window(RandomAtk, .6f, .9f); break;
            case "Fire":
                if (DistanceFromPlayer() < 15f) { Window(BasicAtk, 0f, .1f); Window(Jump, .2f, .5f); }
                else Window(Jump, .3f, .5f);
                Window(RandomAtk, .5f, .9f); break;
            case "Light":
                if (DistanceFromPlayer() < 10f) { Window(RandomAtkFront, 0f, .1f); Window(BackFlip, .1f, .4f); }
                direction = 1; Window(RandomAtk, .5f, .9f); break;
        }
    }

    private void Lv3Side()
    {
        switch (playerElement)
        {
            case "Air": EvadeAttack(); Window(Roll, .3f, .5f); Window(RandomAtk, .5f, .9f); break;
            case "Water": Window(RandomAtkStrong, 0f, .5f); break;
            case "Earth": EvadeAttack(); Window(cpuElement == "Air" ? (TestDelegate)Block : Roll, .3f, .6f); Window(RandomAtk, .6f, .9f); break;
            case "Fire":
                if (DistanceFromPlayer() < 5f) { Window(Block, .1f, .3f); Window(Block, .3f, .7f); Window(RandomAtk, .7f, .9f); }
                else Window(RandomAtk, 0f, .5f);
                break;
            case "Light": if (DistanceFromPlayer() < 7f) Window(BackFlip, 0f, .5f); else Window(RandomAtk, .1f, .5f); break;
        }
    }

    private void Lv3Side2()
    {
        switch (playerElement)
        {
            case "Air":
                Window(RandomAtk, .1f, .2f);
                if (cpuElement == "Air") Window(Block, .2f, .6f);
                else if (DistanceFromPlayer() > 15f) Window(Roll, .2f, .6f); else Window(BackFlip, .2f, .6f);
                Window(RandomAtk, .6f, .9f); break;
            case "Water": if (DistanceFromPlayer() < 10f) { callback = Block; BackFlip(); } else Window(RandomAtk, .1f, .6f); break;
            case "Earth": if (DistanceFromPlayer() < 10f) { Window(BackFlip, .1f, .4f); Window(RandomAtk, .4f, .9f); } else Window(RandomAtk, .1f, .6f); break;
            case "Fire": EvadeAttack(); Window(Roll, .3f, .5f); Window(RandomAtk, .5f, .9f); break;
            case "Light": EvadeAttack(); Window(Roll, .3f, .6f); Window(RandomAtk, .6f, .9f); break;
        }
    }

    private void Lv3Down()
    {
        switch (playerElement)
        {
            case "Air":
                if (DistanceFromPlayer() < 10f) { Window(FastAttackFloor, 0f, .1f); Window(Jump, .2f, .5f); Window(RandomAtk, .6f, .9f); }
                else if (DistanceFromPlayer() < 18f) { Window(Jump, .4f, .6f); Window(RandomAtk, .6f, .9f); }
                else { Window(UpAtk, .1f, .3f); Window(Jump, .6f, .9f); }
                break;
            case "Water": if (DistanceFromPlayer() < 15f) { direction = 1; Window(UpAtk, 0f, .1f); Window(RandomAtk, .8f, .9f); } else Window(UpAtk, .1f, .5f); break;
            case "Earth":
                if (DistanceFromPlayer() < 10f) { Window(Jump, .1f, .3f); Window(Block, .3f, .6f); Window(RandomAtk, .6f, .9f); }
                else { Window(RandomAtk, 0f, .1f); direction = 1; Window(RandomAtk, .6f, .9f); }
                break;
            case "Fire": if (DistanceFromPlayer() < 10f) { Window(BackFlip, .1f, .6f); Window(Block, .6f, 1f); } else Window(UpAtk, .1f, .5f); break;
            case "Light":
                if (DistanceFromPlayer() < 15f) { Window(FastAttackFloor, 0f, .1f); Window(Block, .3f, .6f); Window(RandomAtk, .3f, .9f); }
                else { Window(RandomAtk, .1f, .2f); direction = 1; Window(RandomAtk, .6f, .9f); }
                break;
        }
    }

    private void Lv3Down2()
    {
        switch (playerElement)
        {
            case "Air":
                if (DistanceFromPlayer() < 20f) { Window(BasicAtk, 0f, .1f); Window(Block, .2f, .5f); Window(RandomAtk, .5f, .9f); }
                else { Window(UpAtk, .1f, .3f); Window(RandomAtkStrong, .3f, .7f); Window(RandomAtk, .7f, .9f); }
                break;
            case "Water":
                if ((double)animPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime < .2) RandomAtk();
                else Window(DistanceFromPlayer() > 10f ? (TestDelegate)Jump : Block, .5f, .9f);
                break;
            case "Earth": Window(RandomAtk, .5f, .9f); break;
            case "Fire": Window(EvadeAttackDelay, .5f, .7f); Window(RandomAtk, .7f, .9f); break;
            case "Light":
                if (DistanceFromPlayer() < 5f) { Window(BackFlip, 0f, .6f); Window(RandomAtk, .6f, .9f); }
                else { Window(FastAttack, 0f, .2f); Window(Jump, .2f, .4f); Window(RandomAtk, .4f, .9f); }
                break;
        }
    }

    private void Lv3Weak()
    {
        int mode = anim.GetInteger("BasicAttack");
        switch (cpuElement)
        {
            case "Air": if (mode == 0) { if (direction == 1) DownAtk(); else BasicAtk(); } else if (DistanceFromPlayer() < 15f) RandomAtkStrong(); else RandomSideOrUpAtk(); break;
            case "Water": if (mode == 0) RandomAtk(); else if (DistanceFromPlayer() < 10f) RandomAtkSide(); else RandomAtkStrong(); break;
            case "Earth": if (mode != 1) DownAtk(); else if (DistanceFromPlayer() < 10f) RandomAtkSide(); else RandomAtkFront(); break;
            case "Fire": if (mode == 1) RandomAtkButDownAtk(); else if (DistanceFromPlayer() < 10f) RandomAtkStrong(); else UpAtk(); break;
            case "Light": if (mode == 1) RandomAtkFront(); else if (DistanceFromPlayer() < 7f) RandomAtkSide(); else RandomAtkStrong(); break;
        }
    }

	private void RandomAtk()
	{
        int mode = anim.GetInteger("BasicAttack");
        float distance = DistanceFromPlayer();
        switch (cpuElement)
        {
            case "Air": case "Water":
                if (mode == 0) { if (distance < 13f) RandomAtk2(); else RandomAtkFront(); }
                if (mode == 1) { if (distance < 13f) RandomAtk2(); else RandomAtkButDownAtk(); }
                break;
            case "Earth":
                if (mode == 0) RandomAtk2();
                if (mode == 1) { if (distance < 13f) RandomAtk2(); else RandomAtkFront(); }
                break;
            case "Fire":
                if (mode == 0) { if (distance < 13f) RandomAtk2(); else RandomAtkButDownAtk(); }
                if (mode == 1) { if (distance < 13f) RandomAtk2(); else RandomAtkFront(); }
                break;
            case "Light":
                if (mode == 0) { if (distance < 13f) RandomAtk2(); else RandomAtkFront(); }
                if (mode == 1) { if (distance < 5f) DownAtk(); else if (distance < 13f) RandomAtk2(); else RandomAtkFront(); }
                break;
            case "Shadow": RandomAtk2(); break;
        }
    }

	private void FastAttack()
	{
        int mode = anim.GetInteger("BasicAttack");
        switch (cpuElement)
        {
            case "Air":
                if (mode == 0) { if (DistanceFromPlayer() < 10f) RandomSideOrBasic(); else BasicAtk(); }
                if (mode == 1) BasicAtk();
                break;
            case "Water":
                if (mode == 0) { if (DistanceFromPlayer() < 10f) RandomAtkSide(); else BasicAtk(); }
                if (mode == 1) { if (DistanceFromPlayer() < 5f) SideAtkR(); else BasicAtk(); }
                break;
            case "Earth":
                if (mode == 0) BasicAtk();
                if (mode == 1) { if (DistanceFromPlayer() < 10f) RandomSideOrBasic(); else BasicAtk(); }
                break;
            case "Fire":
                if (mode == 0) { if (DistanceFromPlayer() < 5f) RandomAtkSide(); else BasicAtk(); }
                if (mode == 1) { if (DistanceFromPlayer() < 10f) RandomAtkSide(); else BasicAtk(); }
                break;
            case "Light":
                if (mode == 0) { if (DistanceFromPlayer() < 7f) RandomAtkSide(); else BasicAtk(); }
                if (mode == 1) { if (DistanceFromPlayer() < 5f) DownAtk(); else if (DistanceFromPlayer() < 10f) RandomAtkSide(); else BasicAtk(); }
                break;
            case "Shadow": BasicAtk(); break;
        }
	}

	private void FastAttackFloor()
	{
        int mode = anim.GetInteger("BasicAttack");
        switch (cpuElement)
        {
            case "Air":
                if (mode == 0) BasicAtk();
                if (mode == 1) { if (DistanceFromPlayer() < 15f) DownAtk(); else RandomAtkSide(); }
                break;
            case "Water":
                if (mode == 0) DownAtk();
                if (mode == 1) BasicAtk();
                break;
            case "Earth":
                if (mode == 0) { if (DistanceFromPlayer() < 15f) DownAtk(); else BasicAtk(); }
                if (mode == 1) BasicAtk();
                break;
            case "Fire":
                if (mode == 0) { if (DistanceFromPlayer() < 5f) RandomAtkSide(); else if (DistanceFromPlayer() < 10f) DownAtk(); else UpAtk(); }
                if (mode == 1) BasicAtk();
                break;
            case "Light":
                if (mode == 0) { if (DistanceFromPlayer() < 5f) RandomAtkSide(); else RandomAtkStrong(); }
                if (mode == 1) { if (DistanceFromPlayer() < 5f) DownAtk(); else BasicAtk(); }
                break;
            case "Shadow": SideAtkR(); break;
        }
	}

	private void EvadeAttack()
	{
        callback = EvadeAttackDelay; DoInTime(callback, .1f, .2f);
	}

	private void EvadeAttackDelay()
	{
        if (Random.Range(0, 2) == 1) { Roll(); return; }
        int mode = anim.GetInteger("BasicAttack");
        switch (cpuElement)
        {
            case "Air":
                if (mode == 0) DownAtk();
                if (mode == 1) RandomAtkFront();
                break;
            case "Water":
                if (mode == 0) DownAtk();
                if (mode == 1) SideAtkR();
                break;
            case "Earth":
                if (mode == 0) RandomAtkSide();
                if (mode == 1) DownAtk();
                break;
            case "Fire":
                if (mode == 0) { if (DistanceFromPlayer() < 10f) DownAtk(); else Roll(); }
                if (mode == 1) DownAtk();
                break;
            case "Light":
                if (mode == 0 || mode == 1) DownAtk();
                break;
            case "Shadow": UpAtk(); break;
        }
	}

	public void DoInTime(TestDelegate action, float start, float end)
	{
        if (!animPlayer) return;
         float time = animPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime;
         if (time > start && time < end) action?.Invoke();
	}

	public void CheckAttacks()
	{
        // Native method is a single RET.
	}

	public bool IsOnFront(Vector3 pos)
	{
        return Vector3.Dot((pos - transform.position).normalized, transform.forward) > 0f;
	}

	public void Move()
	{
        float advance = direction == 0 ? 1f : .5f;
         if (DistanceFromPlayer() < 2.5f) RandomAtk();
         anim.SetFloat("SVertical", advance);
         anim.SetFloat("SHorizontal", direction);
         transform.position += (transform.right * direction + transform.forward * advance).normalized * 7f * Time.deltaTime;
	}

	private void Update()
	{
        if (anim && !anim.GetBool("Stunned") && moving) Move();
	}

	public void CPUSelection()
	{
        switch(modeCpu) { case 0: Lv1(); break; case 1: EasyTourney(); break; case 2: NormalTourney(); break; case 3: ProTourney(); break; case 4: Lv3(); break; }
	}

	public void SinglePlay()
	{
        if (Score == null) return;
         if ((Score.playerPoints == 2 && Score.enemyPoints == 0) || (Score.playerPoints > Score.enemyPoints && HardMode)) Pro();
         else if (Score.playerPoints > Score.enemyPoints || (Score.playerPoints == Score.enemyPoints && HardMode)) Normal();
         else Noob();
	}

	public void NormalTourney()
	{
        if (Score == null) return;
         if ((Score.playerPoints == 2 && Score.enemyPoints == 0) || Score.playerPoints > Score.enemyPoints) Lv3();
         else if (Score.playerPoints == Score.enemyPoints) Lv2(); else Lv1();
	}

	public void EasyTourney()
	{
        if (Score == null) return;
         if (Score.playerPoints == 2 && Score.enemyPoints == 0) Lv3();
         else if (Score.playerPoints > Score.enemyPoints) Lv2(); else Lv1();
	}

	public void ProTourney()
	{
        if (Score == null) return;
         if (Score.playerPoints == 2 && Score.enemyPoints == 0) Lv2();
         else if (Score.playerPoints >= Score.enemyPoints) Lv3(); else Lv2();
	}

	private void ActivateAI()
	{
        on = true;
         var description = GetComponent<Description>();
         if (description && description.moveset == 1) SetMoveset(1);
	}

	private void DeativateAI()
	{
        on = false;
         if (anim) anim.Rebind();
         moving = false;
         blockActive = false;
         if (GameController.i) GameController.i.notMove = true;
	}

	public void ManageAI(bool state)
	{
        var opponent = BattleSceneSingleton.i ? BattleSceneSingleton.i.Opponent : null;
         var ai = opponent ? opponent.GetComponent<AI>() : null;
         if (!ai) return;
         if (state) ai.ActivateAI(); else ai.DeativateAI();
	}

	private void ChangeEasyMove()
	{
        easyMove = Random.Range(1, 11);
	}

	private void ChangeDirection()
	{
        direction = Random.Range(-1, 2);
	}

	private bool IsNearEdge()
	{
        Vector3 pos = transform.position;
         return pos.x > 11f || pos.x < -11f || pos.z > 11f || pos.z < -11f;
	}

	public string CenterSide()
	{
        float side = Vector3.Dot(Vector3.Cross(-transform.position, transform.forward), transform.up);
         return side > 0f ? "Left" : side < 0f ? "Right" : "Front";
	}

	private float DistanceFromPlayer()
	{
        return player ? Vector3.Distance(player.transform.position, transform.position) : 0f;
	}

	private float DistanceFromCenter()
	{
        return transform.position.magnitude;
	}

	public bool IsCenterOnFront()
	{
        return Vector3.Dot((-transform.position).normalized, transform.forward) > 0f;
	}

	private void Legend()
	{
        // Native 0x125A3D4; legacy state spellings deliberately retained.
        restore();
        if (DistanceFromPlayer() < 9f)
        {
            if (DistanceFromCenter() > 20f && IsCenterOnFront()) { Jump(); return; }
            if (PlayerState("DownAttack")) UpAtk();
            else if (PlayerState("UpAttack")) BasicAtk();
            else if (PlayerState("BasicAttack")) DownAtk();
            else if (PlayerState("Jump L")) SideAtkR();
            else if (PlayerState("Jump R")) SideAtkL();
            else if (PlayerState("roll")) DownAtk();
            else if (PlayerState("hit1")) BasicAtk();
            else if (animPlayer.GetBool("Weak")) UpAtk();
            else RandomAtk();
            return;
        }
        if (PlayerState("BasicAttack")) { if (DistanceFromPlayer() < 15f) DownAtk(); else Roll(); return; }
        if (PlayerState("hit1")) { BasicAtk(); return; }
        if (PlayerState("DownAttack")) { UpAtk(); return; }
        if (PlayerState("UpAttack"))
        {
            animPlayer.IsInTransition(0); // Native calls this and discards its result.
            if ((double)animPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime < .55) MoveToSide("Left"); else BasicAtk();
            return;
        }
        if (animPlayer.GetBool("Weak")) { if (DistanceFromPlayer() >= 12f) UpAtk(); return; }
        if (PlayerState("Movement") && animPlayer.GetFloat("Vertical") < 0f)
        {
            float horizontal = animPlayer.GetFloat("Horizontal");
            if (horizontal > 0f) SideAtkL(); else if (horizontal < 0f) SideAtkR(); else BasicAtk();
        }
        else MoveFront();
	}

	private void FireVsFire()
	{
        ReportPending(nameof(FireVsFire));
	}

	private void FireVsEarth()
	{
        ReportPending(nameof(FireVsEarth));
	}

	private void FireVsAir()
	{
        ReportPending(nameof(FireVsAir));
	}

	private void FireVsWater()
	{
        ReportPending(nameof(FireVsWater));
	}

	private void FireVsLight()
	{
        ReportPending(nameof(FireVsLight));
	}

	private void EarthVsEarth()
	{
        ReportPending(nameof(EarthVsEarth));
	}

	private void EarthVsFire()
	{
        ReportPending(nameof(EarthVsFire));
	}

	private void EarthVsAir()
	{
        ReportPending(nameof(EarthVsAir));
	}

	private void EarthVsWater()
	{
        ReportPending(nameof(EarthVsWater));
	}

	private void EarthVsLight()
	{
        ReportPending(nameof(EarthVsLight));
	}

	private void AirVsEarth()
	{
        ReportPending(nameof(AirVsEarth));
	}

	private void AirVsAir()
	{
        ReportPending(nameof(AirVsAir));
	}

	private void AirVsFire()
	{
        ReportPending(nameof(AirVsFire));
	}

	private void AirVsWater()
	{
        ReportPending(nameof(AirVsWater));
	}

	private void AirVsLight()
	{
        ReportPending(nameof(AirVsLight));
	}

	private void WaterVsFire()
	{
        ReportPending(nameof(WaterVsFire));
	}

	private void WaterVsEarth()
	{
        ReportPending(nameof(WaterVsEarth));
	}

	private void WaterVsAir()
	{
        ReportPending(nameof(WaterVsAir));
	}

	private void WaterVsWater()
	{
        ReportPending(nameof(WaterVsWater));
	}

	private void WaterVsLight()
	{
        ReportPending(nameof(WaterVsLight));
	}

	private void LightVsWater()
	{
        ReportPending(nameof(LightVsWater));
	}

	private void LightVsFire()
	{
        ReportPending(nameof(LightVsFire));
	}

	private void LightVsEarth()
	{
        ReportPending(nameof(LightVsEarth));
	}

	private void LightVsAir()
	{
        ReportPending(nameof(LightVsAir));
	}

	private void LightVsLight()
	{
        ReportPending(nameof(LightVsLight));
	}

	private void Pro()
	{
        // Native 0x1258D58 dispatches the exact element pair; pair strategies
        // retain their own reconstruction status below.
        if (GuestPlayer.guestPlayer.GetComponent<DamageOnCollision>().Damage > 6f && !anim.GetBool("Stunned") && animPlayer.GetBool("Weak")) { Special(); return; }
        switch (cpuElement)
        {
            case "Fire": switch (playerElement) { case "Fire": FireVsFire(); break; case "Earth": FireVsEarth(); break; case "Air": FireVsAir(); break; case "Water": FireVsWater(); break; case "Light": FireVsLight(); break; } break;
            case "Earth": switch (playerElement) { case "Earth": EarthVsEarth(); break; case "Fire": EarthVsFire(); break; case "Air": EarthVsAir(); break; case "Water": EarthVsWater(); break; case "Light": EarthVsLight(); break; } break;
            case "Air": switch (playerElement) { case "Earth": AirVsEarth(); break; case "Fire": AirVsFire(); break; case "Air": AirVsAir(); break; case "Water": AirVsWater(); break; case "Light": AirVsLight(); break; } break;
            case "Water": switch (playerElement) { case "Earth": WaterVsEarth(); break; case "Fire": WaterVsFire(); break; case "Air": WaterVsAir(); break; case "Water": WaterVsWater(); break; case "Light": WaterVsLight(); break; } break;
            case "Light": switch (playerElement) { case "Earth": LightVsEarth(); break; case "Fire": LightVsFire(); break; case "Air": LightVsAir(); break; case "Water": LightVsWater(); break; case "Light": LightVsLight(); break; } break;
        }
	}

	private void Hard()
	{
        // Native 0x1275EE4.
        restore();
        if (DistanceFromPlayer() < 14f)
        {
            if (DistanceFromCenter() > 20f && IsCenterOnFront()) { Jump(); return; }
            if (PlayerState("DownAttack")) { if ((double)animPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime < .7) Block(); else UpAtk(); }
            else if (PlayerState("UpAttack")) { if ((double)animPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime < .7) Jump(); else BasicAtk(); }
            else if (PlayerState("BasicAttack")) { if (direction == 1) MoveToSide(); else DownAtk(); }
            else if (PlayerState("Jump L")) SideAtkR();
            else if (PlayerState("Jump R")) SideAtkL();
            else if (PlayerState("roll")) DownAtk();
            else if (PlayerState("hit1")) UpAtk();
            else if (animPlayer.GetBool("Weak")) DownAtk();
            else RandomAtk();
            return;
        }
        if (PlayerState("BasicAttack"))
        {
            if (DistanceFromPlayer() <= 20f) Roll();
            else if ((double)animPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime < .9) MoveFront();
            else MoveToSide();
        }
        else if (PlayerState("DownAttack")) UpAtk();
        else if (PlayerState("UpAttack")) { if ((double)animPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime < .55) Jump(); else RandomAtk(); }
        else if (animPlayer.GetBool("Weak")) MoveFront();
        else if (PlayerState("Movement") && animPlayer.GetFloat("Vertical") < 0f)
        {
            if (animPlayer.GetFloat("Horizontal") == 0f) RandomAtk();
        }
        else MoveToSide();
	}

	private void Normal()
	{
        // Native 0x125955C: this legacy strategy is separate from Lv2.
        restore();
        if (GuestPlayer.guestPlayer.GetComponent<DamageOnCollision>().Damage > 6f && !anim.GetBool("Stunned") && animPlayer.GetBool("Weak")) { Special(); return; }
        if (DistanceFromCenter() > 15f && IsCenterOnFront())
        {
            if (PlayerState("BasicAttack2") || PlayerState("UpAttack2") || PlayerState("BasicAttack") || PlayerState("DownAttack") || PlayerState("UpAttack")) MoveToSide();
            else if (DistanceFromPlayer() < 5f) RandomAtk(); else Roll();
            return;
        }
        if (DistanceFromPlayer() < 10f)
        {
            if (PlayerState("BasicAttack2") || PlayerState("UpAttack2") || PlayerState("BasicAttack") || PlayerState("DownAttack")) Block(); else RandomAtk();
            return;
        }
        switch (easyMove)
        {
            case 1: BasicAtk(); break;
            case 2: UpAtk(); break;
            case 3: DownAtk(); break;
            case 4: if (direction == 1) SideAtkL(); else SideAtkR(); break;
            case 5: Jump(); break;
            case 6: if (PlayerState("BasicAttack") || PlayerState("DownAttack") || PlayerState("UpAttack")) Jump(); else BasicAtk(); break;
            case 7: if (PlayerState("BasicAttack") || PlayerState("DownAttack") || PlayerState("UpAttack")) Jump(); else MoveFront(); break;
            case 8: if (DistanceFromPlayer() < 13f) BackFlip(); else Roll(); break;
            default: MoveToSide(); break;
        }
	}

	private void Noob()
	{
        restore();
         if (DistanceFromPlayer() < 5f) BackFlip();
         switch (easyMove) {
         case 1: BasicAtk(); break;
         case 2: RandomAtk(); break;
         case 3: case 4: MoveFront(); break;
         case 5: Jump(); break;
         case 6: if (DistanceFromPlayer() > 10f) Roll(); else BackFlip(); break;
         default: MoveToSide(); break;
         }
	}

	private void Easy()
	{
        restore();
         if (DistanceFromPlayer() < 5f) BackFlip();
         switch (easyMove) {
         case 1: BasicAtk(); break;
         case 2: if (direction == 1) UpAtk(); else DownAtk(); break;
         case 3: if (direction == 1) SideAtkL(); else SideAtkR(); break;
         case 4: MoveFront(); break;
         case 5: Jump(); break;
         case 6: if (DistanceFromPlayer() > 10f) Roll(); else BackFlip(); break;
         default: MoveToSide(); break;
         }
	}

	private void restore()
	{
        if (!anim) return;
         anim.SetBool("Attack", false);
         anim.SetBool("Jump", false);
         anim.SetFloat("Vertical", 0f);
         anim.SetFloat("Horizontal", 0f);
         if (easyMove <= 3) { anim.SetFloat("SVertical", 0f); anim.SetFloat("SHorizontal", 0f); moving = false; }
	}

	private void RandomAtk2()
	{
        int value = Random.Range(1, 10);
         if (value <= 3) BasicAtk(); else if (value <= 5) UpAtk(); else if (value <= 7) DownAtk(); else if (value == 8) SideAtkL(); else SideAtkR();
	}

	private void RandomSideOrDownAtk()
	{
        switch(Random.Range(1, 4)) { case 2: SideAtkL(); break; case 3: SideAtkR(); break; default: DownAtk(); break; }
	}

	private void RandomSideOrUpAtk()
	{
        switch(Random.Range(1, 4)) { case 2: SideAtkL(); break; case 3: SideAtkR(); break; default: UpAtk(); break; }
	}

	private void RandomSideOrBasic()
	{
        switch(Random.Range(1, 6)) { case 1: SideAtkR(); break; case 2: SideAtkL(); break; default: BasicAtk(); break; }
	}

	private void RandomAtkButDownAtk()
	{
        switch(Random.Range(0, 5)) { case 0: case 1: BasicAtk(); break; case 2: SideAtkL(); break; case 3: SideAtkR(); break; default: UpAtk(); break; }
	}

	private void RandomAtkFront()
	{
        switch(Random.Range(0, 3)) { case 0: BasicAtk(); break; case 1: DownAtk(); break; case 2: UpAtk(); break; }
	}

	private void RandomAtkStrong()
	{
        if (Random.Range(0, 2) == 0) UpAtk(); else DownAtk();
	}

	private void RandomAtkSide()
	{
        if (Random.Range(0, 2) == 0) SideAtkL(); else SideAtkR();
	}

	private void RandomBasicAtk()
	{
        switch(Random.Range(0, 3)) { case 0: SideAtkL(); break; case 1: SideAtkR(); break; case 2: BasicAtk(); break; }
	}

	private void BasicAtk()
	{
        SetAction(0f, 0f, "Attack");
	}

	private void SideAtkL()
	{
        var shield = GetComponent<WaterShieldController>();
         if (gameObject.name == "Vinea(Clone)" && shield && shield.LeftShieldOn) { RandomAtk(); return; }
         SetAction(-1f, 0f, "Attack");
	}

	private void SideAtkR()
	{
        var shield = GetComponent<WaterShieldController>();
         if (gameObject.name == "Vinea(Clone)" && shield && shield.RightShieldOn) { RandomAtk(); return; }
         SetAction(1f, 0f, "Attack");
	}

	private void Jump()
	{
        SetAction(CenterSide() == "Right" ? -1f : 1f, 0f, "Jump");
	}

	private void Block()
	{
        if (blockActive) return;
         var damage = GetComponent<DamageOnCollision>();
         if (damage && damage.shieldDamage >= 5f) { Jump(); return; }
         SetAction(0f, 0f, "Jump");
         blockActive = true;
         Invoke(nameof(DeactiveBlock), 1.5f);
	}

	private void DeactiveBlock()
	{
        blockActive = false;
	}

	private void BackFlip()
	{
        if (DistanceFromCenter() > 15f && IsCenterOnFront()) { Block(); return; }
         SetAction(0f, -1f, "Jump");
	}

	private void Roll()
	{
        SetAction(0f, 1f, "Jump");
	}

	private void DownAtk()
	{
        if (((cpuElement == "Earth" && DistanceFromCenter() > 13f) || (cpuElement == "Air" && DistanceFromCenter() > 15f)) && anim.GetInteger("BasicAttack") == 0) { UpAtk(); return; }
         SetAction(0f, -1f, "Attack");
	}

	private void UpAtk()
	{
        SetAction(0f, 1f, "Attack");
	}

	private void CPUAwakening()
	{
        Invoke(nameof(FinishAwakening), 25f);
	}

	private void FinishAwakening()
	{
        anim.SetInteger("NumSpecials", 1);
         var aura = GetComponent<AuraController>(); if (aura) aura.StopAura();
         var states = GetComponent<AnimatorStatesManager>(); if (states) states.awakening = false;
         var damage = GetComponent<DamageOnCollision>(); if (damage) damage.specialCharge = 0f;
	}

	private void Special()
	{
        var damage = GetComponent<DamageOnCollision>(); if (damage) damage.specialCharge = 0f;
         if (SpecialDone) { Special2(); return; }
         CPUAwakening(); SpecialDone = true; anim.SetTrigger("Special");
	}

	private bool HasSpecial()
	{
        if (!BattleSceneSingleton.i || !BattleSceneSingleton.i.Opponent) return false;
         string key;
         switch (BattleSceneSingleton.i.Opponent.name) {
         case "Rina(Clone)": case "Amon(Clone)": key = "Rina"; break;
         case "Dante(Clone)": case "Agata(Clone)": key = "Dante"; break;
         case "Eliot(Clone)": case "Vinea(Clone)": key = "Eliot"; break;
         case "Sora(Clone)": case "Aura(Clone)": key = "Aura"; break;
         case "Diana(Clone)": case "Ren(Clone)": key = "Ren"; break;
         default: return false;
         }
         return PlayerPrefs.HasKey(key);
	}

	private void Special2()
	{
        var damage = GetComponent<DamageOnCollision>(); if (damage) damage.specialCharge = -100f;
         if (Special2Done) return;
         Special2Done = true; anim.SetTrigger("Special");
	}

	private void MoveToSide(string side = "Right", bool back = false)
	{
        anim.SetFloat("SVertical", .5f);
         anim.SetFloat("SHorizontal", direction);
         if (DistanceFromCenter() > 24f) Jump();
	}

	private void MoveFront()
	{
        anim.SetFloat("SVertical", 1f); anim.SetFloat("SHorizontal", 0f);
	}
    private void SetAction(float horizontal, float vertical, string action)
    {
        if (!anim) return;
        anim.SetFloat("Vertical", vertical);
        anim.SetFloat("Horizontal", horizontal);
        anim.SetBool(action, true);
    }
    private void SetMoveset(int value)
    {
        foreach (string parameter in new[] { "BasicAttack", "UpAttack", "DownAttack", "LeftAttack", "RightAttack" }) anim.SetInteger(parameter, value);
    }
    private readonly System.Collections.Generic.HashSet<string> pendingMethods = new System.Collections.Generic.HashSet<string>();
    private void ReportPending(string method)
    {
        if (pendingMethods.Add(method)) Debug.LogWarning("AI." + method + " native decision tree is pending reconstruction.", this);
    }
}
