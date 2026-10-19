# Glosario del modo LAN

- **Anfitrión (host):** PC que abre la sala, escucha conexiones y fija los ajustes del encuentro.
- **Invitado (guest):** PC que se conecta a la dirección LAN del anfitrión.
- **IP LAN:** dirección IPv4 privada de la interfaz Ethernet/Wi-Fi compartida por ambos PCs. No es una dirección pública de Internet.
- **Sala:** sesión privada temporal con un anfitrión y un invitado. En el MVP no tiene nombre, contraseña ni lista de búsqueda.
- **Lobby:** estado previo a la batalla donde cada jugador elige luchador y el anfitrión fija mapa, velocidad y tiempo.
- **Handshake:** intercambio inicial para confirmar protocolo, versión de build, rol y configuración básica antes de aceptar la sala.
- **Mensaje confiable:** orden discreta cuya entrega y secuencia importan, por ejemplo personaje confirmado, empezar, ataque creado o resultado.
- **Snapshot de entrada/estado:** paquete frecuente con entrada o estado de movimiento reciente; puede descartarse si llega tarde, según el contrato elegido.
- **Autoridad del anfitrión:** PC que arbitra el inicio y reglas críticas del combate para evitar que ambos equipos resuelvan el mismo resultado por separado.
- **Desconexión:** pérdida o cierre del enlace TCP/UDP. En la primera versión termina el encuentro con aviso y retorno al menú LAN.
