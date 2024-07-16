<h1 align="center">
<img src="https://raw.githubusercontent.com/AristurtleDev/AsepriteDotNet/main/.images/banner.png" alt="AsepriteDotNet Logo">
<br/>
A Cross Platform C# Library for Reading Aseprite Files

[![main](https://github.com/AristurtleDev/AsepriteDotNet/actions/workflows/main.yml/badge.svg)](https://github.com/AristurtleDev/AsepriteDotNet/actions/workflows/main.yml)
[![Nuget 1.8.3](https://img.shields.io/nuget/v/AsepriteDotNet?color=blue&style=flat-square)](https://www.nuget.org/packages/AsepriteDotNet/1.8.3)
[![License: MIT](https://img.shields.io/badge/📃%20license-MIT-blue?style=flat)](LICENSE)
[![Twitter](https://img.shields.io/badge/%20-Share%20On%20Twitter-555?style=flat&logo=twitter)](https://twitter.com/intent/tweet?text=AsepriteDotNet%20by%20%40aristurtledev%0A%0AA%20new%20cross-platform%20library%20in%20C%23%20for%20reading%20Aseprite%20.ase%2F.aseprite%20files.%20https%3A%2F%2Fgithub.com%2FAristurtleDev%2FAsepriteDotNet%0A%0A%23aseprite%20%23dotnet%20%23csharp%20%23oss%0A)
</h1>

**AsepriteDotNet** is a cross-platform C# library for reading Aseprite (.aseprite/.ase) files.  Once file has been read, the library presents an easy to navigate `AsepriteFile` class containing the data read from the file.

# Features
* Simple one line import method (see [Usage](#usage) section below)
* Supports Aseprite files using **RGBA**, **Grayscale** and **Indexed** color modes.
* Supports all Aseprite layer blend modes.
* Support Aseprite Tileset, Tilemap Layer, and Tilemap Cel.
* Provides processors to convert the Aseprite data loaded into common formats, including:
    * Sprite
    * SpriteSheet
    * TextureAtlas
    * Tileset
    * Tilemap
    * AnimatedTilemap

# Installation
Install via NuGet
```
dotnet add package AsepriteDotNet --version 1.8.3
```

# Usage
## Load Aseprite file
The following example demonstrates how to load an Aseprite file from disk.

```csharp
//  Import namespaces
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.IO;

//  Load the Aseprite file from disk.
AsepriteFile aseFile = AsepriteFileLoader.FromFile("file.aseprite");
```

## Get Frame Color Data
In Aseprite, each frame is a collection of cels, with each cel on a different layer and each layer having its own blending mode.  To get the full image of a single frame, the frame needs to be flattened.  Flatting a frame blends all cel elements, starting with the bottom most layer and blending the layer above it until all layers have blended producing a single iamge.

Doing this in `AsepriteDotNet` will produce a `Rgba32[]` containing the pixel data from flattening the frame. You can specify if only cel elements that are on a visible layer should be included, if cels on the background layer should be included, and if tilemap cels should be included.

```csharp
//  Import namespaces
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Common;
using AsepriteDotNet.IO;

//  Load the Aseprite file from disk.
AsepriteFile file = AsepriteFileLoader.FromFile("file.aseprite");

//  Flatten the frame to get the pixel data
Rgba32[] framePixels = file.Frames[0].FlattenFrame(onlyVisibleLayers: true, includeBackgroundLayer: false, includeTilemapCels: false);
```

## Processor
`AsepriteDotNet` provides several out-of-the-box processors that can be used to transform the data in the `AsepriteFile` that is loaded into common formats.  These are out-of-the-box general solutions and may not fit all use cases, but should give an idea if you want to create your own.

Some processors take a `ProcessorOptions` argument.  The following table defines the values that can be set for these options

| Property | Type | Description |
| --- | --- | --- |
| **OnlyVisibleLayers** | `bool` | Indicates whether only visible layers should be processed. |
| **IncludeBackgroundLayer** | `bool` | Indicates whether the layer assigned as the background layer should be processed. |
| **IncludeTilemapLayers** | `bool` | Indicates whether tilemap layers should be processed. |
| **MergeDuplicateFrames** | `bool` | Indicates whether duplicates frames should be merged. |
| **BorderPadding** | `int` | The amount of transparent pixels to add to the edge of the generated texture. |
| **Spacing** | `int` | The amount of transparent pixels to add between each texture region in the generated texture. |
| **InnerPadding** | `int` | The amount of transparent pixels to add around the edge of each texture region in the generated texture. |

The examples below demonstrate how to transform the `AsepriteFile` using one of the processors:

```cs
//  Import namespaces
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Common;
using AsepriteDotNet.IO;
using AsepriteDotNet.Processors;

//  Load the Aseprite file from disk.
AsepriteFile aseFile = AsepriteFileLoader.FromFile("file.aseprite");

//  Create new processor options or use the provided defaults
ProcessorOptions options = ProcessorOptions.Default;

//  Use the Sprite processor to create a sprite from a single frame
Sprite sprite = SpriteProcessor.Process(aseFile, frameIndex: 0, options);

//  Use the TextureAtlas processor to generate a texture atlas from the Aseprite file.  A TextureAtlas generates a packed texture with all frames and TextureRegion data describing the bounds of each frame in the source texture
TextureAtlas atlas = TextureAtlasProcessor.Process(aseFile, options);

//  Use the SpriteSheet processor to generate a sprite sheet from the Aseprite file.  A SpriteSheet contains a TextureAtlas as well as AnimationTags which define the animations created from tags in Aseprite.
SpriteSheet spriteSheet = SpriteSheetProcessor.Process(aseFile, options);

//  Use the TileSetProcessor to generate a texture from a tileset in the Aseprite file
Tileset tileset = TilesetProcessor.Process(aseFile, tilesetIndex: 0, options);

//  Use the TilemapProcessor to generate a tilemap from a specified frame in the Aseprite file
Tilemap tilemap = TilemapProcessor.Process(aseFile frameIndex: 0, options);

//  Use the AnimatedTilemapProcess to generate an animated tilemap from the Aseprite file
AnimatedTilemap animatedTilemap = AnimatedTilemapProcessor.Process(aseFile, options);
```

# License
**AsepriteDotNet** is licensed under the **MIT License**.  Please refer to the [LICENSE](LICENSE) file for full license text.

# Contributors
<a href="https://github.com/aristurtledev/asepritedotnet/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=aristurtledev/asepritedotnet" />
</a>

Made with [contrib.rocks](https://contrib.rocks).

