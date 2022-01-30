using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine.SceneManagement;

public class LoadLevelDebugPage : DebugPage
{
    public override string Header { get; } = "Load Level";

    protected override void RunItems(DebugMenu caller)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            Scene scene = SceneManager.GetSceneByBuildIndex(i);

            
            if (Button(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i))))
                SceneManager.LoadScene(i);
        }
    }
}
