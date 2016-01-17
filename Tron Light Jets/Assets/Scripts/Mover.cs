using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

	public float speed;
    public float destroyDelay;
    public GameObject explosion;
    public int damages;

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}

    void FixedUpdate()
    {
        Object.Destroy(gameObject, destroyDelay);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Destructible")
        {
           other.transform.parent.SendMessage("damage",damages);
           Destroy(gameObject);
        }
        // Instantiate(explosion, transform.position, transform.rotation);
        // Destroy(gameObject);
    }
}
