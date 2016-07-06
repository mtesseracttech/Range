using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOver;
    private PlayerControls2 movment;
    private Crosshair cross;

    void Start()
    {
        
        movment = GetComponent<PlayerControls2>();
        cross = GetComponent<Crosshair>();
        gameOver.SetActive(false);
    }

    void Update()
    {
        /**
        if (Health.health <= 0)
        {
            GameOverShow();
        }
        else
        {
            gameOver.SetActive(false);
            Time.timeScale = 1.0f;
        }
        /**/
    }

    void GameOverShow()
    {
        gameOver.SetActive(true);
        Cursor.visible = true;
        movment.enabled = false;
        cross.enabled = false;
        Time.timeScale = 0.0f;
    }

    public void Restart()
    {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
        
    }

    public void Exit()
    {
        Application.Quit();
    }
}
