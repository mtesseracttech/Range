using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingEndGameScreen : MonoBehaviour {

    [SerializeField]
    private int _scene;

    private bool showupmessage;

    void OnTriggerEnter()
    {
        showupmessage = true;
        StartCoroutine(LoadNewScene());
    }

    void OnTriggerExit()
    {
        showupmessage = false;
    }


    void OnGUI()
    {
        if (showupmessage)
        GUI.Box(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 100, 120, 25), "You win");
    }

    IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(3);
      //  StaticVariablesScript.ResetStatic();
        AsyncOperation async = SceneManager.LoadSceneAsync(_scene);
        while (!async.isDone)
        {
            yield return null;
        }

    }
}
