using UnityEngine;
using System.Collections;

public class playerControlerV2 : MonoBehaviour {

	private float speedSelector; // throtle speed selector, from 0 (full stop) to 100 (full speed)
	public float speedSelectorFactor; // set the speed at which the selector go up or down 
	
	private float speedX; // current speed along X axis
	private float speedY; // current speed along Y axis
	private float speedZ; // current speed along Z axis	
	private float RotationX; // current rotation speed around X axis
	private float RotationY;
	private float RotationZ;
	private float thrust; // current thrust power
	private float selectedThrust; // speedSelector * thrustMax / 100
	private int energy; // energy left
    private int health; // health left

	private bool boostActivated;

	// ship specs : 
	private float mass; // ship mass
	public float sizeCoeff; // coefficient used to simulate the surface on which air drag applies
	public int energyMax; // maximum of energy the ship can store
    public int healthMax;
	public int energyRefillSpeed; // multiply per 50 to have the amount of energy refilled per second
	public int boostEnergyConsumption; // multiply per 50 to have the amount of energy used by boost each second
	public int weaponEnergyConsumption; // energy used for each shot
	public float stabilizatorThrust; // multiply the drag effect along X and Y axis. the higher this factor, the less the ship will drift
	public float stabilizatorThrustBoost; // bonus thrust granted by boost
	public float thrustMax; // maximum engines thrust power
	public float thrustBoost; // bonus thrust granted by boost
	public float thrustFactor; // thrust increment each update
	public float brakeFactor; // how much brake is efficient
	public float pitchSpeed; // speed at which the ship can pitch
	public float yawSpeed; // speed at which the ship can yaw
	public float rollSpeed; // speed at which the ship can roll
	public float rollStabilizator;

	private float airVisquosity = 1.2754f;

	//
	public GameObject shotfire1;
	public Transform[] shotSpawnFire1;
	public float fireRate1;
	private float nextFire1;
	public GameObject shotfire2;
	public Transform[] shotSpawnFire2;
	public float fireRate2;
	private float nextFire2;
	
	public GUIText debugText; // on sreen texts
	public Object horizonUI; // artificial horizon

	// Use this for initialization
	void Start () {
		speedX = 0;
		speedY = 0;
		speedZ = 0;
		speedSelector = 0;
		selectedThrust = 0;
		energy = energyMax;
        health = healthMax;
		mass = GetComponent<Rigidbody> ().mass;
		if (mass == 0)
			mass = 1000;
	}

	void Update() {
		UIUpdate ();

        if (health < 1)
        {
            Destroy(gameObject);
        }
    }

	void FixedUpdate () {
		speedSelecting ();
		thrustUpdate (); 
		brake ();
		movement (); 
		pitch ();
		rollLeft ();
		rollRight ();
		yaw ();
		refillEnergy ();
		fireButton ();
		finalisation ();
	}

	// select thrust in % of maximum thrust
	void speedSelecting() {
		if (Input.GetButton("SetSpeed"))
			setSpeedSelector(speedSelector + (speedSelectorFactor * Input.GetAxisRaw("SetSpeed")));

		if (speedSelector > 100)
			speedSelector = 100;

		if (speedSelector < 0)
			speedSelector = 0;

		selectedThrust = (speedSelector * thrustMax) / 100;
	}

	void pitch() {
        GetComponent<Transform>().Rotate(Vector3.right * pitchSpeed * Input.GetAxis("Pitch"));
	}
	
	void yaw() {
        GetComponent<Transform>().Rotate(Vector3.up * yawSpeed * Input.GetAxis("Yaw"));
	}

	void rollLeft() {
        GetComponent<Transform>().Rotate (Vector3.forward * rollSpeed * Input.GetAxis ("RollLeft"));
	}
	
	void rollRight() {
        GetComponent<Transform>().Rotate(Vector3.forward * -rollSpeed * Input.GetAxis("RollRight"));
	}

