using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControls2 : MonoBehaviour
{
    //      GUN variables
    [Header("Gun information")]
    public GameObject HandModel;
    public GameObject GunModel;
    public GameObject CrosshairObject;
    public GameObject BulletPrefab;
    public Transform GunTipObject;
    //public Text AmmunationText;
   // public AudioSource AudioRunBreath;
   // public AudioSource AudioWalk;
   // public AudioSource AudioGunShoot;
    public float FireDelay;
    public static int _ammo = 10;//StaticVariablesScript.playerAmmo;
    public float Range = 50;

    private GameObject _bullet;
    private Vector3 _prevRayCastPoint;
    private LineRenderer _lineRenderer;
    private WaitForSeconds _shotLength = new WaitForSeconds(.07f);
    public Light AmmoLight;

    private int ammoMax = 6;
    private float _deltaTime;
    private bool _slowMotion;
    private float _fireDelta;
    private bool _countFireDelta;

    // Variables used for raycasting and shooting
    private bool blowback;
    private bool backwards;
    private int blowbackLimit;
    private int blowbackCounter;

    // Variables used for Line of Aim Handling
    private RaycastHit _hitInfo;
    private Ray _ray;

    [Header("Particle Effects")]
    public ParticleSystem SmokeParticleSystem;

    //private WaitForSeconds shotLength = new WaitForSeconds(.07f);
    private AudioSource source;

    // Variables used for player movement 
    [Header("Player movements")]
    private Rigidbody rb;
    public int MaxMagnitude = 3;
    public int accelerationValue;
    public int MovementSpeed = 1;
    public float JumpSpeed = 500;
    public float SlowDownFactor = 0.945f;
    public int MaxMovementSpeed = 3;

    private Rigidbody _rigidBody;
    private float _maxMagnitude;
    private float _forwardSpeed = 0f;
    private float _sidewaysSpeed = 0f;
    private bool _grounded = true;
    // Variables used for player movement

    public enum RotationAxes
    {
        MouseXAndY = 0, MouseX = 1, MouseY = 2
    }

    // Variables used for player and camera rotation
    [Header("Camera Sensitivity")]
    public Transform Camera;
    public RotationAxes Axes = RotationAxes.MouseXAndY;
    public float SensitivityX = 15F;
    public float SensitivityY = 15F;
    public float MinimumX = -360F;
    public float MaximumX = 360F;
    public float MinimumY = -90F;
    public float MaximumY = 90F;
    private float _rotationY = 0f;


    void Start()
    {
        blowback = false;
        backwards = true;
        blowbackCounter = 0;
        blowbackLimit = 5;

        //Getting Components
        _rigidBody = GetComponent<Rigidbody>();
       // AudioGunShoot = GetComponent<AudioSource>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        //SlowdownCode();
        //AmmoInfo();
       // LineOfAimHandler();
        PlayerMovement();
       // _stamina = StaticVariablesScript.playerStamina;
       // Debug.Log(StaticVariablesScript.playerStamina + "STAMINA");
        PlayerAndCameraRotation();
       // Shoot();
        PickUpHandler();
    }

    /*private void AmmoInfo()
    {
        AmmunationText.text = "Ammo 6-" +  _ammo;
    }*/

    private void SlowdownCode()
    {
        if (Input.GetKeyDown(KeyCode.LeftCommand)) _slowMotion = !_slowMotion;
        if (_slowMotion) Time.timeScale = 0.5f;
        else Time.timeScale = 1f;
        _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "floor" || collision.gameObject.tag == "building" )
        {
            _grounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "floor" || collision.gameObject.tag == "building")
        {
            _grounded = false;
        }
    }

    void PickUpHandler()
    {
        if (_hitInfo.collider != null && _hitInfo.collider.gameObject.tag == "magazine")
        {
            if ((_hitInfo.collider.gameObject.transform.position - gameObject.transform.position).magnitude <= 0.5f)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    _ammo += _hitInfo.collider.gameObject.GetComponent<AmmoScript>().GetAmountOfBullets();
                    Destroy(_hitInfo.collider.gameObject);
                }
            }
        }
    }

    void LineOfAimHandler() // Makes the gun always point at the end of ray (crosshair point)
    {
        _ray = new Ray(Camera.transform.position, CrosshairObject.transform.position - Camera.transform.position);
        if (Physics.Raycast(_ray, out _hitInfo,Range))
        {           
            if (_hitInfo.point != _prevRayCastPoint)
            {
                if ((_hitInfo.point- Camera.transform.position).magnitude >= 0.5f && _hitInfo.collider.gameObject.tag != "triggerBox")
                {
                    HandModel.transform.LookAt(_hitInfo.point);
                }
                _prevRayCastPoint = _hitInfo.point;
            }
            Debug.DrawLine(Camera.transform.position, _hitInfo.point,Color.blue);
        }
    }

    void PlayMuzzleFlash()
    {
        if (SmokeParticleSystem != null) SmokeParticleSystem.Play();
    }

    private IEnumerator ShotEffect()
    {
        _lineRenderer.enabled = true; // for line to show up
        //AudioGunShoot.Play();
        yield return _shotLength;
        _lineRenderer.enabled = false; // for line to disapear
    }

    void Shoot()
    {
        // Enable delaycounter (fireDelta++)
        if (Input.GetMouseButton(0) && !_countFireDelta)
        {
            _countFireDelta = true;
        }

        // Shoot a bullet if fireDelta = 0
        if (_fireDelta == 0.0f && Input.GetMouseButton(0) && _ammo > 0)
        {
            blowback = true;
            bool CreateImpactHole = true;
            _ammo -= 1;

            PlayMuzzleFlash();
            UpdateGunGlow();
            // Destroying enemies if rayHitInfo holds information about an enemy
            if (Physics.Raycast(_ray, out _hitInfo))
            {
                if (_hitInfo.collider.gameObject.tag == "enemy" || _hitInfo.collider.gameObject.tag == "boss")
                {
                    if (_hitInfo.collider.gameObject.tag == "boss")
                    {
                       // StaticVariablesScript.bossAlive = false;
                    }
                    CreateImpactHole = false;
                    Destroy(_hitInfo.collider.gameObject);
                }

                StartCoroutine(ShotEffect());
            }
            // Instantiating a bullet for visual effects
            Vector3 gunPos = GunModel.transform.position/*new Vector3(camera.position.x, camera.position.y-0.125f, camera.position.z)*/;
            Vector3 gunDirection = GunModel.transform.forward;
            Quaternion gunRotation = GunModel.transform.rotation;
            float spawnDistance = 0.055f;
            Vector3 spawnPos = gunPos + gunDirection * spawnDistance;

            _bullet = Instantiate(BulletPrefab, spawnPos, gunRotation) as GameObject;
            if (_bullet != null)
            {
                _bullet.GetComponent<BulletScript>().SetDistance((_hitInfo.point - spawnPos).magnitude);
                if (_hitInfo.collider.gameObject.tag != "triggerBox")
                {
                    _bullet.GetComponent<BulletScript>().CreateImpactPosImpactRot(CreateImpactHole, _hitInfo.point, Quaternion.LookRotation(_hitInfo.normal));
                }
                _bullet.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, _bullet.GetComponent<BulletScript>().force));
            }
        }
        if (blowback)
        {
            Vector3 stepVector = GunModel.transform.forward * 0.02f;
            if (backwards)
            {
                GunModel.transform.position -= stepVector;
                blowbackCounter += 1;
                if (blowbackCounter >= blowbackLimit)
                {
                    blowbackCounter = 0;
                    backwards = false;
                }
            }
            if (!backwards)
            {
                GunModel.transform.position += stepVector;
                blowbackCounter += 1;
                if (blowbackCounter >= blowbackLimit)
                {
                    blowbackCounter = 0;
                    backwards = true;
                    blowback = false;
                }
            }
        }
        // Count delay for next shot
        if (_countFireDelta) _fireDelta += Time.deltaTime;
        // If (FireDelay) certain time has passed since last bullet was shot, fireDelta becomes 0 and next bullet can be shot
        if (_fireDelta >= GunModel.GetComponent<GunScript>().FireDelay())
        {
            _fireDelta = 0.0f;
            _countFireDelta = false;
        }
    }

    //use it for grenade amount
    public void UpdateGunGlow()
    {
        Debug.Log("Updating color of gun");
        var gunMat = GunModel.GetComponent<Renderer>().materials;
        Color newEmission = Color.white * ((float)_ammo / (float)ammoMax);//multiply by CurrentAmmo / MaxAmmo
        gunMat[0].SetColor("_EmissionColor", newEmission);

        AmmoLight.GetComponent<Light>().intensity = 1f * ((float)_ammo / (float)ammoMax);
    }

    private void PlayerMovement()
    {
        Vector3 velocity = _rigidBody.velocity;
        if (_grounded && Input.GetKeyDown(KeyCode.Space))
        {
           
            _grounded = false;
            _rigidBody.AddRelativeForce(new Vector3(0, JumpSpeed, 0));
        }

        if (Input.GetKey(KeyCode.W))
        {
           // if (!AudioWalk.isPlaying)
          //  {
           //     AudioWalk.Play();
           // }
            _forwardSpeed = MovementSpeed;
            if ( !Input.GetKey(KeyCode.LeftShift))  
            {
                _forwardSpeed = 1;
            }   
            if (Input.GetKey(KeyCode.LeftShift))
            {
              //  if (!AudioRunBreath.isPlaying)
              //  {
              //      AudioRunBreath.Play();
               //}
                _forwardSpeed *= 2;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
           // if (!AudioWalk.isPlaying)
          //  {
            //    AudioWalk.Play();
           // }
            _forwardSpeed = -MovementSpeed;

        }
        else _forwardSpeed = 0;

        if (Input.GetKey(KeyCode.A))
        {
            //if (!AudioWalk.isPlaying)
          //  {
          //      AudioWalk.Play();
         //   }
            _sidewaysSpeed = -MovementSpeed;

        }
        else if (Input.GetKey(KeyCode.D))
        {
            //if (!AudioWalk.isPlaying)
          //  {
          //      AudioWalk.Play();
          //  }
            _sidewaysSpeed = MovementSpeed;

        }
        else _sidewaysSpeed = 0;

        Vector3 direction = new Vector3(_sidewaysSpeed, 0, _forwardSpeed);

        if (direction == Vector3.zero)
            _rigidBody.velocity = _rigidBody.velocity * SlowDownFactor;

        else
        {
            if (velocity.magnitude <= MaxMovementSpeed)
            {
                _rigidBody.AddRelativeForce(direction * accelerationValue);
            }
            if (velocity.magnitude > MaxMovementSpeed)
            {
                _rigidBody.velocity = _rigidBody.velocity * SlowDownFactor;
            }
        }
    }

    void PlayerAndCameraRotation()
    {
        if (Axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * SensitivityX;

            _rotationY += Input.GetAxis("Mouse Y") * SensitivityY;
            _rotationY = Mathf.Clamp(_rotationY, MinimumY, MaximumY);

            transform.localEulerAngles = new Vector3(0, rotationX, 0);
            Camera.localEulerAngles = new Vector3(-_rotationY, 0, 0);
        }
        else if (Axes == RotationAxes.MouseX) transform.Rotate(0, Input.GetAxis("Mouse X") * SensitivityX, 0);
    }

    public bool Grounded
    {
        get
        {
            return _grounded;
        }
       
    }
}
