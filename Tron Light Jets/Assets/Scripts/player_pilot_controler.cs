using UnityEngine;
using System.Collections;

public class player_pilot_controler : MonoBehaviour {

	public float speed;
	public float enginePower;
	public float acceleration;
	public float rotateAmount = 0.25f;
	public float v, h;
	public GameObject shot;
	public Transform shotSpawnRightRight;
	public Transform shotSpawnRightLeft;
	public Transform shotSpawnLeftRight;
	public Transform shotSpawnLeftLeft;
	public float fireRate;
	private float nextFire;
	// true = right, false = left
	private bool shotRightOrLeft = true;

	public GameObject gunFlare;

	public float rotationMouseSpeed = 0.04f; 
	public float forwSpeed = 0.18f; // max normal speed
	public float curForwSpeed = 0.0f;
	public float curForwAcc; // current acceleration FORWARD
	public float backSpeed = 0.04f; 
	public float curBackSpeed = 0.0f;
	public float curBackAcc; // current acceleration BACKWARD
	public float accSpeed = 0.0005f;
	public float inertiaFactor = 0.955f;

	public float speedMaxFactor = 1.0f;
	public Vector3 mouseMovement;
	
	private bool SystemStatus = true;

	// Use this for initialization
	void Start () 
	{
		// Screen.lockCursor = true;
	}

	void StabilisingAgent()
	{
		GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
		GetComponent<Rigidbody>().angularDrag = 1.2f;
	}

	// Update is called once per frame
	void Update()
	{
		fireButton ();
		systemActivation ();
		moveForward ();
		moveBackward ();
		boostActivation ();
		enginesOffline ();
	}
	
	// FixedUpdate is called every 0.02 second
	void FixedUpdate() 
	{
		shipRotation ();
		movement ();
	} 

	void fireButton()
	{
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			if (shotRightOrLeft == true) {
				Instantiate (shot, shotSpawnRightRight.position, shotSpawnRightRight.rotation);
				Instantiate (shot, shotSpawnLeftRight.position, shotSpawnLeftRight.rotation);
				
				Instantiate (gunFlare, shotSpawnRightRight.position, shotSpawnRightRight.rotation);
				Instantiate (gunFlare, shotSpawnLeftRight.position, shotSpawnLeftRight.rotation);
			}
			if (shotRightOrLeft == false) {
				Instantiate (shot, shotSpawnRightLeft.position, shotSpawnRightLeft.rotation);
				Instantiate (shot, shotSpawnLeftLeft.position, shotSpawnLeftLeft.rotation);

				Instantiate (gunFlare, shotSpawnLeftRight.position, shotSpawnLeftLeft.rotation);
				Instantiate (gunFlare, shotSpawnLeftLeft.position, shotSpawnLeftLeft.rotation);
			}
			if (shotRightOrLeft == true) {
				shotRightOrLeft = false;
			} else {
				shotRightOrLeft = true;
			}
			//	GetComponent<AudioSource>().Play ();
		}
	}

	void moveForward()
	{
		if (SystemStatus) 
		{ 
			// forward  
			if (Input.GetKey ("w") /*&& engineSound.audio.pitch <= engineSoundMaxPitch*/) {
				curForwAcc += accSpeed;
				//  engineSound.audio.pitch += enginePitchfactor;
				//  BroadcastMessage("JetLightsOn"); // jet lights controller - script: player_jets_light
				StabilisingAgent ();
			}
			
			// no movement? simulate inertia && engine idle
			//else if (engineSound.audio.pitch >= engineSoundMinPitch){
			//          engineSound.audio.pitch -= enginePitchfactor }
		}	
		else
		{
			curForwSpeed *= inertiaFactor;
			//  BroadcastMessage("JetLightsOff");// jet lights controller - script: player_jets_light
		}
	}

	void moveBackward()
	{
		if (curForwSpeed <= 0.001)
		{
			curForwSpeed = 0.0f;
		}
		
		// -----------------------------------------
		// backward 
		if (Input.GetKey ("s"))
		{
			curBackAcc += accSpeed;
			StabilisingAgent ();
		}
		else
		{ // no movement? simulate inertia
			curBackSpeed *= inertiaFactor;
		}
	}

	void boostActivation()
	{
		//BoosterSystemStatus 
		//if(player_EnerCal.BoostRechargeSwitch){ // if booster > 25% booster activated
		
		if (Input.GetKey (KeyCode.LeftShift) && Input.GetKey ("w")) {
			if (speedMaxFactor <= 1.5) {
				speedMaxFactor += 0.03f; // making it faster
			}
			//  player_EnerCal.SendMessage("DischargeBoost");
		} else if (speedMaxFactor >= 1.0) {
			speedMaxFactor -= 0.03f; // no speed factor, so to speak
		}
	}

	void systemActivation()
	{
		// toggles ship systems on/off
		if(Input.GetKeyDown("escape"))
		{			
			SystemStatus=!SystemStatus;			
			//toggles turretController script: player_turController
			//BroadcastMessage ("TurretActivator");			
		}
	}

	void enginesOffline()
	{
		////////////// ENGINES OFFLINE ///////////////////
		// all systems deactivated  
		if(!SystemStatus)
		{
			curForwSpeed *= inertiaFactor;
			//BroadcastMessage("JetLightsOff"); // jet lights controller - script: player_jets_light
			if (curForwSpeed <= 0.001)
			{
				curForwSpeed = 0.0f;
			}
		}
	}

	void shipRotation()
	{
		if (SystemStatus) {
			if (Input.GetKey ("d")) { 
				transform.Rotate (0f, 0f, -2.0f);
				StabilisingAgent ();
			}
			// -----------------------------------------
			if (Input.GetKey ("a")) {
				transform.Rotate (0f, 0f, 2.0f);
				StabilisingAgent ();
			}
			// for rotation
			mouseMovement = (Input.mousePosition - (new Vector3 (Screen.width, Screen.height, 0f) / 2.0f)) * 0.1f;
			if (mouseMovement.sqrMagnitude < 1.0) {
				mouseMovement = new Vector3 (0f, 0f, 0f); 
			} else {
				mouseMovement -= mouseMovement.normalized * 2.0f;
			}
			
			// rotation
			transform.Rotate (new Vector3 (-mouseMovement.y, mouseMovement.x, 0f) * rotationMouseSpeed);
		}
	}

	void movement()
	{
		////////////////////////////////////////////////////
		// forward movement
		Vector3 movementVector = Vector3.forward * curForwSpeed - Vector3.forward * curBackSpeed;
		transform.Translate (movementVector);
		curForwAcc *= inertiaFactor;
		curBackAcc *= inertiaFactor;
		// add speed to the speed!
		curForwSpeed += curForwAcc; 
		curForwSpeed = Mathf.Max (0.0f, Mathf.Min (forwSpeed * speedMaxFactor, curForwSpeed));
		curBackSpeed += curBackAcc; 
		curBackSpeed = Mathf.Max (0.0f, Mathf.Min (backSpeed * speedMaxFactor, curBackSpeed));
	}
}
