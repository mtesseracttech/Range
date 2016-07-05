using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    public float soundValue;
    public int mouseSensitivity;
    public Slider soundButton;
    public Toggle fullscreenToogle;

    public void SoundManipulation()
    {
        soundValue = soundButton.value;
        AudioListener.volume = soundValue*0.01f;
    }

    public void FullscreenOnOff()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
