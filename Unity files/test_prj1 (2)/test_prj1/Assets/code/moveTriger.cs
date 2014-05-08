using UnityEngine;
using System.Collections;

public class moveTriger : MonoBehaviour {

	public string charName;
	public string objName;
	public Vector3 position;
	public float speed;

	GameObject moveObj;
	GameObject charObj;
	bool finished;
	// Use this for initialization
	void Start () {
		moveObj=GameObject.Find(objName);
		charObj=GameObject.Find(charName);
		finished=false;
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!finished&&transform.position.x<charObj.transform.position.x)
		{
			triger();
		}
	
	}

	void triger()
	{
		if(moveObj.transform.position!=position)
		{
			float distance=Vector3.Distance(moveObj.transform.position, position);
			moveObj.transform.position=Vector3.Lerp(moveObj.transform.position,position,Time.deltaTime* speed/distance);
		}
		else finished=true;
	}
}
