using UnityEngine;
using System.Collections;

public class charHandler2 : MonoBehaviour {

	// Use this for initialization

	enum movementState { RunningLeft, RunningRight, Idle, JumpingUp, JumpingLeft, JumpingRight, Hanging, Climbing, PreHanging, Falling, FallingLeft, FallingRight};

	movementState charMovementState;
	float charRunSpeed;
	float charJumpSpeed;
	float charMaxJumpHeight;
	bool rotatedMovement;

	bool climberInRange;
	GameObject nearestClimber;
	float maxClimberDistance;
	bool climbingMode;
	Vector3 hangPositionFix;
	Vector3 climbPositionFix;
	Vector3 fallingStartPos;
	float charMaxVelocityUp;
	float charMaxVelocityForward;
	bool fallingStart;

	bool processKeys;

	bool needLandingAnimation;

	ConfigurableJoint charHangJoint;
	Collider[] hitColliders;

	float charPosBeforeJump;

	void Start () 
	{
		charMovementState=movementState.Idle;
		charRunSpeed=3.0f;
		charJumpSpeed=1f;
		charMaxVelocityUp=2.0f;
		charMaxVelocityForward=2.0f;
		charMaxJumpHeight=2.0f;
		rotatedMovement=false;
		climberInRange=false;
		maxClimberDistance=0.4f;
		climbingMode=false;
		hangPositionFix=new Vector3(-0.19f,-0.81f,0);
		climbPositionFix=new Vector3(0.16f,0.78f,0);
		needLandingAnimation=false;
		fallingStart=false;
		fallingStartPos=Vector3.zero;
		processKeys=true;
	}
	
	// Update is called once per frame
	void Update () {
		keysHandler();
		movementHandler();
		animationHandler();
		//rayCastGround();
		findObjectsInRange("ground",4,Color.green);
		findObjectsInRange("climber",maxClimberDistance,Color.red);
		//Debug.Log(charMovementState);
	}

	void animationHandler()
	{
		switch(charMovementState)
		{
		case movementState.RunningLeft:
			if(!this.animation.IsPlaying("walk")) this.animation.CrossFade("walk");
			break;
		case movementState.RunningRight:
			if(!this.animation.IsPlaying("walk")) this.animation.CrossFade("walk");
			break;
		case movementState.Idle:
			if(needLandingAnimation&&!this.animation.IsPlaying("landing")) 
			{
				Debug.Log("need landing");
				this.animation.CrossFade("landing");
				//StartCoroutine(WaitAndCallback(this.animation["landing"].length,1));

			}
			if(!needLandingAnimation&&!this.animation.IsPlaying("idle_breath")&&!this.animation.IsPlaying("idle_shake")) 
			{
				int tmp=Random.Range(0,5);
				if(tmp==2)
				{
					this.animation.CrossFade("idle_shake");
				}
				else this.animation.CrossFade("idle_breath");
			}
			break;
		case movementState.JumpingUp:
			if(!this.animation.IsPlaying("jump_forward")) this.animation.CrossFade("jump_forward");
			break;
		case movementState.JumpingLeft:
			if(!this.animation.IsPlaying("jump_forward")) this.animation.CrossFade("jump_forward");
			break;
		case movementState.JumpingRight:
			if(!this.animation.IsPlaying("jump_forward")) this.animation.CrossFade("jump_forward");
			break;
		case movementState.Hanging:
			if(!this.animation.IsPlaying("hang_idle")) this.animation.CrossFade("hang_idle");
			break;
		case movementState.Climbing:
			if(!this.animation.IsPlaying("climb_t")) 
			{
				this.animation.CrossFade("climb_t");
				//StartCoroutine(WaitAndCallback(this.animation["climb_t"].length,0));
			}
			break;
		case movementState.PreHanging:
			if(!this.animation.IsPlaying("hang")) this.animation.CrossFade("hang");
			break;
		case movementState.Falling:
			if(!this.animation.IsPlaying("fall")) 
			{
				this.animation.CrossFade("fall");
			}
			break;
		case movementState.FallingRight:
			if(!this.animation.IsPlaying("fall")) 
			{
				this.animation.CrossFade("fall");
			}
			break;
		case movementState.FallingLeft:
			/*if(needLandingAnimation&&!this.animation.IsPlaying("landing")) 
			{
				Debug.Log("need landing");
				this.animation.CrossFade("landing");
			}
			else */if(!this.animation.IsPlaying("fall")) 
			{
				this.animation.CrossFade("fall");
			}
			break;
		}
	}

