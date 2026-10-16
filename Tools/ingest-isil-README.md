# Ingesta de implementación ISIL

```powershell
node "C:\Users\Jose\Desktop\TrueMastersReconstruction\Tools\ingest-isil.mjs" --method-map "C:\Users\Jose\Desktop\TrueMastersReconstruction\Evidence\cpp2il-native-address-map.json"
node --test "C:\Users\Jose\Desktop\TrueMastersReconstruction\Tools\ingest-isil.test.mjs"
node "C:\Users\Jose\Desktop\TrueMastersReconstruction\Tools\rag\rag.mjs" build
node "C:\Users\Jose\Desktop\TrueMastersReconstruction\Tools\rag\rag.mjs" query "GameManager CapFrameRate targetFrameRate"
```

El lector reconoce los encabezados `Type:`, `Method:`, `Disassembly:` e `ISIL:` del dump Cpp2IL. Una selección explícita de tipos de gameplay, controles, menús y corutinas anidadas (ver `scope` en el script) produce fragmentos de hasta 48 líneas ISIL. Los archivos de terceros en subdirectorios y tipos no seleccionados se excluyen; el manifiesto declara cobertura y errores.

En este ELF se demostró que el bloque `Disassembly:` de Cpp2IL usa VA como offset de archivo y muestra instrucciones equivocadas. **No se indexa ese bloque.** Se conserva su rango y advertencia como artefacto inválido; el original sigue en Evidence para auditar el error. El bloque ISIL se recupera por otra ruta correctamente mapeada, pero contiene operaciones `NotImplemented`, llamadas indirectas y ramas que limitan inferencias. Ninguna salida se presenta como C# original o comportamiento probado.

Cuando existen archivos `Evidence/native-disassembly-TIPO.txt` producidos por `Tools/disassemble-type.ps1`, también se indexan sus instrucciones ARM64 corregidas, con fuente, SHA y líneas propios. El lector valida que VA/offset coincidan con el mapa de metadatos antes de aceptarlos. Los fragmentos ARM64 e ISIL mantienen sus propias coordenadas; no se presupone alineación de instrucciones. Las longitudes de métodos son estimaciones de la herramienta y no prueban el límite exacto del método. El hash del binario se conserva del mapa productor.

Las VA se obtienen exclusivamente del mapa productor `cpp2il-native-address-map.json` (`entries` con `declaring_type`, `method_signature`, `native_address`, `raw_file_offset`). Se conservan campos extra, hash del mapa, firma original y digest/rangos del dump. `emitted_RVA` del productor se mantiene como metadato y no se usa como VA. Para comparar firmas se normalizan separadores CLR de anidamiento `+`/`/` y se omiten valores predeterminados mediante un lector de parámetros que respeta genéricos y comillas; las coincidencias ambiguas permanecen sin dirección. Sin mapa, las VA quedan explícitamente desconocidas.

Salidas: `Knowledge/implementation-findings.json`, `Knowledge/implementation-analysis.md` y `Evidence/implementation-ingestion.json`. Las afirmaciones observadas describen IR estático; la interpretación de carga visual, rutas de espectador y configuración se marca como inferencia o hipótesis y conserva límites explícitos.
