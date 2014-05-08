using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour {

	public string charName;
	GameObject obj;
	// Use this for initialization
	void Start () {
		obj=GameObject.Find(charName).transform.FindChild("Girl").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position=Vector3.Lerp(transform.position,new Vector3(obj.transform.position.x,obj.transform.position.y,transform.position.z),Time.deltaTime*2);
	
	}
}
