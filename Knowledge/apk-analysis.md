# Análisis del APK y recuperación editable

## Identidad observada

APK suministrado: `apk compilado.apk`; SHA256 `51e274bd08fd72ff2be562960d21f31871c0318a1e99d9a5c193e4f5d6024c4d`.

REA inspect_artifact: `ev_0aeb777a9a87ae28fea176c176f1a13afbd3a75ec9b03f035b4a84ac7935c6c4`. Inventario completo dentro de entradas ZIP (309 ocurrencias; no cubre bytes externos a las entradas).

Manifest binario: paquete `com.FacuRodquin.BendingBattle`, versión `2.99`, código `299`, minSdk25, targetSdk36, `requiredSplitTypes=base__abi`. El APK no contiene bibliotecas `.so` ni ensamblados `.dll`. El requisito de split nativo está declarado explícitamente: falta el APK de arquitectura correspondiente. No se ejecutó el juego.

UnityFS `assets/bin/Data/data.unity3d`: versión productora `6000.3.10f1`. `global-metadata.dat`: magic `FAB11BAF`, versión39. `ScriptingAssemblies.json` enumera Unity, Photon Chat/Realtime/PUN/Voice, Firebase Auth/Database/Firestore/Messaging y GoogleMobileAds. Esta lista demuestra referencias compiladas, no actividad actual de servicios ni comportamiento concreto.

## Recuperación realizada

Herramienta portátil oficial AssetRipper2.0.0: https://github.com/AssetRipper/AssetRipper/releases/tag/2.0.0 . Archivo descargado SHA256 `9a7ef0e7c5c3ea5b90b4e6d855e2d98d5f7ec8c3f9e26fccbc194c6a7b01baf7`.

Se cargó el APK en el servicio headless local y se exportó a `Evidence/RecoveredUnityProject/ExportedProject`. `Evidence/assetripper.log` conserva el registro. `Evidence/apk-findings.json` conserva los comandos exactos. El analizador solo leyó el APK; escribió únicamente el directorio de investigación y sus temporales de herramienta.

Exportación: 12 escenas, 380 prefabs, 300 clips de animación, 39 controladores Animator, 297 PNG, 174 materiales, 89 OGG, 6 fuentes, 303 archivos `.asset`, 1194 archivos C# y 18 shaders. Tamaño de archivos exportados: 1.281.606.948 bytes. Cada archivo tiene ruta/tamaño/SHA256 en `Evidence/recovered-project-inventory.json`.

Escenas: DesarrolladoPor, StartMenu, ConnectingServer, HowToPlay, Character Selection, Bending Battle, Tournament, Login, OnlineFriends, FriendsConnection, Spectator, LocalMultiplayer. El orden de construcción consta en ProjectSettings/EditorBuildSettings.asset. Los GameObject y componentes de cada escena se indexan en `Evidence/recovered-scenes-inventory.json` y `Knowledge/apk-scenes.md`.

## Limitaciones materiales

Los 1194 archivos C# son clases vacías de sustitución (Dummy), no código recuperado. Los campos serializados de MonoBehaviour tampoco se exportaron: esto afecta tanto a scripts propios como a textos, imágenes y botones UI; nombres y transformaciones de objetos sí están presentes. No se deben interpretar las escenas como una interfaz funcional recuperada. Los shaders son sustituciones Dummy. El importador registró una discrepancia de tamaño en globalgamemanagers tipo129: leyó832 bytes, esperaba836.

No se importó ni compiló el proyecto recuperado en Unity. La instalación disponible6000.6.5f1 difiere de la versión del juego6000.3.10f1. No se ha verificado Android ni multijugador.

## Metadatos para reconstrucción guiada

Se decodificaron las tablas IL2CPPv39 usando índices de ancho variable. Se comprobaron límites de secciones, tamaños/conteos, índices de cadenas y relación entre cada método y su tipo propietario. Assembly-CSharp contiene793 tipos,3635 campos y4143 métodos. Se conservan offset, token e índices originales. Los nombres de parámetros también están disponibles.

`Evidence/assembly-csharp-metadata.json` y `Knowledge/assembly-csharp-symbols.md` contienen estos contratos. Los índices de tipos nativos no pueden resolverse sin la tabla del binario ausente. Los nombres no prueban reglas de combate, fórmulas, tiempos, controles ni flujo de ejecución. No se recuperó ningún cuerpo de método.

