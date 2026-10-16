# TrueMastersReconstruction

Proyecto Unity de reconstrucción de un juego de combate. El desarrollo se centra en partidas locales para Windows; sigue en progreso.

## Abrir en Unity Hub

1. Instala **Unity 6000.6.5f1** y el módulo de compilación para Windows.
2. Clona este repositorio.
3. En Unity Hub, selecciona **Add / Añadir proyecto desde disco** y elige la carpeta **UnityProject**, que contiene `Assets`, `Packages` y `ProjectSettings`.
4. Espera la importación inicial y abre `Assets/Scenes/DesarrolladoPor.unity` para iniciar desde la escena de arranque.

## Controles de escritorio

| Acción | Predeterminados | Alternativos |
| --- | --- | --- |
| Movimiento | Flechas | WASD |
| Ataque normal | Z | Clic izquierdo |
| Esquivar / escudo / salto según el personaje | X | Espacio |
| Activar súper | C | Clic derecho |

La opción de controles alternativos está en los ajustes. Los cambios más recientes incluyen la corrección del icono del ojo, los fondos del HUD y el engranaje de ajustes.

## Contenido

- `UnityProject/`: proyecto editable, recursos y configuración de Unity.
- `Tools/`: herramientas de reconstrucción, compilación y diagnóstico.
- `Knowledge/`: documentación técnica y alcance de las verificaciones.
- `HANDOFF.md`: historial de trabajo y tareas pendientes; algunas secciones describen versiones anteriores.

No se publican cachés de Unity, ejecutables compilados, copias de respaldo, paquetes originales, vídeos de referencia ni el archivo local completo de evidencias. Las rutas a `Evidence/` en la documentación se refieren a archivos de trabajo locales que no se distribuyen aquí. Los identificadores de servicios externos se eliminan de esta copia pública.

## Estado y alcance

La compilación Windows más reciente completó sin errores y se comprobó visualmente la corrección del HUD. La nueva asignación Espacio / clic derecho todavía no tiene una comprobación separada con entradas físicas. El multijugador, los servicios en línea y otras partes de la reconstrucción permanecen incompletos.

Este repositorio no concede una licencia adicional sobre recursos, marcas ni dependencias de terceros. Sus derechos y licencias siguen perteneciendo a sus respectivos titulares.
