using UnityEngine;
using System.Collections;

public class charHandler : MonoBehaviour {

	// Use this for initialization

	enum movementState { RunningLeft, RunningRight, Idle};
	enum jumpingState {Jumping, Falling, Landing, Climbing, Hanging, Idle,HangingIdle, JumpingUp, PreHang};
	movementState charMovementState;
	jumpingState charJumpingState;
	float charRunSpeed,charJumpSpeed;
	bool charDefaultDirection;

	float jumpingStartY;

	Vector3 hangPosition,climbEndPosition;
	Vector3 charPosBJoint;

	ConfigurableJoint charHangJoint;
	Collider[] hitColliders;

	Vector3 startPreHangPosition;

	void Start () 
	{
		charMovementState=movementState.Idle;	
		charJumpingState=jumpingState.Idle;
		charRunSpeed=2;
		charJumpSpeed=2;
		charDefaultDirection=true;
		jumpingStartY=transform.position.y;
		hangPosition = Vector3.zero;
		climbEndPosition = Vector3.zero;
		charPosBJoint = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () 
	{
		keysHandler();
		movementHandler();
		animationHandler();
		findObjectsInRange("ground",5,Color.green);
		findObjectsInRange("climber",2,Color.red);
		if(charJumpingState!=jumpingState.Idle)Debug.Log(charJumpingState);
	}

	void animationHandler()
	{
		if(charJumpingState!=jumpingState.Idle)
		{
			switch(charJumpingState)
			{
			case jumpingState.Jumping:
				if(!transform.animation.IsPlaying("jump_forward")) transform.animation.CrossFade("jump_forward");
				break;
			case jumpingState.Falling:
				if(!transform.animation.IsPlaying("fall")) transform.animation.CrossFade("fall");
				break;
			case jumpingState.Landing:
				if(!transform.animation.IsPlaying("landing")) transform.animation.CrossFade("landing");
				break;
			case jumpingState.Climbing:
				if(!transform.animation.IsPlaying("climb")) transform.animation.CrossFade("climb");
				break;
			case jumpingState.Hanging:
				if(!transform.animation.IsPlaying("hang")) 
				{
					transform.animation.CrossFade("hang");
					charJumpingState=jumpingState.HangingIdle;
				}
				break;
			case jumpingState.HangingIdle:
				if(!transform.animation.IsPlaying("hang_idle"))transform.animation.CrossFadeQueued("hang_idle");
				break;
			}
		}
		else
		{
			switch(charMovementState)
			{
			case movementState.RunningLeft:
				if(!transform.animation.IsPlaying("walk")) transform.animation.CrossFade("walk");
				break;
			case movementState.RunningRight:
				if(!transform.animation.IsPlaying("walk")) transform.animation.CrossFade("walk");
				break;
			case movementState.Idle:
				if(!transform.animation.IsPlaying("idle")) transform.animation.CrossFade("idle");
				break;
			}
		}
	}

	void keysHandler()
	{
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			charMovementState=movementState.RunningLeft;
		}
		if(Input.GetKey(KeyCode.RightArrow))
		{
			charMovementState=movementState.RunningRight;
		}
		if(Input.GetKey(KeyCode.Space)&&charJumpingState==jumpingState.Idle)
		{
			charJumpingState=jumpingState.Jumping;
			jumpingStartY=transform.position.y;
		}
		if(Input.GetKey(KeyCode.Space)&&charJumpingState==jumpingState.Hanging)
		{
			charJumpingState=jumpingState.Climbing;
		}
		if(Input.GetKeyUp(KeyCode.RightArrow))
		{
			charMovementState=movementState.Idle;
		}
		if(Input.GetKeyUp(KeyCode.LeftArrow))
		{
			charMovementState=movementState.Idle;
		}
	}

