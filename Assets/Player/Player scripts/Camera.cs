
using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
    public GameObject Player;
    private Animator cameraAnimator;
    public GameObject Hand;
    private Animator handAnimator;
    //public Animation anim;
    private PlayerControls2 playerScript;

    private enum State { Idle, Move, Run};
    private State _state;

    // Variables used for camera shake effect when the player jumps
    public int JumpLimit = 5;
    private bool _shake;
    private bool _jumpUp;
    private bool _jumped;
    private float _jumpCounter;

    private bool up = false;
    // Variables used for View Bobbing
    private float _cameraY;
    void Awake()
    {
        cameraAnimator = gameObject.GetComponent<Animator>();
        playerScript = Player.GetComponent<PlayerControls2>();
        handAnimator = Hand.GetComponent<Animator>();
    }
    // Use this for initialization
    void Start()
    {
        _state = State.Idle;
        _cameraY = gameObject.transform.localPosition.y;
        
        _jumpCounter = 0;

        _shake = false;
        _jumpUp = false;
        _jumped = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleAnimations();
    }
    void HandleAnimations()
    {
        if (_state == State.Idle && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            handAnimator.SetBool("Walk", true);
            cameraAnimator.SetBool("Move", true);
            _state = State.Move;
        }
        if (_state == State.Move)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                cameraAnimator.SetBool("Move", false);
                cameraAnimator.SetBool("Run", true);
                handAnimator.SetBool("Run", true);  
                _state = State.Run;
            }
            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                handAnimator.SetBool("Walk", false);
                cameraAnimator.SetBool("Move", false);
                _state = State.Idle;
            }
            
        }
        if (_state == State.Run)
        {
            if (!Input.GetKey(KeyCode.LeftShift) || (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)))
            {
                cameraAnimator.SetBool("Run", false);
                handAnimator.SetBool("Run", false);
                handAnimator.SetBool("Walk", false);
                _state = State.Idle;
            }
        }
    }
}

