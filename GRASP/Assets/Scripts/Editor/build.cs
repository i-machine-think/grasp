using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

public class BatchBuildScript
{
    [MenuItem("Build/All Level 2 Scenes")]
    public static void BuildLevel1Scenes()
    {
        string sceneFolderPath = "Assets/Scenes/Level2";
        string[] scenesInFolder = Directory.GetFiles(sceneFolderPath, "*.unity", SearchOption.AllDirectories);
        foreach (string scenePath in scenesInFolder)
        {
            BuildScene(scenePath, "Level2");
        }
    }

    [MenuItem("Build/All Level 1 Scenes")]
    public static void BuildLevel0Scenes()
    {
        string sceneFolderPath = "Assets/Scenes/Level1";
        string[] scenesInFolder = Directory.GetFiles(sceneFolderPath, "*.unity", SearchOption.AllDirectories);
        foreach (string scenePath in scenesInFolder)
        {
            BuildScene(scenePath, "Level1");
        }
    }
    
    [MenuItem("Build/Current scene")]
    public static void BuildCurrentScene()
    {
        string scenePath = SceneManager.GetActiveScene().path;
        BuildScene(scenePath);
    }

    private static void BuildScene(string scenePath, string subdirectory)
    {
        string sceneName = Path.GetFileNameWithoutExtension(scenePath);
        string outputPath = $"build/{subdirectory}/{sceneName}/Simulator";

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new[] { scenePath },
            locationPathName = outputPath,
            target = BuildTarget.StandaloneLinux64,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    private static void BuildScene(string scenePath)
    {
        string sceneName = Path.GetFileNameWithoutExtension(scenePath);
        string outputPath = $"build/{sceneName}/Simulator";

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new[] { scenePath },
            locationPathName = outputPath,
            target = BuildTarget.StandaloneLinux64,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}

