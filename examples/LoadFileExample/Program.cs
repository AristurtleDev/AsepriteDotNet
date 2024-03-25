// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.IO;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///
/// Load the file using the AsepriteFileLoader.  In this example, we are passing the path to the file.  There is also
/// an overload where you can pass a stream instead if you needed.
/// 
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
AsepriteFile aseFile = AsepriteFileLoader.FromFile("adventurer.aseprite");

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///
/// Output information about the file read.
/// 
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
string info =
$"""
    Filename:       {aseFile.Name}
    Color Depth:    {aseFile.ColorDepth}
    Palette Size:   {aseFile.Palette.Count}
    Canvas Size:    {aseFile.CanvasWidth} x {aseFile.CanvasHeight}
    Frame Count:    {aseFile.Frames.Length}
    Layers Count:   {aseFile.Layers.Length}
    Slices Count:   {aseFile.Slices.Length}
    Tags Count:     {aseFile.Tags.Length}
    Tileset Count:  {aseFile.Tilesets.Length}
    """;

Console.WriteLine(info);
