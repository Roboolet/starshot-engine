---
sidebar_label: 'SceneCamera'
sidebar_position: 3
---
# SceneCamera *: SceneObject*
The SceneCamera is responsible for displaying all the renderers in the scene.
## Properties
| Name | Value type | Description |
| - | - | - |
| `pixelsPerUnit` | float | How many screen-pixels constitute one unit of distance |

## Public methods
| | |
|-|-|
|`AddRenderer`| Manually add a renderer to the camera|
|`WorldToScreenPos`| Convert a world-space coordinate to screen-space|
|`ScreenToWorldPos`| Convert a screen-space coordinate to world-space|

:::warning
The viewport is currently bugged, and does not display camera contents correctly at large distances away from the origin
:::