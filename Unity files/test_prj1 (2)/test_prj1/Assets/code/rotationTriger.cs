using UnityEngine;
using System.Collections;

public class rotationTriger : MonoBehaviour {


	public string charName;
	public string objName;
	public string pivotName;
	public float rotDegree;
	public float rotit;
	public Vector3 rotVector;

	
	bool finished;
	GameObject obj;
	GameObject rotObj;
	GameObject rotPivot;
	float rotElapsed;
	// Use this for initialization
	void Start () 
	{
		finished=false;
		obj=GameObject.Find(charName);	
		rotObj=GameObject.Find(objName);
		rotPivot=rotObj.transform.Find(pivotName).gameObject;
		rotElapsed=0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!finished && transform.position.x<obj.transform.position.x)
		{
			triger();
		}
	}

	void triger()
	{
		//Debug.Log(rotPivot.transform.position);
		//Debug.Log(rotPivot.transform.localPosition);
		rotObj.transform.RotateAround(rotPivot.transform.position,rotVector,rotDegree*Time.deltaTime*rotit);
		rotElapsed+=rotDegree*Time.deltaTime*rotit;
		Debug.Log(rotElapsed);
		if(rotElapsed>=rotDegree) finished=true;
	}
}
