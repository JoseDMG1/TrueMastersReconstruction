# Offline combat controls and AI reconstruction

## Evidence and status

This reconstruction uses the retained native Unity/IL2CPP build, SHA-256
`d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c`.
The canonical address source is `Evidence/cpp2il-native-address-map.json`.
`Evidence/native-disassembly-AI.txt` and
`Evidence/native-disassembly-DesktopInputs.txt` decode the correctly mapped ELF
bytes. Cpp2IL's printed `Disassembly:` is invalid for this ELF and was not used.
ISIL is a reading aid; missing comparisons were resolved against corrected ARM64.
These are static observations and C# translations, not proof of runtime parity.

The local RAG was queried for `AI Lv1 Block FastAttackFloor DesktopInputs Counter`.
It returned the AI symbol inventory and retained FastAttackFloor ISIL with address
provenance. The corrected native dump supplied the missing comparisons/constants.
`Tools/inspect-ai-flow.cjs METHOD` is an additional **reading aid**, annotating
corrected native instructions with method names, literal slots and numeric
constants; it does not generate or validate C# and suppresses setup instructions.
Check the full native dump for zero initializations, register reuse and lifetimes.

## Input and countdown ownership

`DesktopInputs.Actions` (VA `0x1288FDC`) sets the horizontal and vertical animator
values, moves through `FighterLocomotion`, and handles attack/jump/special axes.
At `0x12894CC..0x1289514`, a pressed attack axis follows
`BattleSceneSingleton.i.counter` (instance offset `0x50`) and reads `Counter.value`
(offset `0x20`). Attack is set only when the value is **less than zero**; this is
not a general readiness boolean. A nonpressed axis clears Attack. A pressed axis
while the counter is nonnegative does not itself clear a preexisting Attack.

Counter destroys its GameObject after displaying GO!, while the singleton retains
the managed component reference. Native code tests actual null and then directly
loads the float; it does not call Unity's overloaded object liveness operator.
The C# check therefore uses `ReferenceEquals(counter, null)`, preserving access to
the retained negative managed field after Unity destroys the component. The
reconstruction retains a genuine-null guard instead of reproducing a native null
exception. Actual component lifecycle and desktop combat remain runtime checks.

Previous reconstructed controls include InputSet, InputReader, MobileInputs,
AnimatorStatesManager, ButtonsController, ButtonAttack, RadialUIFX,
AnimationMovement, LookAt and ActivateAI. Their presence is not a fresh native
audit of every method. ButtonsController's captured cooldown target and original
initialization guards are reconstruction choices carried from the checkpoint.

## AI behavior restored

The original AI has 88 declared methods including its constructor and delegate
independent of the reconstruction's private helpers. There are 25 explicit
pending matchup methods. All three active level trees now have C# bodies:

| Native method | VA | Reconstruction coverage |
|---|---|---|
| Lv1 | `0x12530C4` | Close/far random attacks, stunned/block gates, Freeze and movement; callback corrected |
| Lv2 | `0x125331C` | Full state/element decision tree, charge/awakening gates and timing windows |
| Lv3 | `0x12548BC` | Full reachable state/element decision tree, charge/awakening gates, movement randomness, weak and jump reactions |
| FastAttack | `0x1257F54` | Element, BasicAttack moveset and distance branches |
| FastAttackFloor | `0x12582EC` | Element, BasicAttack moveset and distance branches |
| EvadeAttackDelay | `0x125860C` | Random roll then element/moveset branches |
| Legend | `0x125A3D4` | Legacy distance/state strategy including normalized-time thresholds |
| Hard | `0x1275EE4` | Legacy distance/state strategy including normalized-time thresholds |
| Normal | `0x125955C` | Legacy easyMove strategy, attack reactions and edge behavior |
| Pro | `0x1258D58` | Special gate and exact dispatch to 25 pending element-pair strategies |
| HasSpecial | `0x12768F0` | Character-group ownership keys, with Dante key corrected |

The prior checkpoint's basic attacks, movement, random helpers and level dispatch
were reused. `Move` was checked against native VA `0x1258A28`: direction zero
advances at 1, other directions at .5; normalized right/forward movement uses
7 * deltaTime. `Start` was checked at `0x1252238`: it schedules ChangeDirection
(4 s delay, 10 s interval) and NewAI (0 delay, .1 s interval). It does **not**
schedule ChangeEasyMove. No repeating legacy easyMove timer was invented.

Lv2/Lv3 helpers split the original large method by the player's animator state;
each helper remains owned by AI. `Window` stores the callback and calls DoInTime
immediately. DoInTime samples the player's current normalized time with exclusive
bounds; it does not create a delayed invocation or coroutine. Multiple windows
retain their original order and can perform multiple actions during one decision.

Three earlier mistakes/omissions were corrected with their producing evidence:

* Lv1's callback slot `0x2816228` is kind 3, method definition index **25459**,
  `Block` in `Evidence/assembly-csharp-metadata.json`. The .2/.6 window blocks
  while the player attacks. The previous reconstruction incorrectly invoked
  RandomAtk inside that window.
* Dante(Clone)/Agata(Clone) use the **Dante** PlayerPrefs ownership key, resolved
  from slot `0x2814960`; see `Evidence/handoff-ai-special-key.json`.
* Desktop countdown gating was restored at the actual Counter owner as described
  above, including destroyed-object managed-reference semantics.

Lv2/Lv3 use own DamageOnCollision.specialCharge >10. Fire additionally triggers
when NumSpecials ==1; other cases require the player's Weak flag. Their awakening
center cutoffs are 5 and 10 respectively. Lv3 first routes playerElement Shadow
to Lv2; Shadow-specific reaction blocks later in its native body are unreachable
after that test and were not duplicated into the per-state helpers.

Preserved native details include Lv2 Water DownAttack2 assigning callback DownAtk
and immediately BackFlip without invoking that callback, and Lv3 Water side
variant 2 assigning Block then immediately BackFlip. These odd sequences were
not replaced by assumed intent. Lv3 JumpR/JumpL uses own object name: Diana/Aura
reverse the normal side attack. Legacy Legend/Hard state names `Jump L`,
`Jump R`, `roll`, `hit1` are preserved even where modern level trees use
`JumpL` and `JumpR`.

## Remaining gaps and validation limits

All 25 legacy matchup trees (Fire/Earth/Air/Water/Light versus each of those five)
still call ReportPending once per method and perform no substitute strategy.
Pro's dispatch exists but is therefore **not complete gameplay**. Native NewAI's
active offline CPUSelection and Survival/Tournament routes call Lv1/Lv2/Lv3 and
level selectors, rather than the legacy SinglePlay -> Pro chain. This is a
source-derived reachability observation, not proof that animation/events or
external callers can never invoke a legacy strategy. Pair-specific reconstruction
remains necessary before claiming all 88 methods are restored.

This pass did not start Unity or a backend. The parent agent owns Unity import,
compilation and runtime validation. No new gameplay test result is claimed here.
The edited trees require actual offline matches at all CPU levels and all
elements, checking callback timing, countdown lifetime, shield/awakening and
Survival progression together with the restored damage and scoring owners.
The prior checkpoint's UI/material/shader and placeholder plugin limitations
continue to apply.
