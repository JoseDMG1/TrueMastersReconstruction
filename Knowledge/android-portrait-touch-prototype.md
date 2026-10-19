# Prototipo de combate táctil vertical para Android

## Objetivo

Probar una interfaz de combate para teléfono en orientación vertical, aislada de la versión principal de True Masters Reconstruction. No cambia controles de PC ni se considera una decisión definitiva para el producto.

## Decisiones acordadas

- El prototipo vive en la rama `prototype/android-portrait-touch-combat` y en un checkout separado.
- Solo se activa en builds Android; Windows y los controles de PC permanecen como están.
- El build Android se fija en orientación vertical.
- Las flechas de la parte inferior lanzan el ataque asociado a la dirección elegida y no mueven al luchador.
- Arrastrar un dedo desde cualquier zona libre de UI controla movimiento.
- Un deslizamiento rápido desde una zona libre solicita un esquive en esa dirección.
- A lanza un ataque sin movimiento; B mantiene el escudo hasta soltarlo; S activa el especial.
- Se acepta una acción táctil a la vez. No se permite mover y atacar/defender con dedos distintos.
- La interfaz debe redistribuirse y conservar áreas táctiles cómodas en formato vertical, respetando los márgenes seguros del teléfono.

## Parámetros para ajustar durante la prueba

La distancia mínima y el tiempo máximo que distinguen un deslizamiento rápido de un arrastre normal deben quedar como valores configurables. La primera compilación usará umbrales iniciales conservadores que se puedan ajustar después de probarla en teléfono.

## Revisión para la primera compilación

- Verificar que el build Android se inicia siempre en vertical.
- Confirmar que Android muestra la interfaz nueva y que Windows conserva la anterior.
- Comprobar que movimiento, esquive, ataques direccionales, A, escudo B y especial S producen acciones separadas.
- Comprobar con un teléfono real la clasificación de gestos y el tamaño de los objetivos táctiles.
