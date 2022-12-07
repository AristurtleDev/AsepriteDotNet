using AsepriteDotNet.Image.Sheet;
using AsepriteDotNet.IO;
using AsepriteDotNet.Document;

string path1 = Path.Combine(Environment.CurrentDirectory, "adventurer.aseprite");
string path2 = Path.Combine(Environment.CurrentDirectory, "output.png");
AsepriteFile file = AsepriteFileReader.ReadFile(path1);
AsepriteSheet sheet = file.ToSpriteSheet(new() { SpritesheetType = SpritesheetPackingMethod.SquarePacked });
sheet.SaveAsPng(path2);