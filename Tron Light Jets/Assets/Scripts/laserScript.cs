using UnityEngine;
using System.Collections;

public class laserScript : MonoBehaviour 
{
	LineRenderer line;
	public float hitForce;

	// Use this for initialization
	void Start () 
	{
		line = gameObject.GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		StartCoroutine("FireLaser");
	}

	IEnumerator FireLaser() 
	{
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;
		line.SetPosition(0, ray.origin);

		if(Physics.Raycast(ray, out hit, 0.1f))
		{
			line.SetPosition (1, hit.point);
			if(hit.rigidbody)
			{
				hit.rigidbody.AddForceAtPosition(transform.forward * hitForce, hit.point);
			}
		}
		else
			line.SetPosition(1, ray.GetPoint(3f));

		yield return null;
	}
}
