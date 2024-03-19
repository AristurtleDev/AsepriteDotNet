using Cake.Frosting;

namespace AsepriteDotNet.Build;

[TaskName("Default")]
[IsDependentOn(typeof(BuildTask))]
[IsDependentOn(typeof(TestTask))]
[IsDependentOn(typeof(PackTask))]
public sealed class DefaultTask : FrostingTask<BuildContext> { }
