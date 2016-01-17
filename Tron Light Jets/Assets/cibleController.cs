using UnityEngine;
using System.Collections;

public class cibleController : MonoBehaviour {
    public int healthMax;
    private int health;

    // Use this for initialization
    void Start () {
        health = healthMax;
	}
	
	// Update is called once per frame
	void Update () {
        if (health < 1)
        {
            Destroy(gameObject);
        }
    }

    void damage(int damages)
    {
        health -= damages;
    }
}
