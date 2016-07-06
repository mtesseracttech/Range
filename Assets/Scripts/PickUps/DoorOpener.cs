using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class DoorOpener : MonoBehaviour
{
    public Transform Door1;
    public Transform Door2;
    public  AudioSource AudioDoor;
    public Animator AnimationDoor1;
    public Animator AnimationDoor2;
    public KeyCardPickUps Key;
    public GameObject KeyCard;

    private bool _readyToOpen;
    private bool _readyToClose;
    private bool _open;
    private bool _close;
    private int _counter;
    private int _limit;

    void Awake()
    {
        Key = KeyCard.GetComponent<KeyCardPickUps>();
    }
    // Use this for initialization
    void Start() {
        _readyToOpen = true;
        _readyToClose = false;
        _open = false;
        _close = false;
        _counter = 0;
        _limit = 15;
        AudioDoor = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        Open();
    }
    void OnTriggerEnter(Collider col)
    {
        if (Key.doorKey && col.gameObject.tag == "Player")
        {
            _open = true;
            AudioDoor.Play();
            Debug.Log("door has to open");
        }
    }

    void Open()
    {
        if (_readyToOpen && _open)
        {
            if (_counter <= _limit)
            {
                AnimationDoor1.enabled = true;
                AnimationDoor2.enabled = true;
                _counter += 1;
                if (_counter > _limit)
                {
                    _readyToClose = true;
                    _readyToOpen = false;
                    _open = false;
                    _counter = 0;
                }
            }
        }
    }
}
