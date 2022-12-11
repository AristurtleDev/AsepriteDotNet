<h1 align="center">
<img src="https://raw.githubusercontent.com/AristurtleDev/AsepriteDotNet/main/.github/images/aseprite-dotnet-banner.png" alt="AsepriteDotNet Logo">
<br/>
A Cross Platform C# Library for Reading Aseprite Files

[![build and test](https://github.com/AristurtleDev/AsepriteDotNet/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/AristurtleDev/AsepriteDotNet/actions/workflows/build-and-test.yml)
[![License: MIT](https://img.shields.io/badge/📃%20license-MIT-blue?style=flat)](LICENSE)
[![Twitter](https://img.shields.io/badge/%20-Share%20On%20Twitter-555?style=flat&logo=twitter)](https://twitter.com/intent/tweet?text=AsepriteDotNet%20by%20%40aristurtledev%0A%0AA%20new%20cross-platform%20library%20in%20C%23%20for%20reading%20Aseprite%20.ase%2F.aseprite%20files.%20https%3A%2F%2Fgithub.com%2FAristurtleDev%2FAsepriteDotNet%0A%0A%23aseprite%20%23dotnet%20%23csharp%20%23oss%0A)
</h1>

**AsepriteDotNet** is a cross-platform C# library for reading Aseprite (.aseprite/.ase) files.  Once file has been read, the library presents an easy to navigate `AsepriteFile` class containing the data read from the file.

Built against [.NET7](https://dotnet.microsoft.com/en-us/)

# Features
* Simple one line import method (see [Usage](#usage) section below)
* Aseprite editor UI only data is excluded so you only have to navigate through the sprite/image data
* Internal Aseprite flags are converted to easily consumed `bool` properties.
* Supports Aseprite files using **RGBA**, **Grayscale** and **Indexed** color modes.
* Uses native and common C# value types
    * When palette is imported, all palette colors are converted to `System.Drawing.Color` values
    * When pixel data is imported, all pixel data is converted to `System.Drawing.Color` values
* Supports Aseprite 1.3-beta Tileset, Tilemap Layer and Tilemap Cel.
    * Tileset pixels are converted to `System.Drawing.Color` values
    * Tile data found in cels is converted to a `Tile` object
        * Tile X-Flip, Y-Flip, and Rotation values are not fully implemented in Aseprite 1.3-beta (https://github.com/aseprite/aseprite/issues/3603). The values are still read but until they are fully implemented in Aseprite, they will always be `0`.
* Aseprite File can be converted to a packed Spritesheet (`Asepritesheet`).
    * Supports Horizontal, Vertical, and Square packing methods
    * Contains `SpritesheetFrame` elements that correspond to each frame in the packed spritesheet
    * Contains all tag animation data as `SpritesheetAnimation`
    * Contains all slices interpolated per frame as `SpritesheetSlice`
    * Contains a collection of `System.Drawing.Color` values that represent the packed spritesheet image
    * Provides method to save generated packed spritesheet as a PNG (.png) file.

# Usage

```csharp
//  Import namespace
using AsepriteDotNet;

//  Import file using the AsepriteFileReader class
AsepriteFile file = AsepriteFile.Load("file.aseprite");

//  Create an Asepritesheet (aka spritesheet) from the file
Asepritesheet sheet = file.ToAsepritesheet();

//  Save the Asepritesheet as a .png file
sheet.ToPng("file.png");
```

## License
**AsepriteDotNet** is licensed under the **MIT License**.  Please refer to the [LICENSE](LICENSE) file for full license text.
