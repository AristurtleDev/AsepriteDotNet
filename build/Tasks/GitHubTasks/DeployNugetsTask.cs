using Cake.Common;
using Cake.Common.Build;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.NuGet.Push;
using Cake.Core.IO;
using Cake.Frosting;

namespace AsepriteDotNet.Build;

[TaskName("DeployNuGets")]
[IsDependentOn(typeof(DownloadArtifactsTask))]
public sealed class DeployNugetsTask : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context) => context.BuildSystem().IsRunningOnGitHubActions;

    public override void Run(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        DotNetNuGetPushSettings settings = new DotNetNuGetPushSettings()
        {
            ApiKey = context.EnvironmentVariable("NUGET_API_KEY"),
            Source = "https://api.nuget.org/v3/index.json"
        };

        context.DotNetNuGetPush("nugets/*.nupkg", settings);
    }
}
