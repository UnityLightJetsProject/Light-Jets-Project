using UnityEngine;
using System.Collections;

public class CircuitManager : MonoBehaviour
{

	public int idCircuit;
	public int idLastCheckPoint;
	public GameObject[] checkpoints;

	// Use this for initialization
	void Start ()
	{
		idLastCheckPoint = 0;
		for (int i = 0; i < checkpoints.Length; i++) {
			checkpoints [i].GetComponent<Checkpoint> ().id = i + 1;
		}
	}

	public void setLastCheckpoint (int id)
	{
		idLastCheckPoint = id % checkpoints.Length;
	}
}
