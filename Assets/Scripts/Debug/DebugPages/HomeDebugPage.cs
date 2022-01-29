using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using UnityEngine;
using UnityEngine.SceneManagement;

using UnityObject = UnityEngine.Object;

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

        if (Toggle("Noclip", NoclipMotion.noclip))
            NoclipMotion.noclip = !NoclipMotion.noclip;

        if (cameraInstance.Value)
        {
            if (Button("Swap Mode"))
                cameraInstance.Value.TogglePlayMode();
        }

        Separator();

        if (Button("Spawn Ball"))
        {
            UnityObject.Instantiate(
                original: caller.BallPrefab, 
                position: UnityObject.FindObjectOfType<GroundController>().transform.position, 
                rotation: Quaternion.identity    
            );
        }

        Separator();

        if (Button("Reset"))
            SceneManager.LoadScene(0);
    }
}