	void keysHandler()
	{
		if(processKeys)
		{
			if(Input.GetKey(KeyCode.RightArrow))
			{
				if(charMovementState==movementState.JumpingUp)
				{
					charMovementState=movementState.JumpingRight;
				}
				else if(charMovementState==movementState.Falling)
				{
					charMovementState=movementState.FallingRight;
				}
				else if(charMovementState==movementState.Idle)
				{
					charMovementState=movementState.RunningRight;
				}
				else if(charMovementState==movementState.FallingLeft)
				{
					charMovementState=movementState.FallingRight;
				}
				else if(charMovementState==movementState.Hanging)
				{
					if(rotatedMovement)
					{
						switchHangMode(false);
						charMovementState=movementState.Falling;
					}
				}
			}
			if(Input.GetKey(KeyCode.LeftArrow))
			{
				if(charMovementState==movementState.JumpingUp)
				{
					charMovementState=movementState.JumpingLeft;
				}
				else if(charMovementState==movementState.Falling)
				{
					charMovementState=movementState.FallingLeft;
				}
				else if(charMovementState==movementState.Idle)
				{
					charMovementState=movementState.RunningLeft;
				}
				else if(charMovementState==movementState.FallingRight)
				{
					charMovementState=movementState.FallingLeft;
				}
				else if(charMovementState==movementState.Hanging)
				{
					if(!rotatedMovement)
					{
						switchHangMode(false);
						charMovementState=movementState.Falling;
					}
				}
			}
			if(Input.GetKey(KeyCode.Space))
			{
				if(charMovementState==movementState.Hanging)
				{
					charMovementState=movementState.Climbing;
				}
				else if(charMovementState==movementState.RunningLeft)
				{
					charPosBeforeJump=this.transform.position.y;
					charMovementState=movementState.JumpingLeft;
				}
				else if(charMovementState==movementState.RunningRight)
				{
					charPosBeforeJump=this.transform.position.y;
					charMovementState=movementState.JumpingRight;
				}
				else if(charMovementState==movementState.Idle)
				{
					charPosBeforeJump=this.transform.position.y;
					charMovementState=movementState.JumpingUp;
				}
			}
			if(Input.GetKeyUp(KeyCode.LeftArrow))
			{
				if(charMovementState==movementState.RunningLeft)
				{
					charMovementState=movementState.Idle;
				}
				else if(charMovementState==movementState.JumpingLeft)
				{
					charMovementState=movementState.JumpingUp;
				}
		    }
			if(Input.GetKeyUp(KeyCode.RightArrow))
			{
				if(charMovementState==movementState.RunningRight)
				{
					charMovementState=movementState.Idle;
				}
				else if(charMovementState==movementState.JumpingRight)
				{
					charMovementState=movementState.JumpingUp;
				}
			}
			if(Input.GetKeyUp(KeyCode.Space))
			{
			}
		}
	}

