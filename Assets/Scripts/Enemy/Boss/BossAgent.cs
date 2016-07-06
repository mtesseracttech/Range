using System;
using System.Collections.Generic;
using UnityEngine;

public class BossAgent : MonoBehaviour
{
    [Header("General Settings")]
    public int SightConeAngle = 90;
    public int SightRange = 10;
    public float SlerpSpeed = 0.1f;
    public GameObject TargetObject;
    private Vector3 _debugLine;
    private bool _movingUp;
    private AbstractBossState _state;
    public GameObject BloodParticles;
    public GameObject ImpactParticles;
    public GameObject MuzzleFlash;
    public int OnDestroyHelpRange = 20;
    public NavigationPath PatrolPath = null;
    private readonly Dictionary<Type, AbstractBossState> _stateCache = new Dictionary<Type, AbstractBossState>();
    private readonly Vector3 _stepVector = new Vector3(0, 0.2f, 0);
    private Transform _turretPoint;
    private Vector3 _turretPointVec;
    private int _upCounter;
    private readonly int _upLimit = 50;


    [Header("Attackstate Settings")] 
    public int ShotsPerSecond = 10;
    public int AcceptableShotRangeInDeg = 20;
    public int AimSteadiness = 1;
    public int AimDistortion = 20;



    public NavMeshAgent NavAgent { get; set; }
    public Rigidbody Parent { get; private set; }
    public Rigidbody Target { get; private set; }
    public bool EnteredNewState { get; set; }
    public Vector3 LastSeenTargetPosition { get; set; }
    public bool SeesTarget { get; private set; }


    // Use this for initialization
    private void Start()
    {
        Parent = GetComponent<Rigidbody>();
        Target = TargetObject.GetComponent<Rigidbody>();
        NavAgent = GetComponent<NavMeshAgent>();

        var childObjects = GetChildrenComponents();

        GameObject _turretPointObject = childObjects.Find(child => child.name == "DroneTurretPoint");
        _turretPoint = _turretPointObject.transform;
        

        _stateCache[typeof (BossPatrolState)] = new BossPatrolState(this, Parent.rotation, PatrolPath);
        _stateCache[typeof (BossAttackState)] = new BossAttackState
            (this, 
            MuzzleFlash, 
            BloodParticles, 
            ImpactParticles,
            _turretPointObject, 
            AimDistortion, 
            AimSteadiness,
            AcceptableShotRangeInDeg, 
            ShotsPerSecond
            );
        _stateCache[typeof (BossReturnState)] = new BossReturnState(this, Parent.position, Parent.rotation);
        _stateCache[typeof (BossLookoutState)] = new BossLookoutState(this);

        SetState(typeof (BossPatrolState));
    }

    private List<GameObject> GetChildrenComponents()
    {
        var childrenObjects = new List<GameObject>();
        for (var i = 0; i < Parent.transform.childCount; i++)
        {
            if (Parent.transform.GetChild(i).name == "DroneTurretPoint")
                childrenObjects.Add(Parent.transform.GetChild(i).gameObject);
        }
        return childrenObjects;
    }

    public void SetState(Type pState)
    {
        Debug.Log("Switching state to:" + pState.FullName);
        EnteredNewState = true;
        if (NavAgent.isOnNavMesh) NavAgent.Stop();
        SetSeeTarget();
        _state = _stateCache[pState];
    }

    public AbstractBossState GetState()
    {
        return _state;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Parent != null)
        {

            DebugCode();
            //Levitate();
            SetSeeTarget();
            _state.Update();
        }
    }

    private void SetSeeTarget()
    {
        if (LookForTarget())
        {
            LastSeenTargetPosition = Target.position;
            Debug.DrawLine(Parent.position, Target.position, Color.yellow);
            SeesTarget = true;
        }
        else
        {
            SeesTarget = false;
        }
    }

    private void DebugCode()
    {
        Debug.DrawLine(Parent.transform.position, Parent.transform.position + Parent.transform.forward, Color.blue);
        if (Input.GetKeyDown("g"))
        {
            Debug.Log("Currently in state: " + _state);
        }
    }


    // Update is called once per frame
    private bool LookForTarget()
    {
        var differenceVec = Target.transform.position - _turretPoint.position;//Parent.transform.position;
        Debug.DrawLine(Target.transform.position, _turretPoint.position);
        if (differenceVec.magnitude < SightRange) //Sees if Target is even in range
        {
            var targetAngle = Vector3.Angle(differenceVec, Parent.transform.forward);

            if (targetAngle > SightConeAngle/2)
            {
                return false; //Sees if Target is in sight cone
            }
            RaycastHit hit;

            if (Physics.Raycast(_turretPoint.position, differenceVec, out hit, differenceVec.magnitude))
            {
                Debug.Log(hit.collider.gameObject.tag);
                if (hit.collider.gameObject.tag != "Player")
                    return false; //Checks if anything is between the Target and the Parent
            }
            return true;
        }
        return false;
    }

    public void CreateParticles(GameObject particles, Vector3 position)
    {
        Instantiate(particles, position, Quaternion.identity);
    }

    public void CreateParticlesRotated(GameObject particles, Vector3 position, Quaternion rotation)
    {
        Instantiate(particles, position, rotation);
    }

    private void Levitate()
    {
        if (_movingUp)
        {
            Parent.gameObject.transform.position += _stepVector;
            Debug.Log("Moving Up");
        }
        else
        {
            Debug.Log("Moving Down");
            Parent.gameObject.transform.position -= _stepVector;
        }

        _upCounter += 1;
        if (_upCounter >= _upLimit)
        {
            _movingUp = !_movingUp;
            _upCounter = 0;
        }
    }

    private void OnDestroy()
    {
    }
}