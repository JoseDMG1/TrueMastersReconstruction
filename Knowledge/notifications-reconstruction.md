# Notificaciones y alertas offline

Implementación parcial desde ARM64, pendiente de validación runtime del checkpoint. Fuente corregida: `Evidence/native-disassembly-Notifications.txt` y `native-disassembly-StarMenuManager.txt`; nombres/argumentos genéricos resueltos con Tools/resolve-native-usage.cjs y Tools/resolve-native-generic.cjs. No se recuperó C# original.

Notifications.Awake 0x129F8D0 registra singleton y destruye el componente duplicado. Se añade limpieza OnDestroy del dueño. Log 0x129FA00 restablece triggers On/Off, cambia texto y color, cancela Invoke anteriores, activa On y programa Remove al tiempo solicitado. Remove 0x129FF14 restablece On y activa Off. Colores de SetColor 0x129FB08: info=(0,0,0,1), warn=(.7,.5,0,1), alert=(.5,0,0,1), success=(0,.5,0,1). Tipos desconocidos mantienen el color. El .7 se leyó del dato ELF en VA0x651C20.

OpenQuestionPopup 0x129FC9C y OpenPopup 0x129FE38 muestran controles Yes/No u OK y llaman al dueño LerpManager.LerpScale: Vector3.zero→one, .5s, EasyOut. ClosePopup 0x129FDB8 hace one→zero en .25s. Yes 0x129FD88 invoca callback y cierra. Los objetos y curvas son los recuperados, no se crean pantallas sustitutas.

StarMenuManager.TourneyAlert 0x12C2C90 requiere Aura/Eliot/Ren/Rina desbloqueados y activa tourneyAlert/points; en caso contrario emite la cadena literal de aviso durante7s. CloseTourneyAlert 0x12C2E44 usa GetComponent<IncreaseSize>().Decrease() en ambos objetos, confirmado por slot0x2814D38. EnterTournament 0x12C2EC4 delega el precio en TournamentController.PayTournament; implementar la alerta no establece que el torneo esté completo.

AllCharactersUnlocked 0x12C2D54 acepta tmPremium2/3 solo con Firebase auth y CurrentUser; el getter se confirmó en el ISIL FirebaseAuth por identidad de método, sin usar su Disassembly con desplazamiento incorrecto. En otro caso requiere ownership1 en los cuatro campos. La rama offline añade guardas para FirebaseController/auth ausentes, usa propiedad local y no simula login ni crea entitlement. TournamentController, anuncio y otros callbacks no restaurados se declaran aparte.

## Integración UI observada

La prueba PlayMode del checkpoint de las21:41 UTC falló con TypeLoadException TMPHelpURL y crash del editor. El plugin exportado Unity.InternalAPIEngineBridge.004.dll colisionaba con la asamblea real que compila UGUI2.6.0. Se retiraron solamente DLL/meta del proyecto de trabajo, con SHA y copia en `Evidence/ui-bridge-migration.json` y WorkingBackups; los exports de evidencia permanecen intactos. No había referencias serializadas a su GUID en Assets (.unity/.prefab/.asset/.asmdef). La herramienta de migración ahora incluye también ese plugin. Los siguientes arranques ya cargaron menú y selección, sin repetir este crash; la partida aún requiere validación.

UnityEditor.Search.SearchDatabase emite por separado un ArgumentOutOfRangeException al indexar en batch. El probe lo conserva como editorInfrastructureErrors con stack completo; no lo clasifica como excepción del código del jugador. Otros errores sí fallan la prueba.
