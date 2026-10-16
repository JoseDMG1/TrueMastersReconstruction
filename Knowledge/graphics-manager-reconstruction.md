# GraphicsManager: ajustes de menú reconstruidos

Se implementaron los13 métodos existentes de UnityProject/Assets/Scripts/Assembly-CSharp/GraphicsManager.cs. Se conservaron todos los nombres, firmas y campos serializados; no se modificó Evidence exportada. El constructor implícito equivale a la única llamada MonoBehaviour del constructor nativo. No se añadieron métodos de gameplay ni se entró en Play Mode.

## Fuentes

- Evidence/native-disassembly-GraphicsManager.txt, decodificación Capstone5.0.6 de offsets ELF correctos.
- Evidence/Cpp2IL-ISIL/IsilDump/Assembly-CSharp/GraphicsManager.txt, solo sección ISIL; Disassembly incorrecto no se utilizó.
- Evidence/cpp2il-native-address-map.json, firmas, tokens y offsets de campos.
- Evidence/graphics-manager-native-constants.json, resolución de literales GameSpeed, mensaje de singleton, userQualityGraphics, userFrameCap y toogle mediante relocaciones ELF y metadata usage.
- libil2cpp.so SHA256d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c y metadata SHA256c79db4f28d92a0d5959ec74c3e8a2267b8838f7e924640096b5a811237eddfd4.

## Cobertura por método

| Método | Dirección virtual | Comportamiento reconstruido |
|---|---|---|
| Start | 0x12966D0 | Asigna i cuando está vacío; instancia duplicada registra mensaje exacto y Destroy(this), continúa inicialización. TimeScale1.25 inicial; GameSpeed0/1/2 establece1.15/1.275/1.4; print(timeScale); vSync0; objetivo60; inicializa resoluciones/calidad/límite. |
| OnDestroy | 0x1296B00 | TimeScale1. No limpia i explícitamente. |
| Update | 0x1296B0C | DeltaTime suavizado0.1; ceil(timeScale/deltaTime) y fpsText. |
| SetInitialRes | 0x1296BCC | Bajo35fps mínimo; bajo50 medio; desde50 original. No cambia framecap. |
| setMinRes | 0x1296BFC | Si altura actual difiere, calidad1, preferencia1, graphicsChange?.Invoke(), restaura vSync, aplica fullscreen; siempre actualiza colores. |
| setMedRes | 0x1296D00 | Igual con calidad/preferencia2 y tamaño medio. |
| setOriginalRes | 0x1296E04 | Calidad3 y resolución original siempre, sin comparar altura; callback antes de restaurar vSync. |
| CapFrameRate | 0x1296F68 | vSync0; cap30 o60; preferencia1/0; no depende de modo online. |
| toogleSettings | 0x1296FFC | debug.GetComponent<Animator>().SetTrigger(toogle). |
| setInitialFrameCap | 0x1296A68 | userFrameCap default0;0/1 aplican límite;1 enciende Toggle. |
| setInitialQualityGraphics | 0x12969A8 | Preferencia1/2/3 aplica tamaño; otros programan SetInitialRes tras5segundos. |
| setResolutions | 0x12968C0 | Display.main altura/ancho; medio75%,mínimo50%; si cualquiera dimensión es menor720, ambos tamaños usan originales. |
| restartButtonsColor | 0x1296ED4 | Las tres etiquetas negras RGB0 alpha1. |

Las constantes GameSpeed se leyeron de floats en ELF VA0x67E0E0:[1.149999976158142,1.274999976158142,1.399999976158142]. Vector de resoluciones VA0x652560:[0.75,0.75,0.5,0.5]. Suavizado VA0x651BA8:0.10000000149011612. Se usaron literales C# que representan esos mismos valores float32.

Las ramas FCMP/CSEL, las instrucciones SIMD y los colores RGB no estaban completos en ISIL; se resolvieron con los bytes nativos mapeados y Capstone. Se conservan diferencias observadas respecto a GameManager: umbrales35/50, tamaños75/50%, defaultFrameCap0, original siempre reaplica, callback graphicsChange y velocidades configurables. No se unificaron clases porque esas diferencias están respaldadas por evidencia.

## Limitaciones

Código reconstruido, no fuente original. Dependencias de UI/Animator y singleton tienen el mismo acceso directo observado; faltan referencias o inicialización de otros scripts puede fallar hasta completar su reconstrucción. No se añadió DontDestroyOnLoad, reinicio de singleton, guardas silenciosas ni return después de Destroy, porque no aparecen en el código nativo. La compilación final la coordina el agente principal; este subtrabajo no ejecutó Unity ni pruebas ni contactos de red del juego.