	void movementHandler()
	{
		switch(charJumpingState)
		{
		case jumpingState.Jumping:
			Debug.Log(transform.position.y-jumpingStartY);
			if(transform.rigidbody.velocity.y<6)
			{
				transform.rigidbody.AddForce(new Vector3(0,1,0)*charJumpSpeed,ForceMode.VelocityChange);

			}
			else charJumpingState=jumpingState.Falling;
			break;
		case jumpingState.Falling:
			break;
		case jumpingState.Climbing:
			charHangJoint.connectedBody=null;
			if(transform.position.z!=charPosBJoint.z)transform.position=new Vector3(transform.position.x,transform.position.y,charPosBJoint.z);
			transform.rigidbody.isKinematic=true;
			if(transform.position.y<climbEndPosition.y)
			{
				transform.Translate(new Vector3(0,1,0)*2*Time.deltaTime);
			}
			else if(transform.position.x<climbEndPosition.x)
			{
				transform.position+=new Vector3(1,0,0)*2*Time.deltaTime;
			}
			else 
			{
				transform.rigidbody.isKinematic=false;
				charJumpingState=jumpingState.Idle;
			}
			break;
		case jumpingState.PreHang:
			transform.rigidbody.isKinematic=true;
			charMovementState=movementState.Idle;
			if(transform.position!=hangPosition)
			{
				transform.position=Vector3.Lerp(transform.position,hangPosition,Time.deltaTime*10f);
			}
			else 
			{
				charHangJoint.connectedBody=transform.rigidbody;
				transform.rigidbody.isKinematic=false;
				charJumpingState=jumpingState.Hanging;
			}
			break;
		}
		//if(charJumpingState==jumpingState.Idle)
		switch(charMovementState)
		{
		case movementState.RunningLeft:
			if(charDefaultDirection)
			{
				charDefaultDirection=false;
				transform.Rotate(new Vector3(0,1,0),180);
			}
			transform.Translate(Vector3.forward * Time.deltaTime*charRunSpeed);
			break;
		case movementState.RunningRight:
			if(!charDefaultDirection)
			{
				charDefaultDirection=true;
				transform.Rotate(new Vector3(0,1,0),180);
			}
			transform.Translate(Vector3.forward * Time.deltaTime*charRunSpeed);
			break;
		}
	}

	void findObjectsInRange(string tag, int range, Color col)
	{
		hitColliders = Physics.OverlapSphere(transform.FindChild("hands").transform.position, range);
		foreach(Collider coll in hitColliders)
		{
			if(coll.gameObject.tag==tag)
			{
				Debug.DrawLine(transform.FindChild("hands").transform.position, coll.gameObject.transform.position, col);
			}
			if(coll.gameObject.tag=="climber"&&Vector3.Distance(transform.FindChild("hands").transform.position, coll.transform.position)<0.5&&charJumpingState!=jumpingState.Hanging&&charJumpingState!=jumpingState.Climbing&&charJumpingState!=jumpingState.PreHang)
			{
				//charPosBJoint=transform.position;
				//charMovementState=movementState.Idle;
				//charJumpingState=jumpingState.Hanging;
				//hangPosition=transform.position;
				//climbEndPosition=hangPosition+new Vector3(0.56f,0.77f,0);
				charHangJoint=coll.GetComponent<ConfigurableJoint>();
				//charHangJoint.connectedBody=transform.rigidbody;*/
				hangPosition=new Vector3(coll.gameObject.transform.position.x-0.1f,coll.gameObject.transform.position.y-0.75f,coll.gameObject.transform.position.z);//+new Vector3(-0.09f,1.05f,0);
				//transform.GetComponent<CapsuleCollider>().enabled=false;
				charJumpingState=jumpingState.PreHang;
			}
		}
	}

	void OnCollisionEnter(Collision coll)
	{
		if(coll.gameObject.tag=="ground"&&charJumpingState==jumpingState.Falling)
		{
			charJumpingState=jumpingState.Idle;
		}
	}

	void OnCollisionExit(Collision coll)
	{
		if(coll.gameObject.tag=="ground"&&charJumpingState!=jumpingState.Jumping&&charJumpingState!=jumpingState.Climbing)
		{
			charJumpingState=jumpingState.Falling;
		}
	}
	
}






