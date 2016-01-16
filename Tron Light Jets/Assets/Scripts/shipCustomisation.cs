using UnityEngine;
using System.Collections;

public class shipOptimisation : MonoBehaviour {
	// Values range from 0 to 100. The ship will keep it's base values if the
	// values of this script are equal to 50. The ship will be 10 to 20% better
	// if values at 100, and worst if values at 0.
	
	private int shieldCapacity;
	private int shieldReloading;
	public GameObject shieldCapacityCursor;
	public GameObject shieldReloadingCursor;


	private int weapon1FiringSpeed;
	private int weapon1EnergyEconomy;
	private int weapon1Damages;
	public GameObject weapon1FiringSpeedCursor;
	public GameObject weapon1EnergyEconomyCursor;
	public GameObject weapon1DamagesCursor;

	private int weapon2FiringSpeed;
	private int weapon2EnergyEconomy;
	private int weapon2Damages;
	public GameObject weapon2FiringSpeedCursor;
	public GameObject weapon2EnergyEconomyCursor;
	public GameObject weapon2DamagesCursor;

	private int thrusterPower;
	private int thrusterAcceleration;
	private int thrusterControl;
	public GameObject thrusterPowerCursor;
	public GameObject thrusterAccelerationCursor;
	public GameObject thrusterControlCursor;

	private int boostEnergyEconomy;
	private int boostMainPower;
	private int boostControlPower;
	public GameObject boostEnergyEconomyCursor;
	public GameObject boostMainPowerCursor;
	public GameObject boostControlPowerCursor;

	private int shieldPointReserve;
	private int weapon1PointReserve;
	private int weapon2PointReserve;
	private int thrusterPointReserve;
	private int boostPointReserve;

	public int shieldReserveInit;
	public int weapon1ReserveInit;
	public int weapon2ReserveInit;
	public int thrusterReserveInit;
	public int boostReserveInit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (shieldCapacity != getShieldCapacityCursorValue ()) {
			setShieldCapacity (getShieldCapacityCursorValue ());
			setShieldPointReserve ();
		}
		if (shieldReloading =! getShieldReloadingCursorValue()) {
			setShieldReloading (getShieldReloadingCursorValue);
			setShieldPointReserve ();
		}


		if ( =! getValue()) {
			;
			setWeapon1PointReserve ();
		}
		if ( =! getValue()) {
			;
			setWeapon1PointReserve ();
		}
		if ( =! getValue()) {
			;
			setWeapon1PointReserve ();
		}
		if ( =! getValue()) {
			;
			setWeapon2PointReserve ();
		}
		if ( =! getValue()) {
			;
			setWeapon2PointReserve ();
		}
		if ( =! getValue()) {
			;
			setWeapon2PointReserve ();
		}
		if ( =! getValue()) {
			;
			;
		}
		if ( =! getValue()) {
			;
			;
		}
		*/
	}

	void movingCursor() {
		
	}

	void setShieldCapacity (int selected) {
		shieldCapacity = selected;
	}

	void setShieldReloading (int selected) {
		shieldReloading = selected;
	}
	
	void setWeapon1FiringSpeed (int selected) {
		weapon1FiringSpeed = selected;
	}
	
	void setWeapon1EnergyEconomy (int selected) {
		weapon1EnergyEconomy = selected;
	}
	
	void setWeapon1Damages (int selected) {
		weapon1Damages = selected;
	}
	
	void setWeapon2FiringSpeed (int selected) {
		weapon2FiringSpeed = selected;
	}
	
	void setWeapon2EnergyEconomy (int selected) {
		weapon2EnergyEconomy = selected;
	}
	
	void setWeapon2Damages (int selected) {
		weapon2Damages = selected;
	}
	
	void setThrusterPower (int selected) {
		thrusterPower = selected;
	}
	
	void setThrusterAcceleration (int selected) {
		thrusterAcceleration = selected;
	}
	
	void setThrusterControl (int selected) {
		thrusterControl = selected;
	}
	
	void setBoostEnergyEconomy (int selected) {
		boostEnergyEconomy = selected;
	}
	
	void setBoostMainPower (int selected) {
		boostMainPower = selected;
	}
	
	void setBoostControlPower (int selected) {
		boostControlPower = selected;
	}
	
	void setShieldPointReserve (int selected) {
		shieldPointReserve = shieldCapacity - shieldReloading;
	}
	
	void setWeapon1PointReserve (int selected) {
		weapon1PointReserve = weapon1FiringSpeed - weapon1EnergyEconomy - weapon1Damages;
	}
	
	void setWeapon2PointReserve () {
		weapon2PointReserve = weapon2FiringSpeed - weapon2EnergyEconomy - weapon2Damages;
	}
	
	void setThrusterPointReserve () {
		thrusterPointReserve = thrusterAcceleration - thrusterControl - thrusterPower;
	}

	void setBoostPointReserve () {
		boostPointReserve = boostReserveInit - boostControlPower - boostEnergyEconomy - boostMainPower;
	}

}
