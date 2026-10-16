# True Masters: evidencia visual y especificación pendiente

Fecha de inspección: 2026-10-15. Este documento describe lo comprobado visualmente en los tres videos suministrados. No establece que el juego completo esté comprendido ni que se haya recuperado su código.

## Fuentes y método

Carpeta original: C:\Users\Jose\Desktop\true masters videos gameplay y menú

- **M**: true masters gameplay multiplayer with a friend.mp4, 848,6 s.
- **C**: true masters gameplay singleplayer all charactersmp4.mp4, 1003,2 s.
- **S**: true masters gameplay singleplayer mode survival (with the principal menú options).mp4, 264,5 s.

Se inspeccionaron las siete hojas de contacto anteriores que cubren los tres videos de forma dispersa. Se generaron y revisaron hojas adicionales con muestras nominales cada dos segundos: inicio y cierre de S; inicio, tramo 470–524 y cierre de M; inicio y transiciones 174–196, 380–402, 646–668, 832–854 de C. Se guardan en video-frames/. Las etiquetas suman el desplazamiento indicado al tiempo del mosaico; la selección de fps puede desplazar la imagen hasta aproximadamente un segundo. Se extrajo además tutorial-5s.png, fotograma exacto a los 5 s. tutorial-4s.png quedó negro y no se usa como evidencia.

No se escuchó el audio ni se observó cada fotograma. No se interactuó con el juego. Los intervalos amplios indican cobertura por muestras, no visualización continua. Los tiempos de resultados son límites aproximados y deben volver a precisarse si se necesita sincronización exacta. video-findings.json contiene 45 hallazgos con procedencia y categoría observed, inferred o unknown.

## Navegación observada

**Menú principal** [S 00:00–00:03; M 14:00–14:07]: logo TRUE MASTERS con símbolo de elementos y seis botones en dos filas:

| Fila | Izquierda | Centro | Derecha |
|---|---|---|---|
| 1 | 1 PLAYER (VS CPU) | 2 PLAYERS (ONLINE) | SHARE GAME |
| 2 | SURVIVAL | TOURNAMENT | EXIT GAME |

Abajo aparecen iconos de Instagram, YouTube y Discord. La imagen de fondo es una arena gris con horizonte luminoso; el cierre online muestra PLAYER HAS DISCONNECTED. La existencia de SHARE GAME y TOURNAMENT es observada; no se vieron sus flujos.

**Supervivencia** [S 00:05–00:28]: tutorial HOW TO PLAY → CONTINUE → carga → selección de personaje DIANA con flechas → selector de escenario → START → carga → arena. La selección muestra HIGH SCORE 11 y dos contadores con iconos de trofeo y diamante: 8 y 1795. QUIT aparece arriba a la derecha. El escenario se selecciona mediante miniaturas y una ventana SELECT STAGE con imagen grande, flechas, SELECT y CLOSE. Se observa un escenario marrón plano y otro de piedra verdosa con cielo de nubes. No se ha contado el catálogo completo de escenarios.

**CPU** [C 03:02–03:05; 06:28–06:34; 14:02–14:07]: CHOOSE YOUR CHARACTER, nombre y flechas, miniaturas de escenario, selección CPU con flechas y nombre del oponente o ?, selección Lv 3 con flechas, START/READY!, QUIT, contadores. Se observa cambiar el rival SORA → DIANA → ?. El símbolo ? probablemente significa selección aleatoria; eso es inferencia. La disponibilidad de otros niveles no quedó comprobada.

**Online** [M 00:00–00:37]: SERVER - EU, CHANGE desplegable, código 8157-eu, SHARE CODE, WAITING FOR OTHER PLAYER, QUIT. Aviso de juego en desarrollo que recomienda jugar con amigos. Mensajes CONNECTION SUCCESSFUL y A PLAYER HAS ENTER THE MATCH. Después aparece la selección de personaje con START y GET READY!; sigue una carga y la arena. El video empieza dentro del lobby: no muestra cómo se creó ni cómo se une el segundo jugador.

**Resultados CPU** [C 02:56–02:59; 13:54–13:58; cierre 16:40 aproximadamente]: WINNER, cámara cercana al ganador, RESULTS, RANK B, diamantes 8, PLAY AGAIN. El último resultado muestra RANK A y diamantes 10. Se observa el saldo subir 97→105, 105→113, 113→121, 121→129 y finalmente 139 en distintos resultados. No se conoce la fórmula del rango.

