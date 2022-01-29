using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine.SceneManagement;

public class HomeDebugPage : DebugPage
{
    public override string Header { get; } = "Home";

    public static bool disableGroundCheck;

    RenewableLazy<CreateSpriteFromCamera> cameraInstance = new RenewableLazy<CreateSpriteFromCamera>(
        () => UnityEngine.Object.FindObjectOfType<CreateSpriteFromCamera>()
    );

    protected override void RunItems(DebugMenu caller)
    {
        if (Toggle("Disable ground check", disableGroundCheck))
            disableGroundCheck = !disableGroundCheck;

        if (cameraInstance.Value)
        {
            if (Button("Swap Mode"))
                cameraInstance.Value.TogglePlayMode();
        }

        if (Button("Reset"))
            SceneManager.LoadScene(0);
    }
}
