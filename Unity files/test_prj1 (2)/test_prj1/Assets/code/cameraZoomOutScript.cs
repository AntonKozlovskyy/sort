using UnityEngine;
using System.Collections;

public class cameraZoomOutScript : MonoBehaviour {

	public string charName;
	public string camName;

	int normal;
	float smooth;
	GameObject obj;
	bool finished;
	// Use this for initialization
	void Start () {
		obj=GameObject.Find(charName);
		normal=5;
		smooth=5;
		finished=false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!finished && transform.position.x<obj.transform.position.x)
		{
			triger();
		}
		
	}
	
	void triger()
	{
		GameObject cam=GameObject.Find(camName);
		cam.camera.orthographicSize = Mathf.Lerp(cam.camera.orthographicSize,normal,Time.deltaTime*smooth);
		if(Mathf.Floor(cam.camera.orthographicSize+0.5f)==Mathf.Floor(normal))
		{
			finished=true;
		}
	}
}
