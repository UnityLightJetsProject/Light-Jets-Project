using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
	public int id;
	private GameObject circuit;


	void Start ()
	{
		circuit = GameObject.FindGameObjectWithTag ("Circuit");
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag ("Player")) {
			if (verifyCheckpointOrder ()) {
				goodCheck ();
			} else {
				badCheck ();
			}
		}
	}

	bool verifyCheckpointOrder ()
	{
		if (circuit.GetComponent<CircuitManager> ().idLastCheckPoint == id - 1)
			return true;
		return false;
	}

	void goodCheck ()
	{
		print ("ok");
		circuit.GetComponent<CircuitManager> ().setLastCheckpoint (id);
	}

	void badCheck ()
	{
		print ("pas le bon");
	}
}

