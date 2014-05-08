using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class rope_test : MonoBehaviour {

	// Use this for initialization
	public int segCount;
	public Vector3 minSegScale;

	List<GameObject> ropeObject=new List<GameObject>();

	void Start () 
	{
		if(!this.GetComponent<FixedJoint>())
		{
			this.gameObject.AddComponent<FixedJoint>();
		}
		ropeObject.Add(GameObject.CreatePrimitive(PrimitiveType.Capsule));
		ropeObject[0].GetComponent<CapsuleCollider>().enabled=false;
		//ropeObject[0].rigidbody.mass=0.1f;
		ropeObject[0].transform.localScale=minSegScale;
		ropeObject[0].transform.position=new Vector3(this.transform.position.x,this.transform.position.y-this.transform.localScale.y/2,this.transform.position.z);
		ropeObject[0].gameObject.AddComponent<HingeJoint>();
		ropeObject[0].GetComponent<HingeJoint>().connectedBody=this.rigidbody;
		for(int i=1;i<segCount;i++)
		{
			ropeObject.Add(GameObject.CreatePrimitive(PrimitiveType.Capsule));
			ropeObject[i].GetComponent<CapsuleCollider>().enabled=false;
			//ropeObject[i].rigidbody.mass=0.1f;
			ropeObject[i].transform.localScale=minSegScale;
			ropeObject[i].transform.position=new Vector3(ropeObject[i-1].transform.position.x,ropeObject[i-1].transform.position.y-minSegScale.y/2,ropeObject[i-1].transform.position.z);
			ropeObject[i].gameObject.AddComponent<HingeJoint>();
			ropeObject[i].gameObject.GetComponent<HingeJoint>().connectedBody=ropeObject[i-1].rigidbody;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