**Resultados online** [M 08:24–08:26; 13:40–13:54]: WINNER, PLAY AGAIN, OPEN CHAT, EXIT GAME. Una hoja anterior también muestra REMATCH? y READY!; falta precisar el momento exacto y la relación entre ambas pantallas.

**Chat online** [M 13:42–13:52]: ventana CHAT con X, historial, campo Enter message..., SEND. Se ven etiquetas (Opponent) y (You said) y Your opponent is typing...; se recibe gg y se envían aww :/ y gg. El teclado Android usa una barra de texto superpuesta con OK.

## Controles comprobados en el tutorial

Fuente exacta: S 00:05, video-frames/tutorial-5s.png.

| Entrada | A: ATTACK | B: DEFENSE |
|---|---|---|
| Joystick neutro | BASIC ATK | BLOCK |
| Joystick derecha | RIGHT ATK | JUMP R |
| Joystick izquierda | LEFT ATK | JUMP L |
| Joystick abajo | DOWN ATK | BACKFLIP |
| Joystick arriba | UP ATK | ROLL |

MOVE está asociado al joystick. El tutorial imprime BASIC ATK (LV1), DOWN ATK (LV2), UP ATK (LV3). El nivel podría ser disponibilidad de movimiento o configuración de dificultad; hay que comprobar su semántica con código o interacción. No se inventa que el juego disponga de salto mediante un botón independiente.

Durante combate se ven joystick abajo a la izquierda, A y B abajo a la derecha, y un tercer botón por encima con X al inicio de varios duelos y S más tarde. Su cambio y color/relleno están observados; no se conoce el contrato exacto de activación, coste ni recarga. Debe mantenerse configurable hasta tener evidencia.

## Personajes y elementos

Hay **al menos ocho nombres** leídos en selección; cinco demostraciones elementales no equivalen a cinco personajes totales.

| Nombre observado | Evidencia | Apariencia visible | Relación elemental |
|---|---|---|---|
| SORA | C 03:02 | Varón de pelo claro, gris/negro | Aire: primera demo y transición son compatibles; inferencia de asociación |
| VINEA | C 03:04 | Mujer encapuchada azul/gris | Agua: selección seguida por demo captionada water |
| DIANA | S 00:14–00:20; C 06:34 | Mujer de pelo corto, negro/amarillo | Tierra: selección seguida por demo captionada earth |
| BLAZE | C 11:02 | Varón barbado con capucha roja/dorada | Fuego: selección seguida por demo captionada fire |
| REN | M 00:34; C 14:04 | Varón musculoso, ropa marcial negra/amarilla | Tierra probable; no equivalencia de estadísticas comprobada |
| RINA | M 08:34 | Mujer de pelo oscuro recogido y ropa roja | Fuego probable por efectos del combate siguiente |
| DANTE | C 14:06 | Caballero con espada y escudo | Rayo: selección seguida por demo captionada Lightning |
| AURA | M 00:36 | Mujer de pelo claro y armadura/ropa oscura | Rayo probable por efectos; no equivalencia comprobada |

Efectos visibles por demostración: aire con arcos/golpes dorados, ráfagas y columna de viento; agua con columnas y dispersión de efectos acuáticos o similares a hielo; tierra con roca elevada/arrojada, formación vertical y varias rocas flotantes; fuego con proyectil ardiente, columna, líneas y áreas amplias de llamas; rayo con brillo azul sobre el luchador y destellos en objetivos/suelo. Estos son tipos visuales. Las muestras no vinculan sin ambigüedad cada efecto a una dirección de joystick.

## Combate, HUD y arenas

La cámara se sitúa detrás del personaje controlado y cambia su distancia/ángulo con el movimiento. Se observan arenas finitas con bordes abruptos, superficies marrón/arena, piedra verde con nubes y una plataforma gris nocturna. No está demostrada la malla, escala ni topología original.

El HUD de duelos muestra resultado central de dos lados, hasta 2-2 y luego 3-2 o 2-3, antes de WINNER. Esto apoya la inferencia de primero en ganar tres puntos/rondas. Falta establecer con precisión qué impactos conceden punto y si el límite es configurable. Los pequeños rombos contiguos al marcador cambian; su semántica exacta no se ha comprobado.

