using AsepriteDotNet.Image;
using AsepriteDotNet.IO;
using AsepriteDotNet.Document;
using System.Drawing;

string path1 = Path.Combine(Environment.CurrentDirectory, "adventurer.aseprite");
string path2 = Path.Combine(Environment.CurrentDirectory, "output.png");


AsepriteFile aseFile = AsepriteFile.Load(path1);

//  Output the canvas size
Console.WriteLine($"Canvas Size: {aseFile.Size}");

//  Output frame data
Console.WriteLine($"Frame Count: {aseFile.Frames.Count}");
for (int i = 0; i < aseFile.Frames.Count; i++)
{
    Frame frame = aseFile.Frames[i];

    Console.WriteLine($"    - Frame {i}");
    Console.WriteLine($"        - Type: {frame.GetType()}");
    Console.WriteLine($"        - Duration: {frame.Duration}");
    Console.WriteLine($"        - Cel Count: {frame.Cels.Count}");

    Console.WriteLine
    (
        $"""
            - Frame {i}:
                - Type: {frame.GetType()}
                - Duration: {frame.Duration}
                - Cel Count: {frame.Cels.Count}
        """
    );
}

//  Output layer data
for (int i = 0; i < aseFile.Layers.Count; i++)
{
    Layer layer = aseFile.Layers[i];

    Console.WriteLine
    (
        $"""
            - Layer {i}:
                - Name: {layer.Name}
                - Type: {layer.GetType()}
                - Visible: {layer.IsVisible}
                - Blend Mode: {layer.BlendMode}
                - Child Level: {layer.ChildLevel}
        """
    );
}

//  Output tag data
for (int i = 0; i < aseFile.Tags.Count; i++)
{
    Tag tag = aseFile.Tags[i];

    Console.WriteLine
    (
        $"""
            - Tag {i}:
                - Name: {tag.Name}
                - From: {tag.From}
                - To: {tag.To}
                - Loop Direction: {tag.LoopDirection}
        """
    );
}

//  Output slice data
for (int i = 0; i < aseFile.Slices.Count; i++)
{
    Slice slice = aseFile.Slices[i];

    Console.WriteLine
    (
        $"""
            - Slice {i}:
                - Name: {slice.Name}
                - 9-Patch: {slice.IsNinePatch}
                - Has Pivot: {slice.HasPivot}
                - Total Keys: {slice.Count}
        """
    );
}





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
