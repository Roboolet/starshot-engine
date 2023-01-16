---
sidebar_label: 'StackRenderer'
---
# StackRenderer *: SpriteRenderer*
The StackRenderer holds information used by the SceneCamera to draw a stack of sprites. Used to create pseudo-3D objects
## Fields
| Name | Value type | Description |
| - | - | - |
| `spritePath` | string | Path used to load the sprite sequence, see naming format below |
| `color` | [float, float, float] | Color of every sprite |
| `drawOrder` | float | From 0 to 1, is rendered on top of sprites with a lower draw order |
| `followCamera` | bool | If true: shift the stack away from the center of the camera|
| `magnitude` | float | Multiplies the offset|
| `maximumCameraPerspective` | float | If `followCamera` is enabled: clamp the offset so it does not exceed this distance from the center of the object|
| `basisVector` | [float, float] | If `followCamera` is disabled: use this vector multiplied by the sprite iteration to determine the offset of each individual sprite|
| `singleSpriteMode` | bool | Enables single-sprite mode. If enabled: loading sprites will work identically to the normal SpriteRenderer|
| `singleSpriteAmount` | int | If `singleSpriteMode` is enabled: determines how many copies of the single sprite are created|
## Loading a sprite sequence
:::info

There is currently no way to load a sprite sequence using a spritesheet or atlas

:::
The sequence loader tries to find an unbroken sequence of numbered sprites, starting with index 1. The `spritePath` given will be used as a prefix to which each number iteration is added as a suffix.

In the example below, the correct `spritePath` value would be "player". The loader will first add "player1.png" to the sequence, then "player2.png"; and so on; until it can't find any more sprites

![A sequence of sprites](/docs/stackSequence.jpg)