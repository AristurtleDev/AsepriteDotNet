using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Common;
using AsepriteDotNet.IO;

AsepriteFile<SystemColor> aseFile = AsepriteFileLoader.FromFile<SystemColor>("adventurer.aseprite");

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