Hay SETTINGS, icono de ojo y X; en CPU se observa pausa y CPU, en online ayuda ? y chat. No se abrieron ajustes en los fotogramas inspeccionados. Una cuenta regresiva grande y GO! preceden al combate. También se observan animaciones de lanzamiento, evasión/salto, bloqueo esférico, caídas y reaparición a distancia; la causalidad exacta de cada movimiento no se puede fijar mediante muestras separadas.

## Supervivencia y recompensas

S 00:28–04:12 muestra SCORE 0 progresando hasta SCORE 10, con DIANA en escenario verde. Hay oponentes/ataques de varios estilos. La interpretación de sucesión de enemigos es probable; no se ha medido la regla de aparición.

En S 04:12–04:14 el personaje cae bajo el borde; después aparecen RESULTS, SCORE 10, diamantes 20, BONUS diamantes 25, WATCH AD y PLAY AGAIN. El saldo pasa 1795→1815 y HIGH SCORE previo era 11. Es compatible con dos diamantes por punto en esa partida. No prueba una fórmula universal, ni que BONUS se conceda sin anuncio, ni cuál es la condición de dificultad/fin del modo.

## Lista de cobertura para reconstrucción

| Área | Qué ya tiene evidencia | Qué falta comprobar antes de afirmar fidelidad |
|---|---|---|
| Menú principal | Seis textos y disposición, iconos sociales | Destinos externos, atrás, salir, localización |
| Tutorial | Tabla de diez acciones y etiquetas de nivel | Entradas sostenidas, zona muerta, prioridad, combos |
| Roster | Ocho nombres/modelos y cinco familias visuales | Catálogo completo, desbloqueos, variantes, estadísticas |
| Selección CPU | Rival, ?, Lv 3, escenario | Rango de niveles, política aleatoria, guardado |
| Selección survival | Personaje, escenario, high score | Todos los escenarios, récord por personaje/modo |
| Online | Lobby EU, código, éxito/entrada, chat | Crear/unirse, regiones, errores, timeout, protocolo, backend |
| Movimiento | Joystick, evasión y caída visibles | Velocidad, aceleración, orientación, gravedad, cámara |
| Ataques | Cinco familias elementales | Cada ataque por entrada, alcance, daño, colisión, coste |
| S/X | Botón y cambio de estado | Desbloqueo, energía, recarga, poder especial |
| Puntuación | Marcadores y victoria a tres en ejemplos | Reglas exactas de punto, empate, rondas, reset |
| Supervivencia | Score creciente, caída y resultado | Aparición/escalado de enemigos, curva y reglas |
| Progresión | Saldo, rank B/A y premios observados | Fórmulas, persistencia, compras, inventario |
| Anuncios | WATCH AD y bonus visible | SDK, disponibilidad, recompensa, cancelación/error |
| Ajustes | Botón SETTINGS | Panel completo, audio/gráficos/controles, persistencia |
| Torneo | Botón TOURNAMENT | Flujo y reglas completos |
| Recursos | Apariencia y efectos grabados | Modelos, rigs, clips, materiales, sonidos originales |
| Audio | Sin revisión | Música, efectos, mezcla y feedback |
| Plataformas | Video móvil en horizontal | Resoluciones, safe area, rendimiento, dispositivos |
| Errores | Mensajes de conexión/desconexión | Matriz de fallos, recuperación y datos locales |

## Próximas evidencias útiles

1. Extraer de forma estructurada escenas, objetos, scripts recuperables, nombres de ataques y tablas de configuración del APK Unity; cruzar versiones con la grabación.
2. Grabar o inspeccionar interactivamente SETTINGS, TOURNAMENT, todas las páginas de selección/tienda/desbloqueo y cómo crear/unirse a sala.
3. Pruebas de una sola entrada por vez: neutro y cuatro direcciones con A y B, luego S, para cada familia; conservar presión/liberación y tiempos. Medir sobre video a velocidad normal, no inferir cooldown de la distancia entre capturas.
4. Capturar reglas de punto, caída, defensa, simultaneidad, reaparición y fin de supervivencia.
5. Definir autoridad de multijugador y un servicio propio si no se recupera el backend. Una interfaz con códigos no demuestra un servidor funcional.
6. Revisar audio y recursos. El corpus RAG debe recuperar cada hallazgo con su ID y su intervalo; los unknown no deben convertirse en hechos al generar Unity.

