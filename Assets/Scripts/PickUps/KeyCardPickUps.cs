using UnityEngine;
using System.Collections;

public class KeyCardPickUps : MonoBehaviour
{
    //public AudioClip PickUpSound;
    public bool doorKey;

    void OnTriggerEnter(Collider theCollider)
    {
        if (theCollider.tag == "Player")
        {
            StartCoroutine(PickedUp());
           // AudioSource.PlayClipAtPoint(PickUpSound, transform.position);
            doorKey = true;
            Debug.Log(doorKey);
        }  
    }

    void OnTriggerExit(Collider theCollider)
    {
        if (theCollider.tag == "Player")
        {
            StartCoroutine(PickedUp());
            // AudioSource.PlayClipAtPoint(PickUpSound, transform.position);
            doorKey = false;
        }
    }

    IEnumerator PickedUp()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
