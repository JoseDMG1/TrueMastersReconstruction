# Initial scale owner and battle pause overlay

Observed in `UnityProject/Assets/Scenes/Bending Battle.unity`: GameObject fileID
226, ExitMenu, is active and holds RectTransform 1005, ChangeStartScale 1418 and
ExitMenu 1695. ChangeStartScale has serialized `scale: {x: 0,y: 0,z: 0}`;
ExitMenu has `open: 0`. Its scale owner previously had an empty Awake stub.

Corrected ELF-native `ChangeStartScale.Awake` at VA `0x121E73C` / raw offset
`0x121A73C` gets its own Transform, reads the component Vector3 at offsets
0x20/0x24/0x28 and calls Transform.set_localScale. Source retained in
`Evidence/native-disassembly-ChangeStartScale.txt`. Source binary SHA-256:
`d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c`.

Restored Awake as `transform.localScale = scale`. This uses the existing authored
initialization component; no ExitMenu lifecycle method or scene-scale edits were
added. It should hide ExitMenu initially and let its existing LerpManager open/
close methods control later scale. Runtime visibility remains the main agent's
verification responsibility; this pass did not launch Unity.

Bounded search found ChangeStartScale references only in the battle scene and
flowerBonfire, GrassSpecialFX2, Maya prefabs. Those authored scales now initialize
through the same owner. This is not a broad audit of all UI initialization scripts.
