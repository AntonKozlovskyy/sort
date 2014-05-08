using UnityEngine;
using System.Collections;

public class animationTriger : MonoBehaviour {


	public string charName;
	public string animationObject;
	public string animationName;
	GameObject obj;
	bool trigered;
	bool finished;
	string defaultAnimation;
	// Use this for initialization
	void Start () {
		obj=GameObject.Find(charName);
		trigered=false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!trigered && transform.position.x<obj.transform.position.x)
		{
			trigered=true;
			triger();
		}
	
	}

	void triger()
	{
		GameObject anim=GameObject.Find(animationObject);
		defaultAnimation=anim.animation.clip.name;
		Debug.Log(defaultAnimation);
		anim.animation.CrossFade(animationName);
		anim.animation.PlayQueued(defaultAnimation);
		finished=true;
	}
}
