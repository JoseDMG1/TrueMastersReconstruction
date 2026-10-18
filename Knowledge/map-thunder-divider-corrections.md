# Mapas, luz del rayo y separación de cámaras

## HUD durante la carga

`DesktopCombatHud` ya ocultaba los indicadores cuando el menú de pausa estaba activo. La ruta del botón de pausa inicia `LevelChanger.FadeToLevel`, que activa `LoadingScreen.loading` antes del fundido y de mostrar la pantalla de carga. El HUD ahora también se desactiva mientras esa bandera esté activa, evitando que A/B/S se queden encima de la carga al salir a selección de personaje o al menú principal.

La build resultante `Evidence/unity-windows-build-20261018-175458.log` terminó con 0 errores y 44 advertencias; ensamblado `Assembly-CSharp.dll` SHA-256 `f40e2d447fc220f10074e3e43389f8abe7a3f07f334de771fe00d9abcfebbe1c`. No se ejecutó una prueba de juego después de este cambio de visibilidad.

## Revisión de música del tercer mapa

El selector muestra “Mapa 3” para `actualStage=2`; el arreglo de la escena apunta ese índice a `Stage2.prefab`, cuyo AudioSource activo usa `animeMusic.ogg`. El export retenido contiene la misma asignación. `music3.ogg` está en `Stage3.prefab`, que se muestra como “Mapa 4”. No se cambió audio sin evidencia de una asignación distinta en el proyecto original.

Correcciones solicitadas el 18 de octubre de 2026, después de la selección local dividida.

## Miniatura y mapas disponibles

`LocalOfflineSelection` creaba la miniatura con el mismo método que los fondos oscuros. Su `Image.color` multiplicaba la textura por un color casi negro. La imagen del mapa ahora usa blanco opaco y conserva su proporción dentro del panel superior central.

`StagesManager.unlockAllMaps` está activado por defecto para esta build. `SetPlayableStages` conserva los siete escenarios y la opción aleatoria incluso con los campos de propiedad de los mapas en cero. No se escriben compras ni cambios de propiedad en el guardado. Desactivar ese campo en el inspector permite recuperar el filtro anterior para futuras builds.

## Luz de los ataques eléctricos

`LightOn` tenía vacíos `Start`, `On` y `Off`; los `Light` de `SumonThunderAtk2` y `SumonThunderAtk` permanecían encendidos hasta la destrucción de sus objetos. Se restauraron el encendido y apagado gradual con los parámetros existentes del prefab.

La skill REA se usó como guía. No había herramientas REA conectadas y su CLI 6.5.0 falló durante `doctor` por una importación ausente de `@modelcontextprotocol/client/dist/index.mjs`. No se modificó la configuración del cliente. La evidencia usada procede del ARM64 original conservado localmente, con el decodificador Capstone existente y los volcados de metadatos/ISIL; estos resultados son de herramientas locales, no de un proveedor REA conectado.

- `Evidence/native-disassembly-LightOn.txt`: `Start` 0x1241CB4 guarda la intensidad inicial, pone la luz a cero y arranca las corutinas; `off` decide si se ejecuta el apagado. El constructor define `duration=0.5` y `delayOff=2`.
- `Evidence/native-disassembly-LightOn__On_d__5.txt`: `MoveNext` 0x124208C incrementa la intensidad en pasos de 0.15 con esperas de 0.02 segundos escalados y restaura el máximo al terminar.
- `Evidence/native-disassembly-LightOn__Off_d__6.txt`: `MoveNext` 0x1241E9C espera `delayOff`, resta 0.15 por paso con la misma espera y termina en intensidad cero.
- Constantes leídas de los segmentos ELF: VA 0x651B0C = +0.15, 0x651BA0 = -0.15, 0x651BC8 = 0.02. Los prefabs eléctricos ya definen `duration=0.5`, `off=true`, `delayOff=0.7`; no se cambian sus tiempos de daño, colliders, posición ni creación del rayo.

La implementación añade protección para un `Light` ausente o duración no positiva. Las cuatro familias de prefabs que usan este componente son `LightLandAtkFX`, `SumonLightAtk`, `SumonThunderAtk` y `SumonThunderAtk2`.

