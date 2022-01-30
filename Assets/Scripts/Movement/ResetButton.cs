using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
            ResetToLastCheckpoint();

        if (Input.GetKeyDown(KeyCode.Escape))
            ResetLevel();
    }

    public static void ResetToLastCheckpoint()
    {
        if (PlayModeSwitchTrigger.HasCheckpoint())
        {
            FindObjectOfType<PuzzleObjectInteraction>().LoadRotationsFromCheckpoint();
        }
            
        else
            ResetLevel();
    }

    public static void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
