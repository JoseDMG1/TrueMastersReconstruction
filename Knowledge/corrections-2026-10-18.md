# Correcciones solicitadas el 18 de octubre de 2026

Solicitud: `nuevas cosas a corregir.txt`. Cambios aplicados al proyecto editable y al player de `Builds/Windows`, recompilado correctamente el 18 de octubre a las 21:57 UTC (0 errores, 43 advertencias). La limitaci?n de licencia descrita m?s abajo corresponde al primer intento y qued? resuelta al repetirlo en la sesi?n del usuario.

## Cambios

- Dante y Agata: `LightingAnimatorController/JumpR` activa el reflejo humanoide. Se conservan el clip, la transición y el evento `goHorizontal(+.37)`; `JumpL` sigue sin reflejo y con `goHorizontal(-.37)`. Ambos personajes comparten este controlador.
- Sora y Aura, ataques laterales 2: `AirAtkL2` y `AirAtkR2` desactivan `Follow` y el apuntado al rival. `MoveAttack.fixedLocalDirection` captura el eje frontal de la instancia al crearse. `SummonAttack.CreateKickAttack` ya aplica el giro lateral de -90/+90 grados según el ataque; no se aplica un segundo giro. Tras la carga original de .75 segundos, el tornado recorre una dirección mundial constante a la velocidad original de .55 por paso fijo, aunque cambien de posición el rival o el propietario. El comportamiento de debilitamiento del ataque se conserva. Otros proyectiles mantienen su configuración anterior.
- Zoom: ambas vistas locales multiplican la distancia anterior por 1.25 (cercana 6.4 -> 8, lejana 9.6 -> 12). Las dos preferencias de cámara de un jugador también amplían su distancia un 25%. La presentación cercana del ganador conserva su encuadre.
- Raven: `IncreaseSize.IncreaseSmooth` interpola los tres ejes desde la escala actual, evitando estirar la profundidad hacia .99 durante cada paso. Se conservan todas las escalas serializadas de los seis animales y el crecimiento adicional de las trampas.
- Ganador local: se desactiva la segunda cámara y la división central, la cámara principal ocupa toda la pantalla y se vincula al ganador para la transición al baile. `LateUpdate` deja de seguir a ambos luchadores. El HUD de acciones se oculta cuando termina la batalla. Se reutiliza la ruta de resultados existente para cualquiera de los dos ganadores.
- Selección: los textos serializados de batalla, pausa, menú inicial y torneo pasan a `SELECT CHARACTER`, con ajuste de tamaño. Se actualizan también los tres campos de traducción correspondientes. Los callbacks conservan su destino de selección y, en torneo, su cobro existente.

## Evidencia nativa de Raven

Se aplicó la skill `reverse-engineer-anything` para revisar evidencia ARM64 previamente retenida, mediante herramientas locales normales; esta sesión no tiene herramientas MCP de REA conectadas y no produjo una nueva ejecución de REA.

`Evidence/native-disassembly-IncreaseSize__IncreaseSmooth_d__7.txt` muestra una segunda llamada a `get_localScale` en `0x1297840` (destino `0x23B139C`). Esa llamada devuelve x/y/z en s0/s1/s2. Después, `0x1297860` calcula originalZ-currentZ y `0x1297888` suma el incremento a currentZ. El valor .99 cargado antes en `0x129780C` pertenece al umbral del bucle y deja de estar en s2 tras la segunda llamada. La implementación anterior confundía ambos usos del registro. Esto corrige la reconstrucción; no es una nueva proporción artística inventada.

Biblioteca retenida: `Evidence/acquired/merged-public-apk/lib/arm64-v8a/libil2cpp.so`, SHA256 `d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c`.

## Verificación y límite actual

- Compilación C# directa con Roslyn y las referencias/configuraciones de Unity 6000.6.5f1: editor y Windows development, ambas con código de salida 0, 0 errores y 43 avisos. Logs en `Evidence/october-corrections-compile/`. Salidas aisladas en Evidence; no se sustituyeron DLL del ejecutable anterior.
- `node Tools/audit-october-corrections.mjs`: aprobado; 25 registros de recursos/verificaciones. Valida YAML, referencias de controlador y clips, eventos reales de creación de tornados, tiempo/velocidad, textos con ajuste y preservación de escalas frente a la exportación original. Informe: `Evidence/october-corrections-assets.json`.
- Se regeneraron los inventarios de roster/dependencias y los contratos de entrada/vuelo para no conservar el hash previo del controlador ni seguir clasificando estos tornados como proyectiles de seguimiento.
- El primer intento de build Unity quedó bloqueado por la licencia y el smoke inicial esperaba las distancias de cámara y botones táctiles anteriores. Ambos puntos se corrigieron y el build/smoke posterior pasó; ver “Recompilación posterior” al final.
- Prueba preparada: `OfflineOctoberCorrectionsProbe`, optativa y exclusiva de desarrollo. Cubre Dante/Agata, Sora/Aura, los seis animales, zoom y tercera caída con ambos ganadores. Usa entradas inyectadas y proyectiles aislados; no afirma entradas físicas ni equivalencia con vídeo. Se ejecuta después de un build válido con `Tools/Run-OfflineProbe.ps1 -Scenario OctoberCorrections`.

Quedan pendientes el escenario OctoberCorrections completo y la revisión visual de esquivas, animales, texto y baile. Copias de los archivos originales de este checkpoint: `Evidence/WorkingBackups/corrections-20261018`.

## Recompilaci?n posterior, 2026-10-18 21:57 UTC

Unity reconoci? la licencia en el nuevo intento. Build Windows final: Succeeded, 0 errores, 43 advertencias, 18.75 segundos; `Evidence/windows-build-validation.json` y `Evidence/unity-windows-build-20261018-final.log`. Se actualiz? la expectativa de distancia en el smoke test para incluir el zoom out 25%; el primer smoke se detuvo por esperar la distancia antigua, sin error de ejecuci?n de Unity. El escenario completo OctoberCorrections y la comparaci?n visual siguen pendientes.

El smoke test se adapt? tambi?n al HUD pasivo de PC: activa el ataque mediante InputReader/GameController, sin buscar botones t?ctiles activos. Estos cambios afectan al instrumento de prueba, no al comportamiento del juego.

Smoke final aprobado: `Evidence/offline-smoke-probe-20261018-165734.json`, sin errores; arranque, selección, movimiento, cámara con zoom out, ataque del jugador y actividad CPU. DLL del player SHA256 `c15befd179d65f606030a0e4a061d0c281324a6f4ae84d4be7b62579a24823b7`. Esta prueba breve no sustituye al escenario completo de correcciones ni a la comparación visual.
