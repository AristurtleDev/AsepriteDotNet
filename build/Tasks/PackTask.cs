using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Pack;
using Cake.Frosting;

namespace AsepriteDotNet.Build;

[TaskName("Pack")]
public sealed class PackTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        DotNetPackSettings settings = new DotNetPackSettings()
        {
            MSBuildSettings = context.DotNetMSBuildSettings,
            Verbosity = DotNetVerbosity.Minimal,
            OutputDirectory = context.NuGetsDirectory,
            Configuration = context.BuildConfiguration
        };

        context.DotNetPack("./source/AsepriteDotNet/AsepriteDotNet.csproj", settings);
    }
}
