using System.Drawing;
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;
using AsepriteDotNet.IO;

//  Load the asperite file telling it to use SystemColor (IColor<System.Drawing.Color>) as the color type
AsepriteFile<SystemColor> aseFile = AsepriteFileLoader.FromFile<SystemColor>("adventurer.aseprite");

//  Get the pixels from the frame
SystemColor[] frame0 = aseFile.Frames[0].FlattenFrame(onlyVisibleLayers: true, includeBackgroundLayer: false, includeTilemapCels: true);

//  Convert to base type of Systrem.Drawing.Color
var pixels = IColorTExtensions.As<SystemColor>(frame0);


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
