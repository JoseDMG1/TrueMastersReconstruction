# Plan de multijugador LAN para dos PCs Windows

Estado: borrador de diseño con alcance y transporte acordados. Los detalles del protocolo y la autoridad de combate requieren auditoría del código conservado. No se ha implementado esta función.

## Resultado esperado

Dos PCs Windows conectados a la misma red Ethernet o Wi-Fi pueden jugar una partida 1 contra 1 sin requerir Internet, cuenta, Photon ni búsqueda pública. El anfitrión crea la sala y la aplicación le muestra su IP local. El invitado escribe esa IP y se conecta. La primera versión no usa nombre de sala, contraseña ni búsqueda automática.

Cada equipo controla un luchador y cada jugador elige su personaje en su propio PC. El anfitrión fija escenario, velocidad y duración. Si un jugador se desconecta, el otro recibe un aviso y vuelve al menú LAN; no hay reconexión automática en la primera versión.

## Decisiones acordadas

- Plataforma inicial: dos PCs Windows.
- Red: LAN privada por Ethernet/Wi-Fi; no se necesita acceso a Internet.
- Unión: conexión manual a la IP del anfitrión.
- Anfitrión: crea partida, comparte IP y decide los ajustes del encuentro.
- Invitado: introduce la IP; elige su luchador.
- Sala: sin nombre, contraseña, catálogo público ni búsqueda automática en esta entrega.
- Desconexión durante el juego: aviso y retorno al menú LAN; sin reanudación.
- Transporte: conservar los contratos TCP/UDP originales y completar los métodos vacíos tras verificar sus payloads y responsabilidades.
- Validación: habrá acceso a dos PCs Windows para probar la misma build en la LAN.

## Estado del proyecto y evidencia

- La escena `UnityProject/Assets/Scenes/LocalMultiplayer.unity` ya contiene UI de anfitrión/invitado, selección, espera y combate local en red.
- `CreateRoomManager`, `JoinRoomManager`, `RoomMessageHandler`, `LocalMultiplayerData`, `CharacterSelection_LM`, `TCPConnection`, `UDPConnectionManager`, `BattleTasks_LM` y los controladores `_LM` tienen métodos vacíos en el C# actual.
- `NetFastEvent` conserva eventos de sala, selección, escena, entradas, posiciones, temporizador, ataques, efectos, puntuación y cierre. El enum es evidencia del contrato, no garantía de que los flujos estén completos.
- Hay una escena `Bending Battle` y una ruta `GameMode.LocalMultiplayer`; `GameController.Awake` sale por una rama específica del modo local. La integración exacta con `BattleTasks_LM` se debe recuperar antes de conectar UI y partida.
- El export del proyecto incluye Photon para los modos online. El flujo LAN tiene clases TCP/UDP propias; la implementación debe mantener separados LAN e Internet.
- Los cuerpos nativos/ISIL conservados pueden ayudar a reconstruir el comportamiento, pero el C# exportado está incompleto. Toda regla de protocolo se marcará como confirmada, inferida o decisión nueva.

## Arquitectura propuesta

Modelo anfitrión/invitado con autoridad del anfitrión. La conexión inicial verifica protocolo y versión de build, limita la sala a un invitado, confirma que ambos están listos y sincroniza la selección y ajustes antes de entrar a la escena de batalla.

La dirección IP manual elimina la dependencia de broadcast para encontrar salas. Se conservarán las rutas TCP y UDP originales. Antes de implementar se auditará el código nativo disponible para documentar qué canal transporta cada evento y los formatos de payload; no se cambiará el protocolo por conveniencia sin actualizar la decisión y comprobar compatibilidad. No se usará UDP broadcast para descubrir salas en el MVP.

La recepción de red debe salir de los hilos de socket a una cola segura y procesarse en el hilo principal de Unity. Toda operación tendrá timeout/cancelación, mensajes de error claros y cierre de sockets al salir, cancelar o desconectarse. La IP que se muestre debe pertenecer a una interfaz LAN activa; el UI debe explicar el bloqueo de firewall si no se acepta la conexión.

