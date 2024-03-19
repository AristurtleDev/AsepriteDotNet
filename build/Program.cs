using AsepriteDotNet.Build;
using Cake.Frosting;

return new CakeHost()
            .UseWorkingDirectory("../")
            .UseContext<BuildContext>()
            .Run(args);
