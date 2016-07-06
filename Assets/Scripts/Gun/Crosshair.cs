using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour
{
    private float sizeModifier;
    private float _width;
    private float _height;
    private Rect crossRect;
    private Texture crossTexture;
    // Use this for initialization
    void Start()
    {
        crossTexture = Resources.Load("Textures/crosshair") as Texture;
        sizeModifier = 0.05f;
        _width = crossTexture.width * sizeModifier;
        _height = crossTexture.height * sizeModifier;
        crossRect = new Rect(Screen.width / 2 - _width / 2, Screen.height / 2 - _height / 2, _width, _height);
    }
    void OnGUI()
    {
        GUI.DrawTexture(crossRect, crossTexture);
    }
}
