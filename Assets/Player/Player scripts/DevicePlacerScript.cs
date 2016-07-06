using UnityEngine;
using System.Collections;

public class DevicePlacerScript : MonoBehaviour
{
    /*public GameObject sharedAI;
    private SharedEnemyAI sharedAIScript;*/
    public GameObject camera;
    public GameObject devicePrefab;
    private GameObject _device;
    //private BoxCollider _deviceCollider;
    private DeviceScript _deviceScript;
    private bool _instantiated;
    private RaycastHit _hitInfo;

    // Use this for initialization
    void Start()
    {
        //sharedAIScript = sharedAI.GetComponent<SharedEnemyAI>();
        _instantiated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 spawnPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            Quaternion spawnRot = gameObject.transform.rotation;

            _device = Instantiate(devicePrefab, spawnPos/* + (gameObject.transform.forward * 2)*/, spawnRot) as GameObject;
            //_deviceScript = _device.GetComponent<DeviceScript>();
            //_deviceCollider = _device.GetComponent<BoxCollider>();
       
            _instantiated = true;
        }
        if (_instantiated && Input.GetKey(KeyCode.E))
        {
            _device.transform.position = (new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z) + (gameObject.transform.forward * 2));
            _device.transform.rotation = gameObject.transform.rotation;
            
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            _instantiated = false;
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out _hitInfo))
            {
                if (_hitInfo.collider.gameObject.layer != 4 && _hitInfo.collider.gameObject.tag != "device") // 4 = Ignore raycast
                {
                    _device.transform.position = new Vector3(_hitInfo.point.x, _hitInfo.point.y, _hitInfo.point.z);
                    _device.transform.rotation = Quaternion.LookRotation(-_hitInfo.normal);
                    //_deviceScript.sharedEnemyAIScript = sharedAIScript;
                    
                }
                else
                {
                    Destroy(_device.gameObject);
                }
            }


        }
        //if (_hitInfo.collider.gameObject.tag != "device")
        //{

        //Debug.DrawLine(camera.transform.position, _hitInfo.point, Color.cyan);
        //}
    }
}