## Fases de implementación

1. **Recuperar y congelar contratos.** Mapear botones/callbacks de la escena, puertos serializados, eventos del enum, secuencias nativas disponibles, payloads y papeles de host/invitado. Identificar cuál clase de conexión usa realmente cada panel y batalla. Resultado: tabla de mensajes y flujos del protocolo original, con elementos confirmados e inferidos, antes de completar métodos.
2. **Conectar salas.** Implementar anfitrión TCP, IP LAN visible/copiar, unión por IP, validación y timeout, una sola conexión, cancelar, errores de IP/puerto/firewall, y cierre limpio. Aceptación: dos procesos Windows en loopback se conectan y se rechazan correctamente los intentos inválidos.
3. **Sincronizar lobby.** Cada PC elige su personaje; solo el anfitrión cambia escenario/velocidad/duración. Sincronizar ready, cambios, pérdida del invitado y señal única de inicio. Aceptación: ambas pantallas muestran las mismas elecciones/ajustes y llegan a la misma batalla.
4. **Sincronizar el combate.** Integrar los controladores `_LM` y elegir autoridad de simulación. Mantener posiciones/entradas, animaciones, ataques/efectos, golpes, vida/escudo, medidores, temporizador, victoria/revancha coherentes. Evitar daños duplicados, ataques fantasmas y efectos fuera de orden. Reusar tasas/protocolo nativos solo cuando se confirmen.
5. **Cerrar el ciclo de vida.** Tratar desconexión, cierre de ventana, cambio de escena, volver a LAN/menú y recrear otra sala; ninguna escucha/hilo debe quedar activo al salir.
6. **Validar en red real.** Con dos PCs disponibles, probar primero en el mismo router por Ethernet y después por Wi-Fi; IP inválida, interfaz incorrecta, puerto ocupado, firewall, versiones distintas, desconexión en sala/partida, latencia y pérdida. Validar ambos roles y todos los personajes/escenarios dentro de una matriz incremental.

## Riesgos y límites de la primera versión

- El bloqueo de entrada de Windows, Wi-Fi con aislamiento de clientes, VPN y subredes distintas pueden impedir una conexión aunque las IP parezcan correctas.
- Una build distinta entre equipos puede tener prefabs, enums o reglas incompatibles; se requiere handshake de versión.
- Simular golpes y efectos en dos máquinas es la parte de mayor riesgo, no mostrar/introducir la IP.
- Sin reconexión, una desconexión termina el encuentro. Sin cifrado ni servidor de confianza, la autoridad del anfitrión solo está pensada para partidas privadas en la LAN.
- No incluye Android, matchmaking por Internet, Photon, salas públicas, espectador, cuenta, reconexión ni controles de latencia avanzados.

## Decisiones aún abiertas

- Confirmar si el host simula todas las colisiones/daño o si el original reparte esa autoridad.
- Determinar cuáles eventos viajan confiables y cuáles admiten pérdida, y si se conserva el formato nativo.
- Confirmar el comportamiento al desconectar durante la selección, además de durante la batalla.
- Confirmar controles disponibles en cada PC para la validación final.

## Estimación inicial

Estimación de **6–11 días laborables** para un MVP probado en dos PCs: inventario de contratos (0.5–1 día), conexión y lobby (1–2 días), combate y ciclo de vida (3–5 días), y validación/fixes en red real (1.5–3 días). Es una estimación de planificación, no compromiso; depende sobre todo de cuánto comportamiento de combate pueda recuperarse del código nativo y de los defectos que revele la prueba entre PCs.

## Criterio de listo para la primera entrega

Dos builds de la misma versión, en dos PCs Windows de la misma LAN y sin Internet, completan crear/unirse por IP, seleccionar personajes, aplicar ajustes del host, empezar y terminar una partida con controles, daño, marcador y resultado coherentes. Una desconexión muestra el aviso acordado y deja el juego listo para volver al menú LAN sin reiniciar el proceso.
