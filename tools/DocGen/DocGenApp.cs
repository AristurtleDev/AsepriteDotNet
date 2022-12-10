using XmlDocMarkdown.Core;

args = new string[]
{
    "AsepriteDotNet",
    "../../.docs",
    "--source", "https://github.com/AristurtleDev/AsepriteDotNet/tree/main/source/AsepriteDotNet",
    "--namespace", "AsepriteDotNet",
    "--visibility", "public",
    "--obsolete",
    "--skip-unbrowsable",
    "--clean",
    "--permalink", "pretty",
    "--namespace-pages",
    "--newline", "auto"
};

return XmlDocMarkdownApp.Run(args);