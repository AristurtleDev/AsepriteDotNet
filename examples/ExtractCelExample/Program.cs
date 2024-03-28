using AsepriteDotNet;
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.IO;

AsepriteFile aseFile = AsepriteFileLoader.FromFile("adventurer.aseprite");
Texture head = aseFile.ExtractCel(0, "head");
PngWriter.SaveTo("head.png", head.Size.Width, head.Size.Height, head.Pixels.ToArray());
