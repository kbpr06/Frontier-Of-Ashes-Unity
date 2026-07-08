# 🎮 Frontier of Ashes

## Descripción

**Frontier of Ashes** es un prototipo de videojuego 2D desarrollado en **Unity 6** como parte de la asignatura **Diseño y Desarrollo de Videojuegos**.

El proyecto consiste en un pequeño escenario medieval donde el jugador puede explorar libremente el mapa, interactuar con objetos recolectables, visualizar un contador de objetos mediante una interfaz gráfica y disfrutar de música ambiental junto con efectos de sonido.

---

# ✨ Características implementadas

- 🌍 Escenario construido mediante Tilemaps.
- 🚶 Personaje controlable utilizando Unity Input System.
- 🎥 Cámara que sigue al jugador.
- 🎬 Animaciones Idle y Walk.
- 🚧 Colisiones con escenario y edificaciones.
- 🗡️ Objetos recolectables.
- 🖥️ Interfaz gráfica (HUD).
- 📊 Contador de objetos.
- 🎵 Música ambiental.
- 🔊 Efecto de sonido al recoger objetos.

---

# 🧠 Arquitectura del proyecto

El proyecto se encuentra organizado utilizando distintos scripts especializados para separar responsabilidades.

## Managers

- GameManager
- AudioManager

## Player

- PlayerMovement

## Camera

- CameraFollow

## Interactables

- CollectableItem

## UI

- HUDController

---

# 📁 Estructura del proyecto

```
Assets
│
├── Animation
├── Audio
│   ├── Music
│   └── SFX
├── Input
├── Prefabs
├── Scenes
├── Scripts
│   ├── Camera
│   ├── Interactables
│   ├── Managers
│   ├── Player
│   └── UI
├── Sprites
└── ThirdParty
```

---

# 🎮 Controles

| Acción | Tecla |
|---------|-------|
| Mover personaje | Flechas del teclado |

---

# 🛠️ Herramientas utilizadas

- Unity 6
- C#
- Unity Input System
- Tilemap
- TextMeshPro
- Git
- GitHub Desktop

---

# 📂 Escena principal

La escena principal del proyecto se encuentra en:

```
Assets/Scenes
```

---

# 🚀 Estado del proyecto

✅ Prototipo completamente funcional.

Incluye:

- Movimiento.
- Escenario.
- Colisiones.
- Interfaz gráfica.
- Elementos visuales.
- Elementos auditivos.
- Recolección de objetos.

---

# 🔮 Mejoras futuras

- 🎒 Sistema de inventario.
- ⚔️ Sistema de combate.
- 👤 NPC con diálogos.
- 🎯 Sistema de misiones.
- 💾 Guardado de partida.
- 🌫️ Transparencia automática de techos.
- 👾 Enemigos con inteligencia artificial.

---

# 👨‍💻 Autores
- Jaime Codoceo
- Sergio Molina
- Karla Pesce

Proyecto desarrollado para la asignatura **Taller de Desarrollo de Videojuegos**.

**Frontier of Ashes**
