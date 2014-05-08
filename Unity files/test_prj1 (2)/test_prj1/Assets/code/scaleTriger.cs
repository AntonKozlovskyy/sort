using UnityEngine;
using System.Collections;

public class scaleTriger : MonoBehaviour {


	public string charName;
	public string objName;
	public Vector3 scaleEnd;
	public Vector3 scaleStart;
	public float scaleit;
	bool finished;
	GameObject obj;
	// Use this for initialization
	void Start () {
		finished=false;
		obj=GameObject.Find(charName);
	}
	
	// Update is called once per frame
	void Update () {
		if(!finished && transform.position.x<obj.transform.position.x)
		{
			GameObject scale=GameObject.Find(objName);
			triger();
		}
	}

	void triger()
	{
		GameObject scale=GameObject.Find(objName);
		scale.transform.localScale+=scale.transform.localScale*scaleit*Time.deltaTime;
		if(scale.transform.localScale.x>=scaleEnd.x)
		{
			finished=true;
		}
	}
}
