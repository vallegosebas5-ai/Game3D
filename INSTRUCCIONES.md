# Cómo poner en marcha el Escape Room 3D

## 1. Crear el proyecto en Unity (una sola vez, integrante A)

1. En Unity Hub crea un proyecto **3D (Built-in)** llamado `EscapeRoom3D`, con la versión de Unity que indicó el docente.
   (También funciona con URP; los materiales se adaptan solos.)
2. Cierra Unity.
3. Copia dentro de la carpeta del proyecto el contenido de este zip:
   - la carpeta `Assets/` (combínala con la existente),
   - `.gitignore`, `README.md` e `INSTRUCCIONES.md`.
4. Abre el proyecto en Unity. Al terminar de compilar, se construyen solos:
   - `Assets/Scenes/MainScene.unity`
   - `Assets/Prefabs/Player.prefab`
   - los materiales de `Assets/Materials/`
5. Abre `Assets/Scenes/MainScene.unity` y presiona **Play**.

Si la escena no apareció, usa el menú **Tools > Escape Room > Construir escena**.

### Si el jugador no se mueve
Ve a **Edit > Project Settings > Player > Other Settings > Active Input Handling** y elige **Both** (o *Input Manager (Old)*). Unity reiniciará.

### Configuración que pide la guía
**Edit > Project Settings > Editor**: *Version Control Mode = Visible Meta Files* y *Asset Serialization = Force Text*.

## 2. Cómo se juega

1. Lee la nota de la pared izquierda (pista).
2. Ve al panel azul de la pared derecha y escribe el código (**1234**).
3. Abre el cofre: aparece la llave.
4. Toma la llave y abre la puerta del fondo. ¡Escape completado!

## 3. Qué archivo se usa en cada caso de la guía

| Caso | Archivo(s) |
|---|---|
| 01 | Todo el proyecto, `.gitignore`, `README.md` (commit `chore: initial Unity project`) |
| 03 | `Assets/Scripts/Player/PlayerController.cs` |
| 04 | `Assets/Scripts/Interaction/PlayerInteraction.cs` |
| 05 | `README.md` (cada uno edita una sección distinta) |
| 06 | `PlayerController.cs` → método `Update()` |
| 07 | `Assets/Prefabs/Player.prefab` |
| 08 | `Assets/Scenes/MainScene.unity` (objetos `Chest` y `Door`) |
| 09 | `Assets/Scripts/Environment/OldDoorController.cs` (no se usa en la escena, existe para eliminarlo/modificarlo) |
| 10 | `Assets/Scripts/Puzzle/PuzzleManager.cs` |
| 16 | `Assets/Scripts/Systems/GameManager.cs` → `Initialize()`; si se llama dos veces aparece un error en Console |
| 17 | `Assets/Scripts/Environment/DoorController.cs` (romper la puerta y luego Revert) |
| 19 | `PuzzleManager.cs` (commit `fix: correct puzzle validation`) |

> Recomendación: para que los casos 03, 04, 06 y 10 tengan sentido, puedes hacer el primer commit sin esos
> scripts y que cada integrante los agregue desde su rama tal como dice la guía. El código final que
> deben conservar es el de este proyecto.

## 4. Estructura

```
Assets/
├── Editor/EscapeRoomSceneBuilder.cs   (construye escena y prefab)
├── Scripts/
│   ├── Player/        PlayerController, PlayerLook
│   ├── Interaction/   IInteractable, PlayerInteraction
│   ├── Puzzle/        PuzzleManager, PuzzleTerminal, ClueNote
│   ├── Environment/   ChestController, KeyItem, DoorController, OldDoorController
│   └── Systems/       GameManager
└── UI/GameHUD.cs
```