El parser reproducible está en `Tools/metadata-v39.cjs`; referencias primarias consultadas: https://github.com/SamboyCoding/Cpp2IL/blob/development/LibCpp2IL/Metadata/Il2CppMetadata.cs y sus estructuras Il2CppTypeDefinition/Il2CppMethodDefinition/Il2CppImageDefinition. Las estructuras consultadas se guardaron en Evidence. El header de Unity6000.6 instalado usa una revisión posterior y no se usó como prueba de compatibilidad del formato39.

## Artefacto que permite avanzar en código

Hace falta el split ABI de la misma versión299 que incluya `libil2cpp.so` y `libunity.so`, por ejemplo `split_config.arm64_v8a.apk`; lo más útil es el conjunto completo `.apks`/`.xapk` o los APK instalados exportados del dispositivo. Eso permitiría analizar código nativo y resolver tipos; tampoco garantiza recuperar el proyecto ni código C# original.

Los videos permiten documentar la experiencia visible. Para reconstruir reglas exactas que no aparezcan allí harían falta observaciones adicionales del juego o evidencia de código. Servicios remotos, cuentas, economía y persistencia externa requieren implementación y decisiones explícitas propias.

## Actualización: conjunto público con split nativo compatible

Se adquirió separadamente el XAPK público2.99; ver Evidence/acquired. El contenedor base tiene un SHA256 distinto al del usuario, pero sus tres miembros decisivos son idénticos byte a byte: data.unity3d `cb3f92a49930d0b8cff6dabd39f36e705cf6e4df3bb786d7bbaa3cd57ab6e478`, global-metadata.dat `c79db4f28d92a0d5959ec74c3e8a2267b8838f7e924640096b5a811237eddfd4`, boot.config `ce2b833293c298335c049fae2e3bc863d9c4f977a334e8605bf1e6f55415c4a2`. Esto incluye el mismo build-guid y versión Unity. Manifest difiere; se mantienen las dos procedencias y el original intacto. Ver Evidence/public-base-comparison.json.

El split público arm64 `config.arm64_v8a.apk`, SHA256 `ff93433b1333759cbe5c0f2f4538551afd550978be517f62c2a1892ffa01b633`, contiene libil2cpp.so y libunity.so. Se extrajeron ambos APK a Evidence/acquired/merged-public-apk; las colisiones conservaron el miembro del APK base y quedaron registradas en merged-provenance.json. No se ejecutaron los binarios.

AssetRipper reconoce ahora el backend IL2CPP, metadata39, codereg0x26B7528, metareg0x279CAF0 y mapea51859 métodos. La segunda exportación completa está en Evidence/RecoveredUnityProject-WithNative; 12 escenas,380prefabs,300animaciones,39AnimatorControllers,297PNG,174materiales,89OGG,563C# y38DLL de dependencias. Hashes en native-recovered-project-inventory.json. Las declaraciones de scripts y sus campos tipados se recuperaron; también los campos serializados UI, textos y eventos de botones. Esto supera la limitación de campos ausentes de la primera exportación.

Los cuerpos de métodos continúan vacíos. Se solicitó ScriptContentLevel=Level3; la implementación libre2.0.0 de IL2CppManager utiliza salida dummy cuando no se inyectan RecoveryProcessingLayers/RecoveryOutputFormat, que es este caso. Por eso no se describe esta exportación como código original funcional. Fuente: https://github.com/AssetRipper/AssetRipper/blob/2.0.0/Source/AssetRipper.Import/Structure/Assembly/Managers/IL2CppManager.cs . El warning globalgamemanagers tipo129 continúa. Algunos GoogleMobileAdsSettings de editor tampoco tienen scriptID válido. Los shaders continúan Dummy.

Se inició una evaluación separada de Cpp2IL oficial2022.1.0-pre-release.21 para obtener ISIL/disassembly nativo y comprobar recuperación IL. Los resultados y limitaciones figuran en Knowledge/native-code-recovery.md cuando termine; esto no cambia la condición vacía de los métodos exportados por AssetRipper.
