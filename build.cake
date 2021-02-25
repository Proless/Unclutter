var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var solutionFolder ="./src";

Task("Clean");

Task("Restore")
    .Does(()=> {
        DotNetCoreRestore(solutionFolder);
    });


Task("Build")
.IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(()=> {
        DotNetCoreBuild(solutionFolder, new DotNetCoreBuildSettings{
            NoRestore = true,
            Configuration = configuration
        });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(()=> {
        DotNetCoreTest(solutionFolder, new DotNetCoreTestSettings{
            NoRestore = true,
            NoBuild = true,
            Configuration = configuration
        });
    });

RunTarget(target);

