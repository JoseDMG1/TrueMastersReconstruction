# Stage music fades

FadeAudio was a recovered stub attached to Stage0's music object and other stage prefabs. Restored from corrected native Start0x122B1E8/Update0x122B2C0 and FadeIn.MoveNext0x122B530/FadeOut.MoveNext0x122B66C, retained under Evidence/native-disassembly-FadeAudio*.txt. Cpp2IL ISIL was used only for named AudioSource getters/setters/Play identity; its printed Disassembly is invalid for this ELF.

Start caches authored volume, sets0, enables loop, plays and caches clip.length. Update uses actual source.time when playing: fade in within fadeDuration at start, fade out after length-duration, single isFading owner. FadeIn increments deltaTime/duration*targetVolume per frame then assigns target exactly; FadeOut subtracts deltaTime/duration*initialVolume then sets0. Constructor default3 is preserved; Stage0 overrides duration4. Fields and .meta retained. Replacement iterators use real C# coroutines, with finally cleanup clearing isFading if explicitly disposed, a reconstruction lifecycle improvement.

No audible listening acceptance or captured audio waveform was performed. Build/runtime checks remain necessary; script restoration does not establish volume balance or equivalence of playback to videos.
