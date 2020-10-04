using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasFunctions : MonoBehaviour
{
    public int loadindex;

    public void Reload()
    {
        LoadLevel(1);
    }

    public void MainMenu()
    {
        LoadLevel(0);
    }

    void LoadLevel(int levelindex)
    {
        SceneManager.LoadScene(levelindex);
    }
}
