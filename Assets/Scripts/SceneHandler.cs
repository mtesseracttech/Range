using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void LoadScene(int number)
    {
        SceneManager.LoadScene(number);
        if (number == 0)
        {
            //StaticVariablesScript.ResetStatic();
        }
    }
    public void Exit()
    {
        Application.Quit();
    }

}
