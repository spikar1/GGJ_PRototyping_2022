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

    protected override void RunItems(DebugMenu caller)
    {
        if (Toggle("Disable ground check", disableGroundCheck))
            disableGroundCheck = !disableGroundCheck;

        if (Toggle("Noclip", NoclipMotion.noclip))
            NoclipMotion.noclip = !NoclipMotion.noclip;

        if (Button("Play Mode [" + PlayModeManager.Instance.CurrentMode + "]"))
            PlayModeManager.Instance.SwitchModes();

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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
