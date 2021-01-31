using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(0); // 0 - main
    }
    public void Credits()
    {
        SceneManager.LoadScene(2); // 0 - main
    }
    public void Back()
    {
        SceneManager.LoadScene(1); // 0 - main
    }
    public void Quit()
    {
        Application.Quit();
    }
}
