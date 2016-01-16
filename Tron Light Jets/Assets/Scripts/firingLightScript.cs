using UnityEngine;
using System.Collections;

public class firingLightScript : MonoBehaviour 
{
	LineRenderer line;
	Light lightFlare;
	public float hitForce;
	
	// Use this for initialization
	void Start () 
	{
		line = gameObject.GetComponent<LineRenderer> ();
		line.enabled = false;
		lightFlare = gameObject.GetComponent<Light> ();
		lightFlare.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown("Fire1"))
		{
			StopCoroutine("FireLaser");
			StartCoroutine("FireLaser");
		}
	}
	
	IEnumerator FireLaser() 
	{
		line.enabled = true;
		lightFlare.enabled = true;
		while (Input.GetButton("Fire2")) 
		{
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;
			line.SetPosition(0, ray.origin);
			
			if(Physics.Raycast(ray, out hit, 100))
			{
				line.SetPosition (1, hit.point);
				if(hit.rigidbody)
				{
					hit.rigidbody.AddForceAtPosition(transform.forward * hitForce, hit.point);
				}
			}
			else
				line.SetPosition(1, ray.GetPoint(100));
			
			yield return null;
		}
		line.enabled = false;
		lightFlare.enabled = false;
	}
}
