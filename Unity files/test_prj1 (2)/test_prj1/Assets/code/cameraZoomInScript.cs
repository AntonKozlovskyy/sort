using UnityEngine;
using System.Collections;

public class cameraZoomInScript : MonoBehaviour {

	public string charName;
	public string camName;

	int zoom;
	float smooth;
	bool isZoomed;
	GameObject obj;
	bool finished;
	// Use this for initialization
	void Start () {
		obj=GameObject.Find(charName);
		zoom=1;
		smooth=5;
		isZoomed=false;
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
		cam.camera.orthographicSize = Mathf.Lerp(cam.camera.orthographicSize,zoom,Time.deltaTime*smooth);
		if((int)cam.camera.orthographicSize==(int)zoom) 
		{
			finished=true;
		}
	}
}