	void movementHandler()
	{
		switch(charMovementState)
		{
		case movementState.RunningRight:
			if(rotatedMovement)
			{
				transform.Rotate(new Vector3(0,1,0),180);
				rotatedMovement=false;
			}
			transform.Translate(Vector3.forward * Time.deltaTime*charRunSpeed);
			/*if(this.rigidbody.velocity.x<charMaxVelocityForward)
			{	
				this.rigidbody.AddForce(new Vector3(1,0)*charRunSpeed);
			}*/
			break;
		case movementState.RunningLeft:
			if(!rotatedMovement)
			{
				transform.Rotate(new Vector3(0,1,0),180);
				rotatedMovement=true;
			}
			transform.Translate(Vector3.forward * Time.deltaTime*charRunSpeed);
			break;
		case movementState.JumpingUp:
			if(this.transform.position.y<=charMaxJumpHeight+charPosBeforeJump&&this.rigidbody.velocity.y<charMaxVelocityUp)
			{
				this.rigidbody.AddForce( new Vector3(0,charJumpSpeed * this.rigidbody.mass,0), ForceMode.Impulse);
			}
			else
			{
				charMovementState=movementState.Falling;
			}
			if(climberInRange) charMovementState=movementState.PreHanging;
			break;
		case movementState.JumpingRight:
			if(climberInRange&&!rotatedMovement) charMovementState=movementState.PreHanging;
			else
			{
				if(this.transform.position.y<=charMaxJumpHeight+charPosBeforeJump&&this.rigidbody.velocity.y<charMaxVelocityUp)
				{
					if(rotatedMovement)
					{
						transform.Rotate(new Vector3(0,1,0),180);
						rotatedMovement=false;
					}
					this.rigidbody.AddForce( new Vector3(0,charJumpSpeed * this.rigidbody.mass,0), ForceMode.Impulse);
					this.transform.Translate(Vector3.forward * Time.deltaTime*charRunSpeed);
				}
				else
				{
					charMovementState=movementState.FallingRight;
				}
			}
			break;
		case movementState.JumpingLeft:
			if(climberInRange&&rotatedMovement) charMovementState=movementState.PreHanging;
			else
			{
				if(this.transform.position.y<=charMaxJumpHeight+charPosBeforeJump&&this.rigidbody.velocity.y<charMaxVelocityUp)
				{
					if(!rotatedMovement)
					{
						transform.Rotate(new Vector3(0,1,0),180);
						rotatedMovement=true;
					}
					this.rigidbody.AddForce( new Vector3(0,charJumpSpeed * this.rigidbody.mass,0), ForceMode.Impulse);
					this.transform.Translate(Vector3.forward * Time.deltaTime*charRunSpeed);
				}
				else
				{
					charMovementState=movementState.FallingLeft;
				}
			}
			break;
		case movementState.FallingLeft:
			if(climberInRange&&rotatedMovement) 
			{
				charMovementState=movementState.PreHanging;
				break;
			}
			if(!rotatedMovement)
			{
				this.transform.Rotate(new Vector3(0,1,0),180);
				rotatedMovement=true;
			}
			if(!fallingStart)
			{
				fallingStart=true;
				fallingStartPos=this.transform.position;
			}
			/*float dist=rayCastGround();
			if(!needLandingAnimation&&dist>0&&dist<1)
			{
				needLandingAnimation=true;
				processKeys=false;
			}*/
			this.transform.Translate(Vector3.forward * Time.deltaTime*charRunSpeed);
			break;
		case movementState.FallingRight:
			if(climberInRange&&!rotatedMovement) 
			{
				charMovementState=movementState.PreHanging;
				break;
			}
			if(rotatedMovement)
			{
				this.transform.Rotate(new Vector3(0,1,0),180);
				rotatedMovement=false;
			}
			if(!fallingStart)
			{
				fallingStart=true;
				fallingStartPos=this.transform.position;
			}
			this.transform.Translate(Vector3.forward * Time.deltaTime*charRunSpeed);
			break;
		case movementState.PreHanging:
			if(!climbingMode)switchHangMode(true);
			if(!rotatedMovement)
			{
				this.transform.position=Vector3.Lerp(transform.position,nearestClimber.transform.position+hangPositionFix,Time.deltaTime*15f);
				if(this.transform.position==nearestClimber.transform.position+hangPositionFix)
				{
					charMovementState=movementState.Hanging;
				}
			}
			else
			{
				this.transform.position=Vector3.Lerp(transform.position,new Vector3(nearestClimber.transform.position.x-hangPositionFix.x,nearestClimber.transform.position.y+hangPositionFix.y,nearestClimber.transform.position.z+hangPositionFix.z),Time.deltaTime*15f);
				if(this.transform.position==new Vector3(nearestClimber.transform.position.x-hangPositionFix.x,nearestClimber.transform.position.y+hangPositionFix.y,nearestClimber.transform.position.z+hangPositionFix.z))
				{
					charMovementState=movementState.Hanging;
				}
			}

			break;
		case movementState.Climbing:
			//if(!climbingMode)switchHangMode(true);
			//this.transform.position=Vector3.Lerp(transform.position,nearestClimber.transform.position,Time.deltaTime*15f);
			//this.transform.position=Vector3.Lerp(transform.position,new Vector3(nearestClimber.transform.position.x,nearestClimber.transform.position.y-0.7f,nearestClimber.transform.position.z),Time.deltaTime*15f);
			/*if(this.transform.position==nearestClimber.transform.position)
			{
				switchHangMode(false);
				charMovementState=movementState.Idle;
			}*/
			/*if(!climbStarted)
			{
				climbStarted=true;
				WaitAndCallback(this.animation["climb"].length);
				switchHangMode(false);
				charMovementState=movementState.Idle;
				this.transform.position+=new Vector3(0.16f,0.85f);
			}*/
			break;
		}
	}


