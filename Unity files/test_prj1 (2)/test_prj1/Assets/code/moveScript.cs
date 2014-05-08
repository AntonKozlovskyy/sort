using UnityEngine;
using System.Collections;

public class moveScript : MonoBehaviour {

	float speed;
	float rotateSpeed;
	bool jumping;
	int jumpingState; //0-up 1-down
	int rotation;
	float char_speed;
	// Use this for initialization
	void Start () {
		speed=10.0f;
		rotateSpeed=3.0f;
		jumping=false;
		jumpingState=1;
		rotation=0;
		char_speed=2;
	}
	
	// Update is called once per frame
	void Update () 
	{
		moveHandler();
	}

	void moveHandler()
	{
		if(Input.GetKey(KeyCode.RightArrow))
		{
			if(rotation==180) 
			{
				transform.Rotate(new Vector3(0,1,0),180);
				rotation=0;
			}
			//transform.rigidbody.AddForce(new Vector3(1,0,0)*speed);
			transform.Translate(Vector3.forward * Time.deltaTime*char_speed);

			if(!transform.animation.IsPlaying("walk")&&!jumping)transform.animation.CrossFade("walk");
		}
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			if(rotation==0) 
			{
				transform.Rotate(new Vector3(0,1,0),180);
				rotation=180;
			}
			transform.Translate(Vector3.forward * Time.deltaTime*char_speed);

			if(!transform.animation.IsPlaying("walk")&&!jumping)transform.animation.CrossFade("walk");
		}
		if(Input.GetKey(KeyCode.Space))
		{
			if(!jumping) 
			{
				jumping=true;
				jumpingState=0;
			}
			if(transform.rigidbody.velocity.y<6 && jumpingState==0)
			{
				jumping=true;
				transform.rigidbody.AddForce(new Vector3(0,1,0)*speed);
				if(!transform.animation.IsPlaying("jump_forward"))transform.animation.CrossFade("jump_forward");
			}
			else 
			{
				jumpingState=1;
				transform.animation.CrossFade("fall");
			}
		}

		if((Input.GetKeyUp(KeyCode.RightArrow)||Input.GetKeyUp(KeyCode.LeftArrow))&&!jumping)
		{
			//Debug.Log("Key Up");
			if(rotation==180) 
			{
				transform.Rotate(new Vector3(0,1,0),180);
				rotation=0;
			}
			transform.animation.CrossFade("walk_to_idle");
		}
		if(jumping&&jumpingState==0)
		{
			if(!transform.animation.IsPlaying("fall"))	transform.animation.CrossFade("fall");
		}

	}

	void OnCollisionEnter(Collision coll)
	{
		if(coll.collider.gameObject.tag=="ground") 
		{
			jumping=false;
			transform.animation.CrossFade("idle");
		}
		//Debug.Log(coll.transform.name);
	}

	void OnCollisionExit(Collision coll)
	{
		if(coll.collider.gameObject.tag=="ground") 
		{
			jumping=true;
			jumpingState=0;
		}
	}
}
