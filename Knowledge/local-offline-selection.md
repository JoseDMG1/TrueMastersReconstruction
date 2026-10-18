# Selección dividida para dos jugadores locales

Al confirmar que ambos jugadores están listos, el modelo de Player 1 conserva el `Cheer` disparado por `SelectionController.start()`. `LocalOfflineSelection` dispara también `Cheer` en el modelo elegido por Player 2, salvo que ambos eligieran el mismo modelo.

Esta corrección se compiló en `Evidence/unity-windows-build-20261018-180736.log` (0 errores, 44 advertencias), con `Assembly-CSharp.dll` SHA-256 `5b19222789af4185e15dafd67486316bf99424a60f7baf39ffd07c2f4ccb2eca`. No se ejecutó una validación dentro del juego para esta animación.

La petición de `cosas a implementar.txt` se aplica exclusivamente a `GameMode.LocalOffline` (2 Players → Local Offline).

`SelectionController.Start` añade `LocalOfflineSelection`. Esta presentación usa dos cámaras con las vistas originales del expositor: la izquierda sigue la elección del jugador 1 y la derecha la del jugador 2. Ambos pueden elegir el mismo personaje. El carrusel recorre los personajes disponibles, incluida la elección aleatoria, y guarda `Character` / `CharacterCPU`, que ya consume la creación de la partida.

Cada jugador usa `LocalControls.ReadPlayer`: izquierda/derecha cambian de personaje, ataque confirma y esquiva cancela su confirmación. Se respetan las teclas reasignadas, el perfil alternativo y el dispositivo elegido de cada jugador. El stick tiene umbral y repetición retardada. Los botones de pantalla permiten usar el ratón. La navegación automática del EventSystem se suspende en esta pantalla para evitar que las mismas teclas activen controles del otro jugador.

El panel superior central reúne la miniatura del mapa, Game Speed y la duración existente. Estos ajustes se manejan con el ratón y quedan bloqueados desde que un jugador confirma; cancelar la confirmación permite modificarlos de nuevo. La partida empieza cuando ambos están listos. El menú conserva el botón original de volver y la transición original a la arena.

La selección normal de un jugador, torneo y LAN conserva su presentación existente. No se modifican los prefabs del expositor ni las reglas de combate.

## Verificación reproducible

1. Ejecutar `Tools/Build-OfflinePlayer.ps1` sin un jugador propio abierto.
2. Ejecutar `Tools/Run-OfflineProbe.ps1 -Scenario LocalSelection`.
3. Revisar el informe y las capturas en `Evidence/offline-localselection-probe-*.json`, `.selection.png` y `.Bending-Battle.png`.

El escenario usa temporalmente personajes desbloqueados y asignaciones de teclado conocidas. Envía eventos reales de teclado de Windows exclusivamente a la ventana del jugador enfocado, prueba ambos perfiles, comprueba el aislamiento entre cámaras/selecciones, la confirmación/cancelación, el bloqueo de ajustes y los personajes y velocidad que recibe la batalla. Restaura las preferencias y los archivos de guardado al terminar. La selección con mandos físicos y todas las relaciones de aspecto requieren comprobación manual adicional.

## Resultado del 18 de octubre de 2026

- Compilación final: `Evidence/unity-windows-build-20261018-171652.log`; Unity 6000.6.5f1, Succeeded, 0 errores y 43 advertencias. Informe `Evidence/windows-build-validation.json`, generado a las 22:17:25 UTC.
- Prueba final: `Evidence/offline-localselection-probe-20261018-171732.json`, `passed`, errores vacíos. Teclas físicas E/H reasignadas y D/RightArrow alternativos, Z/X para confirmar/cancelar y U para confirmar al segundo jugador. La arena recibió Diana y Eliot, mapa 0 y velocidad 1.15.
- Captura `.selection.png` revisada a 960×540: panel de ajustes superior central, nombres/controles y personajes centrados en cada mitad. Las cámaras corrigen el desplazamiento horizontal del expositor original y retroceden un 35 % de su profundidad para dejar espacio a la interfaz.
- DLL de juego final: SHA-256 `3d71e99bbc6edbfaee6f8a00f5008ef58f6b2d224f0949a426cfa62930070710`.
- Regresión de un jugador con la misma DLL: `Evidence/offline-smoke-probe-20261018-171804.json`, `passed`, errores vacíos; menú/selección/arena, movimiento, cámara, proyectiles y actividad de la CPU comprobados.
- Los primeros intentos del observador no encontraron un callback persistente en el botón creado en ejecución y no consiguieron enfocar una ventana oculta. Se corrigió el observador para pulsar el botón real y encontrar la ventana propia de Unity antes de enviar teclas. Esos informes no sustituyen el resultado final.
