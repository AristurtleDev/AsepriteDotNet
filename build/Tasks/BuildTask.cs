using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Frosting;

namespace AsepriteDotNet.Build;

[TaskName("Build")]
public sealed class BuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        DotNetBuildSettings settings = new DotNetBuildSettings()
        {
            MSBuildSettings = context.DotNetMSBuildSettings,
            Verbosity = DotNetVerbosity.Minimal,
            Configuration = context.BuildConfiguration
        };

        context.DotNetBuild("./source/AsepriteDotNet/AsepriteDotNet.csproj", settings);
    }
}
