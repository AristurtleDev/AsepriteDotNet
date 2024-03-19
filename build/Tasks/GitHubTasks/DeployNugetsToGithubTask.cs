using Cake.Common;
using Cake.Common.Build;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.NuGet.Push;
using Cake.Core.IO;
using Cake.Frosting;

namespace AsepriteDotNet.Build;

[TaskName("DeployNugetsToGithub")]
public sealed class DeployNugetsToGitHubTask : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context) => context.BuildSystem().IsRunningOnGitHubActions;

    public override void Run(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        string repositoryOwner = context.GitHubActions().Environment.Workflow.RepositoryOwner;
        DotNetNuGetPushSettings settings = new DotNetNuGetPushSettings()
        {
            ApiKey = context.EnvironmentVariable("GITHUB_TOKEN"),
            Source = $"https://nuget.pkg.github.com/{repositoryOwner}/index.json"
        };

        context.DotNetNuGetPush("nugets/*.nupkg", settings);
    }
}
