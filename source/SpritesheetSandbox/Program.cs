using AsepriteDotNet.Image;
using AsepriteDotNet.Document;

string path1 = Path.Combine(Environment.CurrentDirectory, "adventurer.aseprite");
string path2 = Path.Combine(Environment.CurrentDirectory, "output.png");


AsepriteFile aseFile = AsepriteFile.Load(path1);

//  Output the canvas size
Console.WriteLine($"Canvas Size: {aseFile.Size}");

//  Output frame data
Console.WriteLine($"Frame Count: {aseFile.Frames.Count}");
for (int i = 0; i < aseFile.Frames.Count - 100; i++)
{
    Frame frame = aseFile.Frames[i];

    Console.WriteLine
    (
        $"""
            - Frame {i}:
                - Duration: {frame.Duration}
                - Cel Count: {frame.Cels.Count}
        """
    );
}

//  Output layer data
Console.WriteLine($"Layer Count: {aseFile.Layers.Count}");
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
Console.WriteLine($"Tag Count: {aseFile.Tags.Count}");
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
Console.WriteLine($"Slice Count: {aseFile.Slices.Count}");
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
                - Total Keys: {slice.Keys.Count}
        """
    );
}

//  Output tileset data
Console.WriteLine($"Tileset Count: {aseFile.Tilesets.Count}");
for (int i = 0; i < aseFile.Tilesets.Count; i++)
{
    Tileset tileset = aseFile.Tilesets[i];

    Console.WriteLine
    (
        $"""
            - Tileset {i}:
                - ID: {tileset.ID}
                - Name: {tileset.Name}
                - Tile Size: {tileset.TileSize}
                - Tile Count: {tileset.TileCount}
        """
    );
}





// //  Define the SpritesheetOptions to use when creating the Asepritesheet
// SpritesheetOptions options = new()
// {
//     OnlyVisibleLayers = true,
//     MergeDuplicates = true,
//     PackingMethod = SpritesheetPackingMethod.SquarePacked
// };

// //  Create the Asepritesheet instance
// AsepriteSheet aseSheet = aseFile.ToAsepritesheet(options);

// Console.WriteLine
// (
//     $"""
//     Spritesheet Size: {aseSheet.Size}
//     Frame Count:      {aseSheet.Frames.Count}
//     Animation Count:  {aseSheet.Animations.Count}
//     Pixel Count:      {aseSheet.Pixels.Count}
//     """
// );
