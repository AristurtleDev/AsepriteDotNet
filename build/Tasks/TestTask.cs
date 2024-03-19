using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNet.Test;
using Cake.Frosting;

namespace AsepriteDotNet.Build;

[TaskName("Test")]
public sealed class TestTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        DotNetTestSettings settings = new DotNetTestSettings()
        {
            MSBuildSettings = context.DotNetMSBuildSettings,
            Verbosity = DotNetVerbosity.Minimal,
            Configuration = context.BuildConfiguration
        };

        context.DotNetTest("./tests/AsepriteDotNet.Tests/AsepriteDotNet.Tests.csproj", settings);
    }
}