	// update thrust power, thrust goes up in inferior to selected thrust, else it goes down
	void thrustUpdate (){
		if (thrust < selectedThrust && thrust < thrustMax) {
			thrust += thrustFactor;
		}

		if (thrust > thrustMax) {
			thrust = thrustMax;
		}

		if (thrust > selectedThrust && thrust > 0) {
			thrust -= thrustFactor;
		}
		
		if (thrust < 0) {
			thrust = 0;
		}

		isBoostActivated ();
		if (boostActivated) {
			thrust += thrustBoost;
			stabilizatorThrust += stabilizatorThrustBoost;
		}
	}

	// apply thrust an drag forces on the ship
	void movement() {
		if (speedZ >= 0) {
			GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (0, 0, thrust + dragFormula(speedZ)));
		}
		if (speedZ < 0) {
			GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (0, 0, thrust - dragFormula(speedZ)));
		}
		
		if (speedY > 0) {
			GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (0, -stabilizatorThrust + dragFormula(speedY), 0));
			setSpeedX(GetComponent<Transform> ().InverseTransformDirection(GetComponent<Rigidbody> ().velocity).x);
			if (speedX < 0)
				speedX = 0;
		}
		if (speedY < 0) {
			GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (0, stabilizatorThrust - dragFormula(speedY), 0));
			setSpeedX(GetComponent<Transform> ().InverseTransformDirection(GetComponent<Rigidbody> ().velocity).x);
			if (speedX > 0)
				speedX = 0;
		}
		
		if (speedX > 0) {
			GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (-stabilizatorThrust + dragFormula(speedX), 0, 0));
			setSpeedX(GetComponent<Transform> ().InverseTransformDirection(GetComponent<Rigidbody> ().velocity).x);
			if (speedX < 0)
				speedX = 0;
		}
		if (speedX < 0) {
			GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (stabilizatorThrust - dragFormula(speedX), 0, 0));
			setSpeedX(GetComponent<Transform> ().InverseTransformDirection(GetComponent<Rigidbody> ().velocity).x);
			if (speedX > 0)
				speedX = 0;
		}
	}

	void stabilization() {
		setRotationX(GetComponent<Transform> ().rotation.x);
		setRotationY(GetComponent<Transform> ().rotation.y);
		setRotationZ(GetComponent<Transform> ().rotation.z);

		if (RotationX < 0 && Input.GetAxis("RollRight") == 0) {
			transform.Rotate(Vector3.up * -rollStabilizator);
		}

		if (RotationX > 0 && Input.GetAxis("RollLeft") == 0) {
			transform.Rotate(Vector3.up * rollStabilizator);			
		}
		
		if (RotationY < 0 && Input.GetAxis("Pitch") == 0) {
			transform.Rotate(Vector3.right * -rollStabilizator);
		}
		
		if (RotationY > 0 && Input.GetAxis("Pitch") == 0) {
			transform.Rotate(Vector3.right * rollStabilizator);			
		}
		
		if (RotationZ < 0 && Input.GetAxis("Yaw") == 0) {
			transform.Rotate(Vector3.forward * -rollStabilizator);
		}
		
		if (RotationZ > 0 && Input.GetAxis("Yaw") == 0) {
			transform.Rotate(Vector3.forward * rollStabilizator);			
		}
	}
	
	// apply forces toward the ship on every axis, stabilizing it and slowing it down
	void brake() {
		if (Input.GetButton ("Brake")) {
			if (speedZ >= 0) {
				GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (0, 0, dragFormula(speedZ) * brakeFactor));
			}
			if (speedZ < 0) {
				GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (0, 0, -dragFormula(speedZ) * brakeFactor));
			}
			
			if (speedY > 0) {
				GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (0, dragFormula(speedY) * brakeFactor, 0));
			}
			if (speedY < 0) {
				GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (0, -dragFormula(speedY) * brakeFactor, 0));
			}
			
			if (speedX > 0) {
				GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (dragFormula(speedX) * brakeFactor, 0, 0));
			}
			if (speedX < 0) {
				GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (-dragFormula(speedX) * brakeFactor, 0, 0));
			}
		}
	}

	void UIUpdate() {
		debugText.text = "speedSelector = " + (int)speedSelector + "\n"
				+ "selectedThrust = " + (int)(selectedThrust) + "\n" 
				+ "thrust = " + (int)(thrust) + "\n"  
				+ "speedX = " + GetComponent<Transform> ().InverseTransformDirection(GetComponent<Rigidbody> ().velocity).x + "\n"
				+ "speedY = " + GetComponent<Transform> ().InverseTransformDirection(GetComponent<Rigidbody> ().velocity).y + "\n"
				+ "speedZ = " + GetComponent<Transform> ().InverseTransformDirection(GetComponent<Rigidbody> ().velocity).z + "\n"
				+ "rotationX = " + GetComponent<Transform> ().rotation.x + "\n"
				+ "rotationY = " + GetComponent<Transform> ().rotation.y + "\n"
				+ "rotationZ = " + GetComponent<Transform> ().rotation.z + "\n"
				+ "energy = " + energy + "\n"
				+ "Input.Pitch = " + Input.GetAxisRaw("Pitch") + "\n"
				+ "Input.Yaw = " + Input.GetAxisRaw("Yaw") + "\n"
				+ "Input.RollLeft = " + Input.GetAxisRaw("RollLeft") + "\n"
				+ "Input.RollRight = " + Input.GetAxisRaw("RollRight") + "\n";

	//	GameObject.Find("Horizon").transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
	//	GameObject.Find ("Horizon").transform.eulerAngles = new Vector3 (GetComponent<Transform>().eulerAngles.x, 0, 0); // (Vector3.up * 0);
	//	GameObject.Find ("Horizon").transform.eulerAngles = new Vector3 (0, GetComponent<Transform>().eulerAngles.y, 0); // (Vector3.right * 0);
	}
	
	void refillEnergy() {
		if (energy < energyMax) {
			setEnergy(energy + energyRefillSpeed);
			if (energy > energyMax) {
				setEnergy(energyMax);
			}
		}
	}

	void fireButton() {
		if (Input.GetButton ("Fire1") && Time.time > nextFire1) {
			nextFire1 = Time.time + fireRate1;
			foreach(Transform shotspawn in shotSpawnFire1)
				Instantiate (shotfire1, shotspawn.position, shotspawn.rotation);
		}

		if (Input.GetButton ("Fire2") && Time.time > nextFire2) {
			nextFire2 = Time.time + fireRate2;
			foreach(Transform shotspawn in shotSpawnFire2)
				Instantiate (shotfire2, shotspawn.position, shotspawn.rotation);
		}		
		//	GetComponent<AudioSource>().Play ();
	}

	void isBoostActivated() {
		if (Input.GetButton("Boost") && energy >= boostEnergyConsumption)
		    boostActivated = true;
	}

	void finalisation() {
		if (boostActivated) {
			thrust -= thrustBoost;
			stabilizatorThrust -= stabilizatorThrustBoost;
			boostActivated = false;
		}	

		setSpeedX(GetComponent<Transform> ().InverseTransformDirection(GetComponent<Rigidbody> ().velocity).x);
		setSpeedY(GetComponent<Transform> ().InverseTransformDirection(GetComponent<Rigidbody> ().velocity).y);
		setSpeedZ(GetComponent<Transform> ().InverseTransformDirection(GetComponent<Rigidbody> ().velocity).z);
	}

	// drag is the ait resistance to movement.
	float dragFormula(float speed) {
		return (-(speed * speed * airVisquosity) / 2) * sizeCoeff;
	}

    void setSpeedSelector (float newSpeedSelector){
		speedSelector = newSpeedSelector;
	}
	
	void setSpeedX(float newSpeed) {
		speedX = newSpeed;
	}
	
	void setSpeedY(float newSpeed) {
		speedY = newSpeed;
	}
	
	void setSpeedZ(float newSpeed) {
		speedZ = newSpeed;
	}
	
	void setRotationX(float newRotation) {
		RotationX = newRotation;
	}
	
	void setRotationY(float newRotation) {
		RotationY = newRotation;
	}
	
	void setRotationZ(float newRotation) {
		RotationZ = newRotation;
	}
	
	void setEnergy(int newEnergy) {
		energy = newEnergy;
	}

    void damage(int damages)
    {
        health -= damages;
    }
}