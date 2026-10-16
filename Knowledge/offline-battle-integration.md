# Integración offline: controlador central y escenario

Estado: reconstrucción parcial desde ARM64; la prueba runtime se guarda por separado. No es una declaración de equivalencia visual ni de partida completa.

## Procedencia

Se reutilizan el APK original, la biblioteca ARM64 suplementaria validada y el mapa `Evidence/cpp2il-native-address-map.json` documentados en HANDOFF.md. Los dumps corregidos `Evidence/native-disassembly-GameController.txt`, `Evidence/native-disassembly-StageController.txt` y `Evidence/native-disassembly-TagInChildren.txt` son evidencia de implementación nativa; el C# exportado era un stub. No usar la sección Disassembly de Cpp2IL sin corregir el desfase de archivo.

## Controlador

GameController.Awake 0x12911D0 y CreateCharacters_LM 0x1291684 reconstruyen la rama desconectada: los prefabs se cargan mediante Resources/PhotonPrefabs y las claves Character/CharacterCPU; posiciones (5,0,5) y (-5,0,-5). Los argumentos genéricos nativos justifican AddComponent de HostPlayer/GuestPlayer. Se asocian MainCharacter/Opponent a BattleSceneSingleton, se activan controles de CPU en los modos correspondientes y se dispara Start del Animator del contador. Las rutas online siguen pendientes y no se simulan como disponibles.

CompleteMainChar 0x12923C8 conserva referencias Animator/MobileInputs y configura los cinco índices de ataque cuando Description.moveset es 1. EnableInputs 0x1292888 establece gameStarted y habilita MobileInputs del personaje principal. Restart 0x128BE04 selecciona arena/bracket en torneo según draw, y selección para otros modos. La lógica de torneo circundante no está completa.

EnableSpecial, rama offline 0x1292FC4–0x1293208, usa los nombres exactos y HasKey de PlayerPrefs: Rina/Amon → Rina; Dante/Agata → Dante; Eliot/Vinea → Eliot; Sora/Aura → Aura; Diana/Ren → Ren. Los nombres incluyen el sufijo (Clone). No se infiere una habilidad para otros personajes. AI debe usar las mismas claves derivadas, en particular Dante y no Eliot.

## Escenario

StageController.Awake 0x12B2DD0 instancia stages[PlayerPrefs.GetInt("Stage",0)] en pose de su propio transform y busca el hijo inmediato etiquetado music. TagInChildren.FindGameObjectInChildWithTag examina los hijos inmediatos, incluyendo inactivos; no recorre descendientes arbitrarios. La reconstrucción añade un error explícito para índice inválido y una guarda de singleton duplicado. La música original continúa en los componentes recuperados, no en un recurso inventado.

## Verificación pendiente y herramienta

`Tools/Validate-OfflineFlow.ps1` ejecuta `OfflineFlowValidation.Run` en Play Mode del editor, desde DesarrolladoPor, invocando los callbacks reales SinglePlayerMode, Continue y SelectionController.start de los botones serializados. Espera dos luchadores y gameStarted; registra escenas, excepciones y resultado en Evidence/offline-flow-validation.json con un log fechado. No fuerza la arena ni reemplaza callbacks por carga directa. No prueba todavía combate completo, renderizado, temporización de cámara ni equivalencia con videos.

El último informe importado anterior no valida código recién editado. Solo un log y reporte nuevos pueden establecer que este checkpoint compila y llega a la arena. El orden Awake/Start de los singleton y la destrucción del contador deben verificarse antes de modificar sus consumidores por conjetura.
