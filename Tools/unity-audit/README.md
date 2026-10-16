# Auditoría de escenas exportadas

```powershell
cd "C:\Users\Jose\Desktop\TrueMastersReconstruction\Tools\unity-audit"
npm.cmd ci --ignore-scripts
node audit.mjs
node --test audit.test.mjs
```

`node audit.mjs PATH` selecciona otra raíz del proyecto exportado. Las salidas siempre se escriben en `Evidence/unity-menu-audit.json`, `Knowledge/unity-menu-analysis.md` y `Knowledge/unity-menu-findings.json` del proyecto de reconstrucción; el exportado es de solo lectura para esta herramienta. No se abre Unity ni se ejecutan scripts del juego.

`node audit.mjs PATH --menus-only` limita el examen a todas las escenas y prefabs seleccionados por identificadores de ruta UI/chat/friend/player UI/button/toggle/slider/dropdown/inputfield/menu. Se declara ese alcance en el informe, incluidos prefabs descubiertos frente a examinados. No prueba cobertura de UI oculta en prefabs excluidos. El perfil predeterminado examina todos los prefabs.

Se indexan todos los `.meta` con el parser `yaml` 2.9.0 y se analizan todos los `.unity`/`.prefab`. El encabezado de documento Unity proporciona IDs de clase/objeto; el cuerpo se procesa como YAML mediante esquema failsafe para conservar GUIDs/IDs exactos. Se conservan errores, archivos fuente y SHA-256, jerarquías, referencias de componentes/scripts, textos UI y llamadas persistentes. Los mapas completos se retienen para escenas y candidatos de UI; los otros prefabs contribuyen a los contadores y manifiesto. No se expanden overrides ni herencia de instancias prefab.

Los nombres de GameObject y scripts son identificadores estáticos. No prueban etiquetas visibles, interactividad ni comportamiento en ejecución. Un GUID asociado a un archivo de script no prueba que su implementación original haya sido recuperada. La falta de campos personalizados/UI/UnityEvent puede resultar de una exportación IL2CPP incompleta. Las métricas de memoria y CPU cubren este proceso Node; no son una comparación de rendimiento entre versiones.

## Restore official UI implementation

`node --openssl-legacy-provider restore-ui-package.mjs` creates the static migration plan against the installed official Unity 6000.6.5f1 UGUI 2.6.0 package. It reads placeholder type/schema evidence, matches namespace/type identities, derives legacy DLL fileIDs with Unity's MD4 convention, and reads destination GUIDs from package `.cs.meta` files.

`node --openssl-legacy-provider restore-ui-package.mjs --apply` applies the plan only while the original two placeholder UI plugins exist. Run with Unity closed. All working files and exact plugin bytes are backed up under `Evidence/WorkingBackups/ui-package-<timestamp>`. Handled mutation errors restore touched originals; interruption or machine failure requires restoring those backups. Successful apply is recorded in `Evidence/ui-package-migration-applied.json`. A second apply stops because the original plugins have been removed; it does not rewrite migrated assets.

The YAML parser retains integers/identifiers as strings. Parsed map source spans identify component `m_Script` and nested animation `script` PPtrs. Only those PPtr spans change. Every document is reparsed and checked for semantic equality against the original plus the explicitly mapped reference; all other text remains untouched. Missing/ambiguous mappings, unknown component serialized fields, unexpected old DLL reference contexts, malformed YAML, or changed source digests stop the migration before mutation. `.unity`, `.prefab`, `.asset`, `.anim`, `.controller`, and `.overrideController` are covered. Immutable native exports and custom game scripts are untouched.

`node --openssl-legacy-provider --test restore-ui-package.test.mjs` checks component rewrites, nested animation bindings, unrelated PPtrs, long integer identifiers, explicit fail-closed behavior, and recovered Unity hash identities. Passing these checks establishes static rewrite integrity. Unity compilation, import, and gameplay still require separate verification; unrelated provider/SDK plugins remain placeholders.
