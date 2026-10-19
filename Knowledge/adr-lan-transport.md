# ADR: transporte y topología del modo LAN

- Estado: aceptado para el MVP; payloads, asignación de eventos y autoridad quedan sujetos a auditoría.
- Contexto: el objetivo acordado es conectar dos PCs Windows de una LAN por IP manual, sin Internet, servicio de salas ni contraseña. La escena y los enum del juego conservan clases TCP y UDP y métodos de red vacíos.
- Decisión: conservar el diseño original TCP/UDP y completar sus métodos vacíos. Auditar el código nativo y la escena para recuperar el propósito de cada conexión, mensaje, payload y papel de host/invitado antes de implementar. No usar UDP broadcast para descubrir salas en el MVP de IP manual. Mantener LAN separado de Photon.
- Motivo: preservar el diseño de red original del juego, respetando sus contratos existentes. La IP manual permite obviar el descubrimiento por broadcast, pero no implica cambiar los canales que usa el resto del juego.
- Consecuencias: el anfitrión debe mostrar una IP válida y abrir un puerto; Windows Firewall puede bloquear conexiones entrantes; UDP puede perder/reordenar paquetes; ambos caminos requieren timeout, limpieza y pruebas de desconexión. Si el contrato nativo difiere, actualizar esta ADR antes de implementación.
- Alternativas consideradas: sustituir TCP/UDP por un único TCP simplificaría sockets pero cambiaría el contrato original; Photon requeriría un servicio externo y no cumple el modo LAN sin Internet.
- Decisión del usuario: conservar los contratos TCP/UDP originales.
