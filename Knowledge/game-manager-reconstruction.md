# GameManager: reconstrucción de ajustes desde código nativo

Archivo de trabajo: UnityProject/Assets/Scripts/Assembly-CSharp/GameManager.cs. Se reemplazaron los13 métodos vacíos por implementaciones; nombres, firmas y campos serializados existentes conservados. No se modificó ninguna exportación de Evidence.

## Evidencia y decisiones

Fuente binaria SHA256 d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c; metadata original c79db4f28d92a0d5959ec74c3e8a2267b8838f7e924640096b5a811237eddfd4. Mapa de métodos/campos: Evidence/cpp2il-native-address-map.json. ISIL: Evidence/Cpp2IL-ISIL/IsilDump/Assembly-CSharp/GameManager.txt (solo sección ISIL). Desensamblado corregido usando offsets ELF: Evidence/native-disassembly-GameManager.txt, Capstone5.0.6 portátil. Los comentarios del código enlazan cada método con su dirección virtual.

- Start0x12946AC establece Time.timeScale=1.25 y prepara resoluciones/calidad/framecap.
- Update0x12948F0 suaviza deltaTime con factor0.1 y muestra ceil(timeScale/deltaTime). La constante0.1 se leyó en ELF VA0x651BA8; FDIV/FRINTP se verificaron en bytes correctamente mapeados.
- setResolutions0x12946DC toma Display.main.systemHeight/systemWidth; SIMD float vector en VA0x6525A0 contiene [0.6667000055,0.6667000055,0.3337000012,0.3337000012]. Se multiplican altura/ancho y truncan a int para los tamaños medio/mínimo. No se sustituyeron por2/3 y1/3 porque difieren de los bytes originales.
- setInitialQualityGraphics0x1294798 usa PlayerPrefs userQualityGraphics (default0); valores1/2/3 seleccionan mínimo/medio/original. Otros valores programan SetInitialRes después de5 segundos.
- SetInitialRes0x12949B0 selecciona mínimo bajo10fps, medio bajo30fps (ambos limitan a30), medio entre30y45 y original desde45. Las ramas FCMP ausentes de ISIL se comprobaron con Capstone.
- setMinRes0x1294A30, setMedRes0x1294BFC, setOriginalRes0x1294CE8 comparan solo altura actual antes de cambiar; usan quality1/2/3, guardan preferencia, conservan vSyncCount y aplican Screen.SetResolution fullscreen=true. Siempre ponen etiquetas negras y selección blanca. No se añadió interpretación por ancho.
- CapFrameRate0x1294B1C lee GameController.i.online: cap=true y offline da vSync0/30fps/userFrameCap1; cualquier otra combinación vSync1/60fps/userFrameCap0.
- setInitialFrameCap0x1294858 usa default-1;0/1 aplican preferencias,1 activa Toggle. No hace nada en ausencia de preferencia.
- changeQuality0x1294E68 convierte valor del slider a int (trunca) y aplica QualitySettings.SetQualityLevel(...,true). Solo se afirma equivalencia para valores finitos dentro del dominio UI; conversiones no finitas/extremas no verificadas en ejecución Unity.
- toogleSettings0x1294E8C obtiene Animator de GraphicsManager.i.debug y dispara el trigger exacto toogle. El nombre está respaldado por literal metadata y el tipo por metadata usage.
- restartButtonsColor0x1294DD4 usa color negro RGB0 y alpha1, recuperados de las instrucciones MOVI+FMOV.

Evidence/game-manager-native-constants.json conserva slots ELF, relocation addends, encoded metadata usage, índice de literal y offset de texto para userQualityGraphics/userFrameCap/SetInitialRes/toogle, además de los tipos GameController/GraphicsManager. Tools/resolve-native-usage.cjs reproduce la resolución; recibe direcciones virtuales reales del slot, no las direcciones simplificadas de ISIL.

## Límites y dependencias

Se trata de código C# reconstruido, no del código fuente recuperado. No se usó Disassembly del volcado Cpp2IL, cuya lectura de offsets es incorrecta para este ELF. No se entró en Play Mode ni se ejecutó el APK ni se contactó un backend. No se ejecutaron tests; la validación de compilación del proyecto la coordina el agente principal.

Los originales dereferencian GameController.i, GraphicsManager.i y referencias UI; se conservaron esas dependencias sin inventar otra propiedad de recursos ni silenciamiento. Su inicialización depende de otros scripts aún vacíos/reconstruidos por separado. La implementación y los nombres de Unity APIs se apoyan en llamadas resueltas por Cpp2IL más sus declaraciones recuperadas; la equivalencia visual/runtime sigue pendiente.
