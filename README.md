# Frontier of Ashes

## Descripción del proyecto

**Frontier of Ashes** es un prototipo de videojuego 2D desarrollado en Unity como parte de la asignatura **Diseño y Desarrollo de Videojuegos**.

El proyecto presenta un escenario de libre exploración compuesto por una zona inicial de pueblo y una zona de bosque con mayor nivel de peligro. El jugador puede desplazarse por el escenario, interactuar con el entorno, recolectar recursos, combatir criaturas y enemigos, administrar objetos mediante un inventario y utilizar puntos de control para reaparecer después de perder toda su vida.

El desarrollo se realizó aplicando principios de programación orientada a objetos, separación de responsabilidades y componentes reutilizables, permitiendo ampliar el proyecto mediante nuevos animales, enemigos y recursos sin necesidad de desarrollar nuevamente toda su lógica.

---

## Video demostrativo

El video presenta el prototipo en funcionamiento y explica los principales sistemas desarrollados, incluyendo la arquitectura del proyecto, programación orientada a objetos, componentes reutilizables, movimiento, escenario, diseño de niveles, colisiones, cámara inteligente, interfaz gráfica, sistema de salud, Game Over, reaparición, checkpoints, combate, enemigos, loot e inventario.

**Enlace al video:**

https://drive.google.com/drive/search

---

## Características principales

- Movimiento del jugador en un escenario 2D.
- Animaciones de Idle, Walk, Attack y Dead.
- Cámara inteligente que sigue al jugador.
- Escenario construido mediante Tilemaps.
- Sistema de colisiones para edificios, obstáculos y elementos del mapa.
- Diseño de nivel de libre exploración entre la zona del pueblo y el bosque.
- Animales domésticos con movimiento autónomo.
- Enemigos con detección, persecución y ataque.
- Sistema de combate para el jugador.
- Sistema de salud representado mediante corazones.
- Sistema de daño y muerte.
- Pantalla de Game Over.
- Sistema de reaparición del jugador.
- Puntos de control o checkpoints.
- Sistema de loot configurable.
- Objetos recolectables distribuidos en el escenario.
- Inventario dinámico con iconos y cantidades.
- Notificaciones temporales al recoger recursos.
- Música ambiental y efectos de sonido.
- Interfaz gráfica integrada al gameplay.

---

## Sistemas implementados

### Player

El jugador fue desarrollado mediante componentes separados según su responsabilidad:

- `PlayerMovement`: controla el desplazamiento del personaje y almacena la última dirección de movimiento.
- `PlayerCombat`: administra el sistema de ataque y utiliza un `AttackPoint` dinámico según la dirección del jugador.
- `PlayerHealth`: controla la vida, el daño, la muerte, el Game Over y la reaparición.

---

### Criaturas

Los animales domésticos utilizan componentes reutilizables para compartir una misma lógica entre diferentes tipos de criaturas.

Entre los sistemas implementados se encuentran:

- Movimiento autónomo.
- Sistema de vida.
- Recepción de daño.
- Muerte.
- Generación de loot.

Las criaturas fueron configuradas como Prefabs, permitiendo reutilizar sus componentes y modificar sus parámetros desde el Inspector.

---

### Enemigos

Los enemigos incorporan comportamientos adicionales que les permiten:

- Desplazarse de manera autónoma.
- Detectar al jugador.
- Perseguir al jugador.
- Atacar dentro de un rango determinado.
- Recibir daño.
- Reproducir animaciones según su estado.
- Morir y generar recursos.

Las animaciones se controlan mediante el Animator de Unity utilizando diferentes estados y parámetros.

---

### Sistema de combate

El sistema de combate utiliza un `AttackPoint` dinámico que cambia de posición según la última dirección de movimiento del jugador.

La detección de objetivos se realiza dentro de un radio determinado y mediante una `LayerMask`, permitiendo identificar únicamente las criaturas que pueden recibir daño.

---

### Sistema de salud y reaparición

El jugador posee un sistema de salud conectado con la interfaz gráfica.

Cuando recibe daño:

1. Disminuye su vida.
2. Se actualizan los corazones del HUD.
3. Al llegar a cero se bloquea temporalmente el movimiento.
4. Se ejecuta la animación de muerte.
5. Se muestra la pantalla de Game Over.
6. El jugador reaparece en el último checkpoint activado.
7. Su vida se restaura completamente.

---

### Checkpoints

Los puntos de control utilizan Triggers para actualizar el punto de reaparición del jugador.

Cuando el jugador pierde toda su vida, reaparece en el último checkpoint activado.

---

### Sistema de loot

Las criaturas y enemigos pueden generar recursos al morir.

El sistema permite configurar:

- Objetos obligatorios.
- Objetos adicionales.
- Cantidades.
- Diferentes tipos de recursos.

Los objetos generados pueden ser recogidos por el jugador y almacenados en el inventario.

---

### Inventario

El sistema de inventario permite almacenar y visualizar los recursos obtenidos durante la exploración.

Incluye:

- Gestión centralizada mediante `InventoryManager`.
- Uso de `ScriptableObjects` para definir los datos de los objetos.
- Almacenamiento de cantidades.
- Generación dinámica de slots.
- Iconos individuales para cada recurso.
- Cantidades acumuladas.
- Apertura y cierre mediante la tecla `I`.
- Notificaciones temporales al recoger objetos.

---

## Programación y arquitectura

Durante el desarrollo se aplicaron diferentes conceptos y herramientas de Unity y programación:

- Programación orientada a objetos.
- Separación de responsabilidades.
- Componentes reutilizables.
- Prefabs.
- Managers.
- Singleton.
- ScriptableObjects.
- Dictionary.
- Coroutines.
- Input System.
- Rigidbody2D.
- Collider2D.
- Triggers.
- LayerMask.
- Animator.
- Blend Trees.
- Tilemaps.
- Interfaz gráfica dinámica.

La arquitectura modular permite reutilizar los componentes existentes para incorporar nuevas criaturas, enemigos y recursos.

---

## Controles

| Acción | Control |
|---|---|
| Movimiento | Flechas direccionales |
| Atacar | Barra espaciadora |
| Abrir/Cerrar inventario | I |

---

## Elementos de la interfaz

El videojuego incluye:

- HUD de salud mediante corazones.
- Inventario visual.
- Iconos y cantidades de recursos.
- Notificaciones de objetos recolectados.
- Pantalla de Game Over.
- Transiciones de muerte y reaparición.

---

## Elementos visuales y auditivos

El proyecto utiliza recursos gráficos de estilo pixel art para mantener coherencia entre:

- Escenario.
- Personaje.
- Animales.
- Enemigos.
- Objetos.
- Interfaz gráfica.

También se incorporaron:

- Música ambiental.
- Efectos de sonido para la recolección de objetos.

---

## Tecnologías utilizadas

- Unity
- C#
- Unity Input System
- Unity Tilemap
- Unity Animator
- TextMeshPro
- Git
- GitHub

---

## Autores

Proyecto desarrollado para la asignatura **Diseño y Desarrollo de Videojuegos**.

**Proyecto:** Frontier of Ashes  
**Motor:** Unity  
**Lenguaje de programación:** C#

# 👨‍💻 Autores
- Jaime Codoceo
- Sergio Molina
- Karla Pesce

Proyecto desarrollado para la asignatura **Taller de Desarrollo de Videojuegos**.

**Frontier of Ashes**
