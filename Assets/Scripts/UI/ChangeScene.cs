using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("main"); // 0 - main
    }
    public void Credits()
    {
        SceneManager.LoadScene("credits"); // 0 - main
    }
    public void Back()
    {
        SceneManager.LoadScene("menu"); // 0 - main
    }
    public void Quit()
    {
        Application.Quit();
    }
}
