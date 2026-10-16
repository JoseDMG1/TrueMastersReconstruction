# SpecialAtk: carga y descarga de especiales

Archivo de trabajo: UnityProject/Assets/Scripts/Assembly-CSharp/SpecialAtk.cs. Se restauraron siete métodos activos (Start,OnDestroy,ChargeSpecial,ResetCharge,dos iteradores yFinishEnemyAwakening); inicializadores del constructor recuperados. El FixedUpdate original solo contiene RET: se eliminó ese callback vacío. Los dos tipos privados de máquina de estados vacíos y sus IteratorStateMachineAttributes se sustituyeron por iteradores reales C# con yield; no se conservaron wrappers obsoletos.

## Evidencia

- Evidence/native-disassembly-SpecialAtk.txt; Evidence/native-disassembly-SpecialAtk__DechargeSpecial_d__20.txt; Evidence/native-disassembly-SpecialAtk__DechargeSpecialLM_d__19.txt. Desensamblado Capstone con direcciones virtuales/offsets ELF mapeados.
- Volcados ISIL originales de los tres tipos en Evidence/Cpp2IL-ISIL/IsilDump/Assembly-CSharp. Solo se usó la sección ISIL; Disassembly original tiene el bug de offsets documentado.
- Evidence/special-atk-native-constants.json: literales NumSpecials,X,S,Character,Maya, tipos SystemManager/GameController y slots de métodos genéricos conservados como desconocidos cuando el pequeño resolver no resuelve la referencia. Los tipos concretos GetComponent<AudioSource>,GetComponent<AuraController>,GetComponent<AnimatorStatesManager> se justifican por las llamadas AudioSource.Play/AuraController.StopAura y el campo awakening de AnimatorStatesManager (offset0x41).
- SHA256 binario d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c, metadata c79db4f28d92a0d5959ec74c3e8a2267b8838f7e924640096b5a811237eddfd4.

## Cobertura

| Método | VA | Comportamiento reconstruido |
|---|---|---|
| Start | 0x12B0F04 | specialAtk=this. |
| OnDestroy | 0x12B0F50 | specialAtk=null, sin condición. |
| ChargeSpecial | 0x12B0F9C | Si decharging, retorna. Comprueba fill antes del incremento: si menor0.9 y aún no charged suma val/numberOfSpecial; si ya listo, charged=true, letras/círculo Color.black,fill1 y AudioSource.Play una vez. |
| ResetCharge | 0x12B10D0 | fill0; colores sBlack serializados; NumSpecials0 en animMainChar; letraX; charged/soundPlayedfalse. No altera decharging. |
| DechargeSpecialLM | fábrica0x12B11D8;MoveNext0x12B161C | fill1; mientras positivo suma -0.034/time y espera0.04segundos. Sin cambios de flags/aura al terminar. |
| DechargeSpecial | fábrica0x12B1270;MoveNext0x12B13A4 | dechargingtrue y fill1; mismo bucle; StopAura en animMainChar; flags charged,soundPlayed,dechargingfalse. Al finalizar restaura letraS y NumSpecials1 salvo Survival+CharacterMaya. |
| FinishEnemyAwakening | 0x12B1308 | OpponentCharacter.GetComponent<AnimatorStatesManager>().awakening=false. Conserva PunRPC sin añadir envío remoto. |
| Constructor | 0x12B1384 | speed0.1,numberOfSpecial1; preservados como inicializadores de campos. |

Constantes float32 verificadas en bytes del ELF: VA0x651A48=0.8999999761581421,VA0x651A7C=-0.03400000184774399,VA0x651C54=0.03999999910593033. El umbral de carga conserva la rama nativa de menor-que negada, incluida su comparación conNaN. Las coroutines esperan después de cada decremento, incluido el último que lleva fill a0; no se reemplazaron por interpolación de duración ni deltaTime. La duración real depende del incremento/espera y de Time.timeScale, tal como el código recuperado.

Los nombres de campos serializados se preservaron. sYellow,newCharge,lastCharge,charge yspeed no se eliminaron aunque estos métodos no los usan: son datos del artefacto. El parámetro time no se limita ni valida porque el original tampoco lo hace; no se inventaron respuestas para0/negativos. Tampoco se añadieron finalmente/cleanup por cancelación: Dispose nativo era RET, por lo que detener una coroutine podía dejar dechargingtrue.

## Dependencias y validación

Se requiere GameController.i con animMainChar yOpponentCharacter, SystemManager.i y referencias UI/AudioSource asignadas. AuraController.StopAura era vacío al iniciar este subtrabajo; su recuperación se coordina aparte. No se cambió Special_LM ni el modo local de dos jugadores. No se ejecutó el juego, Play Mode ni backend; la compilación final corresponde al agente principal. Este código es reconstrucción guiada por instrucciones nativas, no fuente original ni equivalencia de combate completa verificada.

## Dependencia directa restaurada: AuraController

Se reconstruyó UnityProject/Assets/Scripts/Assembly-CSharp/AuraController.cs (cinco métodos activos). Se retiró Start vacío, nativoVA0x1215FB8=RET. Fuentes: Evidence/native-disassembly-AuraController.txt, ISIL del tipo y Evidence/aura-controller-native-constants.json.

ShowChargeAwake0x1215FBC instancia chargeFX en transform.position/rotation y copia únicamente la capa del objeto raíz. StartAura0x12160F0 llama StartAuraInit0x1216234: AnimatorStatesManager.awakening=true y Play() en cada ParticleSystem. StopAura0x12162D8 llama StopAuraInit0x121641C:awakening=false y Stop() en cada sistema. No se añadieron Clear(), destrucción ni cambios de hijos.

Las dos funciones públicas preservan el envío Photon original únicamente bajo GameController.i.online&&PhotonView.IsMine, con RpcTarget.Others(valor1) y literales exactos StartAuraInit/StopAuraInit. El flujo offline no entra en esa rama. Los atributos PunRPC se conservan. No se ejecutaron RPCs ni servicios remotos durante la reconstrucción.

Esto resuelve la dependencia StopAura de la coroutine SpecialAtk. Todavía depende del controlador y AnimatorStatesManager correctamente asignados, de referencias reales de prefabs y de que sus animaciones/componentes estén activos. No se afirma equivalencia visual verificada sin validación dinámica; el agente principal coordina esa validación.
