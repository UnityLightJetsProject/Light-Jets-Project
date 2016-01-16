using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
	public GameObject[] checkpoints;
	private GameController gameController;
	
	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}
/*
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag != "Player")
		{
			return;
		}
		if (other.tag == "Player")
		{
			if(verifyCheckpointOrder(other.name, checkpointNumber))
				gameController.setPlayerCheckpoint (player, checkpointNumber);
			else
				wrongCheckpoint();
		}
	}

	bool verifyCheckpointOrder(){
		return;
	}
*/
	// Update is called once per frame
	void Update () {

	}
}