	/*IEnumerator WaitAndCallback(float waitTime,int state)
	{
		yield return new WaitForSeconds(waitTime-0.01f);
		if(state==0)completeClimb();
		else if(state==1) completeLanding();
	}*/

	void completeLanding()
	{
		needLandingAnimation=false;
		processKeys=true;
	}

	void smoothClimbToIdle(int x)
	{
		Debug.Log("smooth");
		this.animation.CrossFade("idle_breath");
	}

	void completeClimb()
	{
		/*if(!rotatedMovement)
		{
			this.transform.position+=climbPositionFix;
		}
		else
		{
			this.transform.position+=new Vector3(-climbPositionFix.x,climbPositionFix.y);
		}*/
		this.animation.CrossFade("idle_breath");
		switchHangMode(false);
		charMovementState=movementState.Idle;
		Debug.Log("Climb finished");
	}

	void switchHangMode(bool mode)
	{
		if(mode)
		{
			this.rigidbody.isKinematic=true;
			this.GetComponent<CapsuleCollider>().enabled=false;
			climbingMode=true;
		}
		else
		{
			this.rigidbody.isKinematic=false;
			this.GetComponent<CapsuleCollider>().enabled=true;
			bool climberInRange=false;
			nearestClimber=null;
			climbingMode=false;
		}
	}

	void findObjectsInRange(string tag, float range, Color col)
	{
		hitColliders = Physics.OverlapSphere(transform.FindChild("hands").transform.position, range);
		foreach(Collider coll in hitColliders)
		{
			if(coll.gameObject.tag==tag)
			{
				Debug.DrawLine(transform.FindChild("hands").transform.position, coll.gameObject.transform.position, col);
				//Debug.Log("Distance to "+tag+"="+Vector3.Distance(transform.FindChild("hands").transform.position,coll.gameObject.transform.position));
				if(tag=="climber"&&Vector3.Distance(transform.FindChild("hands").transform.position,coll.gameObject.transform.position)<maxClimberDistance)
				{
					if(charMovementState!=movementState.Climbing&& charMovementState!=movementState.PreHanging&& charMovementState!=movementState.Hanging&& charMovementState!=movementState.RunningLeft&& charMovementState!=movementState.JumpingRight)
					{
						nearestClimber=coll.gameObject;
						climberInRange=true;
					}
				}
				else climberInRange=false;
			}
		}
	}


	float rayCastGround()
	{
		RaycastHit hit;
		Ray ray = new Ray(this.transform.position,Vector3.down);
		if(Physics.Raycast( ray, out hit,2 ))
		{
			if(hit.transform.tag=="ground")
			{
				Debug.DrawLine(this.transform.position,hit.point,Color.white);
				return Vector3.Distance(this.transform.position,hit.point);
			}
		}
		return 0;
	}

	void climbAnimationHandle(int x)
	{
		Debug.Log("climb handler");
		if(x<30)
		{
			this.transform.position+=new Vector3(0,0.028f);
		}
		else
		{
			if(rotatedMovement)
			{
				this.transform.position+=new Vector3(-0.02f,0);
			}
			else
			{
				this.transform.position+=new Vector3(0.02f,0);
			}
		}
	}

	void OnCollisionEnter(Collision coll)
	{
		if(coll.gameObject.tag=="ground")
		{
			if(charMovementState==movementState.Falling||charMovementState==movementState.FallingRight||charMovementState==movementState.FallingLeft)
			{
				charMovementState=movementState.Idle;
				if(fallingStartPos.y-transform.position.y>4f)
				{
					needLandingAnimation=true;
					processKeys=false;
				}
			}
		}
		else if(coll.gameObject.tag!="ground")
		{
			if(charMovementState==movementState.JumpingUp)
			{
				charMovementState=movementState.Falling;
			}
			if(charMovementState==movementState.JumpingRight)
			{
				charMovementState=movementState.FallingRight;
			}
			if(charMovementState==movementState.JumpingLeft)
			{
				charMovementState=movementState.FallingLeft;
			}
		}
	}

	void OnCollisionExit(Collision coll)
	{
		if(coll.gameObject.tag=="ground")
		{
			if(charMovementState==movementState.Idle)
			{
				charMovementState=movementState.Falling;
			}
			if(charMovementState==movementState.RunningLeft)
			{
				charMovementState=movementState.FallingLeft;
			}
			if(charMovementState==movementState.RunningRight)
			{
				charMovementState=movementState.FallingRight;
			}
			fallingStartPos=transform.position;
		}
	}

}
