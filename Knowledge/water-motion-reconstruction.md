# Water projectile motion reconstruction

MoveWater was empty in the exported source. Its active Vinea/Eliot prefab
dependencies require real displacement, not merely successful instantiation.
The editable owner is now restored from the correctly mapped ARM64 retained in
Evidence/native-disassembly-MoveWater.txt. Native library SHA256:
d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c.
Cpp2IL printed disassembly is incorrectly mapped; its named ISIL is supporting
engine-call evidence only. No original Android execution is claimed.

- Start0x129DA44 adds local right*position + up*positionV + forward*positionZ
  to world position, invokes StartMov after timeToStart, then multiplies speed
  by50 (0x42480000). Slot0x28170D8 resolves the literal StartMov.
- Update0x129DBE0 reads GetComponent<ConfigAttackLayer>().playerWeak. Generic
  slot0x2814C38 resolves that exact type. Weak displacement is
  (-up*.35 + forward*.1)*speed*Time.deltaTime, even before moving becomes true.
  Float data0x651A30=-.35 and0x651BA8=.1 establish those coefficients.
- Otherwise Update returns until moving; optional changeDirectionAtStart calls
  ChangeDirection, then ready=true and forward*speed*Time.deltaTime. This owner
  differs from fixed-tick MoveRock/MoveLight; do not add or remove deltaTime
  based on their superficial similarity.
- ChangeDirection0x129DE4C returns when ready. It computes normalized opponent
  direction, copies the OWNER rotation (not LookAt), and zeros dir.y. Layer9
  means HostPlayer owns it / GuestPlayer is target; other layers reverse that.
  Slots0x2814908/0x2814910 resolve GuestPlayer/HostPlayer.
- StartMov0x129E170 sets moving=true. IsPlayerIsOk0x129E17C retains the legacy
  helper which sets its separate playerWeak field for native hit/fall states
  before normalizedTime.5. No mapped direct callers were found; Start schedules
  only StartMov. The helper was not newly wired into Update. Update's actual
  weak branch belongs to ConfigAttackLayer, not that legacy field.

All public serialized fields and the .meta identity are retained. No prefabs,
clips or immutable exports were modified. Runtime acceptance is pending the
new directional-move reports; source restoration alone proves no motion result.
