<h1 align="center">
<img src="https://raw.githubusercontent.com/AristurtleDev/AsepriteDotNet/main/.github/images/aseprite-dotnet-banner.png" alt="AsepriteDotNet Logo">
<br/>
A Cross Platform C# Library for Reading Aseprite Files

[![build and test](https://github.com/AristurtleDev/AsepriteDotNet/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/AristurtleDev/AsepriteDotNet/actions/workflows/build-and-test.yml)
[![Nuget 0.2.2](https://img.shields.io/nuget/v/AsepriteDotNet?color=blue&style=flat-square)](https://www.nuget.org/packages/AsepriteDotNet/0.2.2)
[![License: MIT](https://img.shields.io/badge/📃%20license-MIT-blue?style=flat)](LICENSE)
[![Twitter](https://img.shields.io/badge/%20-Share%20On%20Twitter-555?style=flat&logo=twitter)](https://twitter.com/intent/tweet?text=AsepriteDotNet%20by%20%40aristurtledev%0A%0AA%20new%20cross-platform%20library%20in%20C%23%20for%20reading%20Aseprite%20.ase%2F.aseprite%20files.%20https%3A%2F%2Fgithub.com%2FAristurtleDev%2FAsepriteDotNet%0A%0A%23aseprite%20%23dotnet%20%23csharp%20%23oss%0A)
</h1>

**AsepriteDotNet** is a cross-platform C# library for reading Aseprite (.aseprite/.ase) files.  Once file has been read, the library presents an easy to navigate `AsepriteFile` class containing the data read from the file.

Built against [.NET6 and .NET7](https://dotnet.microsoft.com/en-us/)

# Installation
Install via NuGet
```
dotnet add package AsepriteDotNet --version 0.2.2
```

# Features
* Simple one line import method (see [Usage](#usage) section below)
* Aseprite editor UI only data is excluded so you only have to navigate through the sprite/image data
* Internal Aseprite flags are converted to easily consumed `bool` properties.
* Supports Aseprite files using **RGBA**, **Grayscale** and **Indexed** color modes.
* Supports Aseprite 1.3-beta Tileset, Tilemap Layer and Tilemap Cel.
    * Tile data found in cels is converted to a `Tile` object
        * Tile X-Flip, Y-Flip, and Rotation values are not fully implemented in Aseprite 1.3-beta (https://github.com/aseprite/aseprite/issues/3603). The values are still read but until they are fully implemented in Aseprite, they will always be `0`.
* Aseprite File can be converted to a packed `Asepritesheet` which contains a `Spritesheet` and a collection of `Tilesheet` for each tileset
* Individual Aseprite components such as `Frame`, `Cel`, and `Tileset` components, as well as generated `Spritesheet`, `Tilesheet` and `Asepritesheet` can be saved to disk as a .png file.

# Usage
## Load Aseprite file
The following example demonstrates how to load an Aseprite file from disk.

```csharp
//  Import namespace
using AsepriteDotNet;

//  Load the Aseprite file from disk.
AsepriteFile file = AsepriteFile.Load("file.aseprite");
```

## Get Frame Color Data
Each `Frame` in Aseprite is a collection of `Cel` elements, with each `Cel` on a different `Layer`.  To get the full image of a single `Frame`, the `Frame` needs to be flattened.  Flattening a `Frame` blends all `Cel` elements, starting with the top most `Layer` and blending down until a single image is produced.

Doing this in `AsepriteDotNet` will produce a `Color[]` containing the pixel data from flattening the `Frame`.  You can specify if only `Cel` elements that are on a `Layer` that is visible should be included.

```csharp
using AsepriteDotNet;
using AsepriteDotNet.Common;

AsepriteFile file = AsepriteFile.Load("file.aseprite");

Color[] framePixels = file.Frame[0].FlattenFrame(onlyVisibleLayers: true);
```

## Save Components To PNG
Individual components can be saved as a PNG file, including `Frame`, `ImageCel` and `Tileset` components

```csharp
using AsepriteDotNet;

AsepriteFile file = AsepriteFile.Load("file.aseprite");

//  Save a frame as png
file.Frames[0].ToPng("frame.png");

//  Save an individual ImageCel as png
if(file.frames[0].Cels[0] is ImageCel imageCel)
{
    imageCel.ToPng("cel.png");
}

//  Save a tileset as png
file.Tilesets[0].ToPng("tileset.png");
```

## Create `Spritesheet`
A `Spritesheet` can be created from the `AsepriteFile` instance that contains:
*   `Color[]` image data generated from all `Frame` data
*   `SpritesheetFrame` elements for each frame in the image data which includes the source rectangle and duration
    *   Each `SpritesheetFrame` also includes `Slice` data for that `Frame` if there was a `Slice` on that `Frame`.
*   `SpritesheetAnimation` elements for all `Tag` data that includes the information about the animation such as frames and loop direction.

`Spritesheet` can also be saved as a .png file

```csharp
using AsepriteDotNet;
using AsepriteDotNet.Image;

AsepriteFile file = AsepriteFile.Load("file.aseprite");

//  Options to adhere to when generating a spritesheet
SpritesheetOptions options = new
{
    //  Should only visible layers be included?
    OnlyVisibleLayers = true,

    //  Should duplicate frames be merged into one frame?
    MergeDuplicate = true,  

    //  Can be Horizontal, Vertical, or Square.        
    PackingMethod = SquarePacked,   

    //  Padding added between each frame and edge of spritesheet.
    BorderPadding = 0,  

    //  Amount of transparent pixels to added between each frame.
    Spacing = 0,

    //  Amount of transparent pixels to add to the inside of each frame's edge.
    InnerPadding = 0    
}

//  Create the spritesheet using the options from the file.
Spritesheet sheet = file.ToSpritesheet(options);

//  Save the spritesheet as a .png
sheet.ToPng("sheet.png");
```

## Create `Tilesheet`
Each `Tileset` in the `AsepriteFile` can be converted into a `Tilesheet` that
contains:
*   The `Name` of the `Tileset
*   `Color[]` image data of the `Tileset`
*   A collection of `TilesheetTile` elements that indicate the source rectangle in the image data for each tile.

`Tilesheet` can also be saved as a .png file

```csharp
using AsepriteDotNet;
using AsepriteDotNet.Image;

AsepriteFile file = AsepriteFile.Load("file.aseprite");

//  Options to adhere to when generating a tilesheet
SpritesheetOptions options = new
{
    //  Should duplicate tiles be merged into one tile?
    MergeDuplicate = true,  

    //  Can be Horizontal, Vertical, or Square.        
    PackingMethod = SquarePacked,   

    //  Padding added between each tile and edge of tilesheet.
    BorderPadding = 0,  

    //  Amount of transparent pixels to added between each tile.
    Spacing = 0,

    //  Amount of transparent pixels to add to the inside of each tile's edge.
    InnerPadding = 0    
}

//  Create the tilesheet form a tile using the options.
Tilesheet sheet = file.Tileset[0].ToTilesheet(options);

//  Save the tilesheet as a .png
sheet.ToPng("tilesheet.png");
```

## Create `Asepritesheet`
An `Asepritesheet` combines both the `Spritesheet` and `Tilesheet` data into a single class instance.  

```csharp
using AsepriteDotNet;
using AsepriteDotNet.Image;

AsepriteFile file = AsepriteFile.Load("file.aseprite");

//  Create options for spritesheet generation.  Default constructor will create
//  default options using only visible layer, merge duplicates, square packed,
//  and 0 for padding and spacing
SpritesheetOptions spriteSheetOptions = new();

//  Create options for tilesheet generation.  Default constructor will create
//  default options using merge duplicates, square packed, and 0 for padding and
//  spacing
TilesheetOptions tilesheetOptions = new();

//  Create the Asepritesheet from the file using the options
Asepritesheet sheet = file.ToAsepritesheet(spritesheetOptions, tilesheetOptions);
```


# License
**AsepriteDotNet** is licensed under the **MIT License**.  Please refer to the [LICENSE](LICENSE) file for full license text.
