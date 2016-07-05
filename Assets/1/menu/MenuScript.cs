using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    public static MenuScript instance;

    public static MenuScript GetInstance()
    {
        return instance;
    }
}
