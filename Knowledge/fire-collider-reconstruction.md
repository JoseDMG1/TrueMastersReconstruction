# Timed fire collider producers

IncreaseCollider and ChangeCollider were empty and are now restored in the
editable project. Source-derived evidence only until a subsequent runtime
report samples actual collider dimensions. These are active dependencies of
NetworkFireWall/NetworkFirePillar2, not a reason to modify scene/prefab sizes.

Corrected native listings: Evidence/native-disassembly-IncreaseCollider.txt and
Evidence/native-disassembly-ChangeCollider.txt. Input library SHA256:
d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c.
Named ISIL corroborates Invoke and BoxCollider get/set APIs; the printed Cpp2IL
disassembly itself is not correctly mapped.

- IncreaseCollider.Start0x129727C invokes IncreaseSize after serialized time.
  Slot0x2816F50 resolves IncreaseSize. IncreaseSize0x12972CC gets BoxCollider,
  preserves current size.x/size.z and sets size.y=4. It does not add4, multiply
  the size, change the center or resize every frame. Generic slot0x2814430
  resolves Component.GetComponent<UnityEngine.BoxCollider>.
- ChangeCollider.Start0x121C4CC invokes ChangeSize after delay. Slot0x2814818
  resolves ChangeSize. ChangeSize0x121C51C assigns the serialized newSize to
  BoxCollider.size. Constructor0x121C57C sets delay=1; this default is restored
  while preserving serialized overrides.

No original export, prefab, collider serialization, GUID or public field was
changed. Recommended acceptance observes a real fire directional attack's
spawned collider before/after its serialized delay and independently checks
resulting overlap. Valid animator entry/spawn alone does not accept these sizes.
