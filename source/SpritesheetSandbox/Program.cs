using AsepriteDotNet.Image;
using AsepriteDotNet.IO;
using AsepriteDotNet.Document;
using System.Drawing;

string path1 = Path.Combine(Environment.CurrentDirectory, "adventurer.aseprite");
string path2 = Path.Combine(Environment.CurrentDirectory, "output.png");
AsepriteFile aseFile = AsepriteFileReader.ReadFile(path1);

//  Define the SpritesheetOptions to use when creating the Asepritesheet
SpritesheetOptions options = new()
{
    OnlyVisibleLayers = true,
    MergeDuplicates = true,
    PackingMethod = SpritesheetPackingMethod.SquarePacked
};

//  Create the Asepritesheet instance
AsepriteSheet aseSheet = aseFile.ToAsepritesheet(options);

Console.WriteLine
(
    $"""
    Spritesheet Size: {aseSheet.Size}
    Frame Count:      {aseSheet.Frames.Count}
    Animation Count:  {aseSheet.Animations.Count}
    Pixel Count:      {aseSheet.Pixels.Count}
    """
);
