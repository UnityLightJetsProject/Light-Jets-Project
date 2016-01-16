using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	struct Player {
		int checkpointNumber;
		int playerNumber;
	}
	struct Checkpoint {
		int number;
		GameObject checkpointNumber;
	}
	public GUIText speedSelector;
	public GUIText acceleration;
	public GUIText speed;

	// we will make a cursor travel through the array, once it reaches the 
	// end, the cursor go back to the beginning.
	// if the player go through the wrong checkpoint, the cursor wont move.
/*	
    public Checkpoint[] checkpoints;
	public Player[] players;
*/
	void Start ()
	{

	}
	
	void Update ()
	{

	}

/*	bool verifyCheckpoint(Player player, Checkpoint checkPoint)
	{
		if (player.checkpointNumber + 1 == CheckPoint.number)
			return true;
		else
			return false;
	}
*/
}