## Línea divisoria

La línea de la batalla vivía en un lienzo independiente de tipo Overlay con orden 150. Ese lienzo se dibujaba después de la interfaz del juego, que utiliza su propia cámara de UI. Reducir solamente el número de orden no resuelve de forma fiable esa diferencia entre modos de renderizado.

`LocalOfflineSplitScreen` ahora inserta su lienzo como primer hijo del lienzo del marcador, sin sobrescribir el orden. Así la línea se dibuja antes que los controles, el marcador y la pausa en la misma jerarquía. Su imagen no recibe raycasts. Se mantiene el comportamiento de ocultarla durante la cámara del ganador. La creación espera un fotograma para que `Score` y TMP completen `Awake`. El lienzo y la imagen heredan la capa UI 5 del marcador: la cámara de UI solo renderiza esa capa (máscara 32).

## Comprobación

Compilar con `Tools/Build-OfflinePlayer.ps1`, después ejecutar secuencialmente:

1. `Tools/Run-OfflineProbe.ps1 -Scenario LocalSelectionUI`: disponibilidad de los ocho elementos con propiedad guardada cero, sprite/tinte/posición de miniaturas, botones UGUI independientes, transición a batalla y profundidad real de renderizado respecto al marcador y la pausa abierta. Se comprueba también que la cámara de UI incluya la capa de la línea. `LocalSelection` conserva la prueba separada con teclado físico, que necesita foco de Windows.
2. `Tools/Run-OfflineProbe.ps1 -Scenario ThunderLight`: Dante y Agata entran en `DownAttack` y luego `DownAttack2` mediante los callbacks de producción `InputReader`/`GameController`, con alternancia natural. El escritorio oculta las zonas UGUI del móvil. Se observa la invocación en el rival, la creación del rayo y las intensidades de las luces hasta que llegan a cero, antes de la destrucción del prefab.

Las pruebas usan personajes desbloqueados temporalmente y CPU controlada. Restauran preferencias y archivos guardados. No prueban entrada física de mando ni equivalencia visual completa con el juego original.

## Aceptación de la build final

- Compilación `Evidence/unity-windows-build-20261018-174354.log`: Unity 6000.6.5f1, Succeeded, 0 errores y 44 advertencias, 22:44:28 UTC. Ejecutable `Builds/Windows/TrueMastersOffline.exe`.
- DLL SHA-256 `ab58670d922f35df84e03748b228565bc2f62b091582b8b421e533b74138169e`.
- `Evidence/offline-localselectionui-probe-20261018-174443.json`: passed, errores vacíos. Ocho miniaturas con geometría/tinte/sprite válidos, todos los mapas disponibles con propiedad guardada cero, transición local con mapa 1, línea con capa renderizable y profundidad inferior al marcador y pausa. Capturas `.map-6.png` y `.pause.png` inspeccionadas: miniatura en el panel superior central y línea visible sin cortar las letras.
- `Evidence/offline-thunderlight-probe-20261018-174517.json`: passed, errores vacíos, misma DLL final. Dante y Agata recorrieron DownAttack→DownAttack2 sin forzar estados; se invocó el marcador en el rival y se creó el rayo. Las tres luces observadas por personaje alcanzaron intensidad 5 y después cero a los 1,218–1,222 segundos escalados desde su aparición, antes de la destrucción del objeto.
- Los intentos de teclado anteriores perdieron el foco de Windows; se conserva su alcance parcial. La nueva variante UI usa los botones reales para probar esta corrección de presentación. Los primeros ensayos del rayo intentaban una zona UGUI móvil que el escritorio oculta; la prueba final usa los callbacks de producción sin forzar estados de Animator. El guard final de CombatTrip conserva ahora la descripción específica del escenario y evita sustituirla por la de doce especiales.
- La prueba visual abre `ExitMenu.OpenMenu`, la ruta del engranaje. No usa `GameController.pauseGame`, que además activa el panel de ayuda móvil heredado. No se cambia ese comportamiento ajeno a la petición.
