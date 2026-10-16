# Recuperación de código nativo: alcance y límites

## Resultado verificado

La exportación nativa con AssetRipper restauró tipos, campos tipados y datos de escenas/UI. Los métodos C# siguen vacíos: el juego aún necesita reconstrucción de su lógica para ser funcional. La exportación automática de IL con Cpp2IL tampoco produjo implementaciones ejecutables.

Se ejecutó el Cpp2IL oficial `2022.1.0-pre-release.21+58fc404ac503f4e512055cafc48c03088fc6e224` portátil, obtenido de https://github.com/SamboyCoding/Cpp2IL/releases/tag/2022.1.0-pre-release.21 . SHA256 del ejecutable `663fb432433b4371fd1ee0ebc321a8fff2a9aac5ac4230c843f9e03ddee4e04c`. No se instaló software global ni se ejecutó el juego.

Entrada binaria: `Evidence/acquired/merged-public-apk/lib/arm64-v8a/libil2cpp.so`, SHA256 `d5669fa920c5034ec1ed5b7988149825430f65ea557ba7882567c0790da2dd8c`. Metadata del APK original: SHA256 `c79db4f28d92a0d5959ec74c3e8a2267b8838f7e924640096b5a811237eddfd4`, idéntico al base público. Versión explícita6000.3.10f1. El split y su adquisición conservan procedencia separada; no se sustituyó el APK original.

## Comandos ejecutados

Desde Tools, con rutas absolutas correspondientes:

```
Cpp2IL-2022.1.0-pre-release.21-Windows.exe --force-binary-path <libil2cpp.so> --force-metadata-path <global-metadata.dat original> --force-unity-version 6000.3.10f1 --use-processor attributeinjector --output-as isil --output-to <Evidence/Cpp2IL-ISIL>
Cpp2IL-2022.1.0-pre-release.21-Windows.exe --force-binary-path <libil2cpp.so> --force-metadata-path <global-metadata.dat original> --force-unity-version 6000.3.10f1 --use-processor attributeinjector --output-as dll_il_recovery --output-to <Evidence/Cpp2IL-ILRecovery>
```

Ambos salieron0. ISIL completó en43,44s según registro del propio proceso (no es benchmark comparativo); produjo6247 archivos/146.115.272 bytes,722 archivos de tipos de Assembly-CSharp. ILRecovery completó en10,72s. No se midieron CPU ni memoria pico. Logs: Evidence/cpp2il-isil.log y cpp2il-ilrecovery.log.

La auditoría de cada método Assembly-CSharp de ILRecovery mediante Mono.Cecil está en Evidence/cpp2il-ilrecovery-audit.json:4126 métodos contienen exactamente `ldnull; throw`;20 carecen de cuerpo;0 cuerpos recuperados útiles. Estos DLL no son una implementación del juego. El recuento4146 incluye miembros generados por Cpp2IL, mientras4143 es el número de métodos del Assembly-CSharp original en metadata.

## Bug observado en el volcado Disassembly

La sección `Disassembly:` de los archivos ISIL de este ELF usa la dirección virtual como offset de archivo y por eso presenta instrucciones incorrectas. La sección `ISIL:` usa bytes correctamente mapeados en los casos examinados; su precisión completa no se ha verificado. No usar la sección Disassembly como evidencia de comportamiento.

Prueba mínima: GameManager.Start tiene dirección virtual0x12946AC; el segmento ELF PT_LOAD ejecutable tiene VA-base0xFAB5C0 y fileOffset-base0xFA75C0 (diferencia0x4000). Por tanto el offset real es0x12906AC. Bytes reales `fe4fbfa900902e1ef30300aae0031faa` corresponden al prólogo y carga FMOV1.25 vistos en ISIL. En cambio los bytes del offset incorrecto0x12946AC son `e8ab00d0e10314aae3031faa08e147f9...`, que corresponden al ADRP/MOV de la sección Disassembly errónea. Este desajuste se repite en ActivateOnClick y StarMenuManager; no se afirma haber revisado cada método.

`Evidence/cpp2il-native-address-map.json` conserva por separado dirección virtual real y offset de archivo, los segmentos PT_LOAD, token, longitud estimada por herramienta y bytes iniciales. Tiene4146 métodos y campos tipados/offsets de Assembly-CSharp. El AddressAttribute.RVA emitido por Cpp2IL coincide con el offset de archivo en este ELF, no con la VA; se conserva como `emitted_RVA` sin reinterpretarlo.

## Reconstrucción posible

Usar los métodos ISIL y sus campos recuperados para reconstruir de forma explícita el código C#; verificar cualquier fórmula, control de flujo o constante contra bytes mapeados y, cuando exista, video. Los nombres de métodos/campos no bastan como prueba. Se pueden recuperar primero navegación, configuración y controles; combate, IA y red requieren implementar y verificar muchas más funciones. La existencia de clases Photon/Firebase no reconstruye servidores ni credenciales de servicios.

No se ha verificado equivalencia funcional del juego, compilación Android, partidas, efectos de cada habilidad, servidor ni persistencia. Estos límites deben permanecer visibles aunque el proyecto de Unity compile.

### Desensamblado independiente disponible

Se obtuvo Capstone5.0.6 del wheel oficial win_amd64 publicado en PyPI (https://pypi.org/project/capstone/5.0.6/), se extrajo localmente su DLL y se invocó cs_disasm vía interop estático; no se instaló Python ni se ejecutó el juego. Hashes en Evidence/capstone-provenance.json. Tools/disassemble-type.ps1 -TypeName GameManager decodifica bytes con el offset correcto y presenta direcciones virtuales reales; el resultado queda en Evidence/native-disassembly-GameManager.txt. Esta vía permite resolver los huecos de ISIL en funciones seleccionadas. GameManager ya fue reconstruido con esta evidencia; ver Knowledge/game-manager-reconstruction.md.
