using UnityEngine;
using System.Collections;

public class env : MonoBehaviour {

	// Use this for initialization

	Vector3 stuff;
	Collider[] hitColliders;
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 cohesion = Vector3.zero;
		hitColliders = Physics.OverlapSphere(transform.position, 2);
		foreach(Collider coll in hitColliders)
		{
			if(coll.gameObject.tag=="climber")
			{
				Debug.DrawLine(transform.position, coll.gameObject.transform.position, Color.magenta);
				Debug.Log("found climber");
			}
		}
	}

	void OnCollisionEnter(Collision coll)
	{
	}
	
	void OnCollisionExit(Collision coll)
	{
	}
}
