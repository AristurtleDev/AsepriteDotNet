using AsepriteDotNet.Image.Sheet;
using AsepriteDotNet.IO;
using AsepriteDotNet.Document;

string path1 = Path.Combine(Environment.CurrentDirectory, "test.aseprite");
string path2 = Path.Combine(Environment.CurrentDirectory, "output.png");
AsepriteFile file = AsepriteFileReader.ReadFile(path1);
SpritesheetOptions options = new() { SpritesheetType = SpritesheetType.SquarePacked };
AsepriteSheet sheet = AsepriteSheet.FromFile(file, options);
sheet.SaveAsPng(path2);