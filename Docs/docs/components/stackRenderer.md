---
sidebar_label: 'StackRenderer'
---
# StackRenderer `: SpriteRenderer`
The StackRenderer holds information used by the SceneCamera to draw a stack of sprites. Used to create pseudo-3D objects
## Fields
| Name | Value type | Description |
| - | - | - |
| `spritePath` | string | Path used to load sprites, see naming format below |
| `color` | [float, float, float] | Color of every sprite |
| `drawOrder` | float | From 0 to 1, is rendered on top of sprites with a lower draw order |
| `followCamera` | bool | If true: shift the stack away from the center of the camera|
| `magnitude` | float | Multiplies the offset|
| `maximumCameraPerspective` | float | If `followCamera` is true: clamp the offset so it does not exceed this distance from the center of the object|
| `basisVector` | [float, float] | If `followCamera` is false: use this vector multiplied by the sprite iteration to determine the offset of each individual sprite|
| `singleSpriteMode` | bool | Enables single-sprite mode. If enabled: loading sprites will work identically to the SpriteRenderer|
| `singleSpriteAmount` | int | If `singleSpriteMode` is true: determines how many copies of the single sprite are created|
## Naming format