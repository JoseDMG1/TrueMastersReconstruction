# Recuperación local para la reconstrucción

Esta herramienta construye un índice **BM25 léxico** y ensambla un prompt con evidencia. Funciona sin paquetes npm, claves, descargas ni acceso a Internet con Node.js 20 o posterior. No implementa embeddings, un modelo generativo ni recuperación del código fuente original de Unity. El prompt se entrega a un asistente por separado.

Desde cualquier carpeta, usando la ruta del script:

```powershell
node "C:\Users\Jose\Desktop\TrueMastersReconstruction\Tools\rag\rag.mjs" build
node "C:\Users\Jose\Desktop\TrueMastersReconstruction\Tools\rag\rag.mjs" query "combate fuego controles" --limit 6
node "C:\Users\Jose\Desktop\TrueMastersReconstruction\Tools\rag\rag.mjs" context "How does multiplayer room selection work?" --limit 8
node --test "C:\Users\Jose\Desktop\TrueMastersReconstruction\Tools\rag\rag.test.mjs"
```

El directorio predeterminado es `Knowledge`, relativo al proyecto del script, independientemente de la carpeta actual. `--knowledge DIR` permite seleccionar otro corpus. `build` escribe únicamente `Knowledge/generated/rag-index.json`. Si cambia, aparece o desaparece un documento fuente, `query` y `context` exigen reconstruir el índice. Una consulta no descarga ni ejecuta los archivos citados.

## Incorporar conocimiento

Colocar informes Markdown y hallazgos JSON bajo `Knowledge`. Se recorren subcarpetas, excluyendo `generated`, `schema`, `node_modules`, `.git` y enlaces simbólicos. Los resultados generados deben guardarse dentro de `generated` para evitar volver a indexarlos.

Los hallazgos JSON pueden ser un array, `{ "entries": [...] }` o `{ "findings": [...] }`. Contrato en `Knowledge/schema/findings.schema.json`. Un ejemplo ilustrativo, **no evidencia del juego**:

```json
{
  "entries": [{
    "id": "example-only",
    "statement": "Descripción concreta de la observación o del dato desconocido.",
    "status": "unknown",
    "source_file": "C:\\ruta\\video.mp4",
    "start_seconds": 42,
    "end_seconds": 46,
    "tags": ["menu", "multiplayer"],
    "evidence_id": "identificador REA cuando exista",
    "limitations": ["Límite explícito de la inspección"]
  }]
}
```

Estados: `observed` para observación directa, `inferred` para interpretación, `unknown` para lo no establecido. Se conserva cada campo extra del hallazgo, incluidos enlaces, ID de evidencia, digest, tiempo y limitaciones. El hash añadido al índice identifica el documento de conocimiento; **no verifica el archivo APK o video citado**. Es responsabilidad del productor registrar el digest y la cobertura del artefacto cuando los haya obtenido. Los IDs se califican con la ruta del documento para evitar colisiones entre productores; IDs repetidos dentro de un documento fallan.

Markdown se divide por encabezados y párrafos. Cada fragmento conserva archivo, líneas y hash; su estado es `unknown` porque el buscador no interpreta prosa como hechos confirmados. Otros JSON sin una lista `entries`/`findings` no aportan fragmentos; errores sintácticos de JSON y hallazgos malformados detienen la construcción con un diagnóstico.

## Usar los resultados

`query` devuelve los hallazgos con fuentes completas y puntuaciones BM25. `context` emite un prompt con etiquetas `[E1]`, `[E2]`, etc., exige citar cada hecho, preservar incertidumbre y distinguir propuestas de implementación de comportamiento observado. Un asistente puede usarlo para diseñar scripts, escenas y datos de Unity. No debe afirmar que esos scripts son código recuperado del original.

El tokenizador normaliza acentos y usa un pequeño diccionario español/inglés sobre combate, elementos, menús y red. Puede omitir paráfrasis o detalles con vocabulario diferente. La ausencia de coincidencias no prueba que una función no exista; las puntuaciones no son confianza ni prueba de cobertura. Los resultados pueden incluir observaciones contradictorias: el consumidor debe mantenerlas visibles y buscar evidencia adicional. La versión inicial no evalúa respuestas generativas ni la fidelidad del juego. Una futura búsqueda por embeddings deberá conservar los mismos IDs, fuentes, estados y controles de frescura.
