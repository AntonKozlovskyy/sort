using UnityEngine;
using System.Collections;

public class transpTriger : MonoBehaviour {


	public string charName;
	public string objName;
	public float startTransp;
	public float endTransp;
	public float transpIt;

	bool finished;
	GameObject obj;
	// Use this for initialization
	void Start () {
		obj=GameObject.Find(charName);
		finished=false;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(!finished&&transform.position.x<=obj.transform.position.x)
		{
			triger();
		}
	}

	void triger()
	{
		GameObject transp=GameObject.Find(objName);
		Color col=transp.transform.renderer.material.color;
		col.a+=transpIt*Time.deltaTime;
		if(transp.transform.renderer.material.color.a>=endTransp)
		{
			finished=true;
		}
	}
}
