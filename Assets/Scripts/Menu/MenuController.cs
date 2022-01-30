using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    public void StartLevelByInt(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }
}