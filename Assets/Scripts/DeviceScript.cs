using UnityEngine;
using System.Collections;

public class DeviceScript : MonoBehaviour
{
    private SharedEnemyAI _sharedEnemyAIScript;
    private bool _canBeSpawned;
    private bool _active;
    // Use this for initialization
    void Awake()
    {
    }
    void Start()
    {
        _canBeSpawned = true;
        _active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!_active)
            {
                _sharedEnemyAIScript.SearchInRangeVec3(gameObject.transform.position, 20, true);
                _active = true;
            }
        }
    }
    /**
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "device")
        {
            Destroy(gameObject);
        }
    }
    /**/
    /*void OnTriggerLeave(Collider collider)
    {
        if (collider.gameObject.tag == "device")
        {

            //Destroy(gameObject);
            //_canBeSpawned = true;
        }
    }*/
    public bool Active
    {
        get { return _active; }
        set { _active = value; }
    }
    public SharedEnemyAI sharedEnemyAIScript
    {
        get { return _sharedEnemyAIScript; }
        set { _sharedEnemyAIScript = value; }
    }
}