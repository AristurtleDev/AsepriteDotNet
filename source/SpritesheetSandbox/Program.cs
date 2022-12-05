using AsepriteDotNet.Common;
using AsepriteDotNet.Document;
using AsepriteDotNet.Image;
using AsepriteDotNet.Image.Sheet;
using AsepriteDotNet.IO;


string path = Path.Combine(Environment.CurrentDirectory, "adventurer.aseprite");
string outPath = Path.Combine(Environment.CurrentDirectory, "output.png");

AsepriteFile file = AsepriteFileReader.ReadFile(path);
SpritesheetOptions options = new()
{
    SpritesheetType = SpritesheetType.SquarePacked
};
AsepriteSheet sheet = AsepriteSheet.FromFile(file, options);
Png.SaveTo(outPath, sheet.Size, sheet.Pixels);