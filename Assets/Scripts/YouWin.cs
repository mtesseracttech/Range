using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class YouWin : MonoBehaviour
{
    public GameObject winscreen;

    void Update()
    {
        /**
        if (!StaticVariablesScript.bossAlive)
        {
            StartCoroutine(ShowWinScreen());
            //Debug.Log("win");
        }
        /**/
    }

    IEnumerator ShowWinScreen()
    {
        winscreen.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        winscreen.SetActive(false);
        SceneManager.LoadScene(0);
    }

}
