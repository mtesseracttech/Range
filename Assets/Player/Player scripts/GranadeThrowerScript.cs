using UnityEngine;
using System.Collections;

public class GranadeThrowerScript : MonoBehaviour {
    [Range(500, 1000)]
    public int ForwardForce = 1000;
    [Range(100, 1000)]
    public int UpwardForce = 500;
    public GameObject gun;
    public GameObject prefab;
    private GameObject granade;
    private bool spawn;
	private void Start () {
        spawn = false;
	}
	
	private void FixedUpdate () {
        SpawnGranade();
	}
    private void SpawnGranade()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            spawn = true;
        }
        if (spawn && Input.GetKeyUp(KeyCode.G))
        {
            spawn = false;
            granade = Instantiate(prefab, SpawnPosition(), Quaternion.identity) as GameObject;
            Rigidbody granadeRigidBody = granade.GetComponent<Rigidbody>();
            // Ignore Collisions between player and the granade
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), granade.GetComponent<Collider>());

            granadeRigidBody.mass = 2.5f;
            // Apply force to the granade
            granadeRigidBody.AddRelativeForce(gun.transform.forward* ForwardForce + gameObject.transform.up* UpwardForce);
        }
    }
    private Vector3 SpawnPosition()
    {
        return (gun.transform.position + gun.transform.forward * 0.1f);
    }
}
