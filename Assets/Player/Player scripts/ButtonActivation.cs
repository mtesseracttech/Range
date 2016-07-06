using UnityEngine;
using System.Collections;
using PickUps;

public class ButtonActivation : MonoBehaviour {

    public float Range = 10;
    public GameObject CrosshairObject;
    public Transform Camera;
    private GameObject _button;
    private RaycastHit _hitInfo;
    private Ray _ray;
    public EndDoorTrigger EndDoorTrigger;


    void FixedUpdate()
    {
        if(Input.GetMouseButtonDown(0))
        LineOfAimHandler();
    }

    void LineOfAimHandler() // Makes the gun always point at the end of ray (crosshair point)
    {
        _ray = new Ray(Camera.transform.position, CrosshairObject.transform.position - Camera.transform.position);
        if (Physics.Raycast(_ray, out _hitInfo, Range))
        {
            if (_hitInfo.collider.gameObject.tag == "buttonYellow")
            {
                _button = _hitInfo.collider.gameObject;
                _button.GetComponent<Animator>().enabled = true;
                EndDoorTrigger.YellowButton = true;
                Debug.Log("yellow true");

            }
            else if (_hitInfo.collider.gameObject.tag == "buttonBlue")
            {
                _button = _hitInfo.collider.gameObject;
                _button.GetComponent<Animator>().enabled = true;
                EndDoorTrigger.BlueButton = true;
                Debug.Log("blue true");
            }
            else if (_hitInfo.collider.gameObject.tag == "buttonOrange")
            {
                _button = _hitInfo.collider.gameObject;
                _button.GetComponent<Animator>().enabled = true;
                EndDoorTrigger.OrangeButton = true;
                Debug.Log("orange true");
            }
            else if (_hitInfo.collider.gameObject.tag == "buttonPink")
            {
                _button = _hitInfo.collider.gameObject;
                _button.GetComponent<Animator>().enabled = true;
                EndDoorTrigger.PinkButton = true;
                Debug.Log("pink true");
            }

            Debug.DrawLine(Camera.transform.position, _hitInfo.point, Color.blue);
        }
    }
}
                