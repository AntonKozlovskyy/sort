using UnityEngine;
using System.Collections;

public class enemy_behavour : MonoBehaviour {

	// Use this for initialization

	public float speedRoutine,speedAttack;
	public Vector3 routineStartPos,routineEndPos;
	public float distanceToAttack;
	public string charName;

	enum EnemyState {routine,follow};
	enum EnemyLocalState{walking,jumping,attacking};

	bool movingLeft;

	GameObject charObj;

	EnemyState stateGlobal;
	EnemyLocalState stateLocal;
	void Start () {
		charObj=GameObject.Find(charName);
		stateGlobal=EnemyState.routine;
		stateLocal=EnemyLocalState.walking;
		movingLeft=false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		stateHandler();
		movementHandler();
	}

	void stateHandler()
	{
		if(Vector3.Distance(this.transform.position,charObj.transform.position)<=distanceToAttack)
		{
			stateGlobal=EnemyState.follow;
		}
		else stateGlobal=EnemyState.routine;

	}

	void movementHandler()
	{
		if(stateGlobal==EnemyState.routine)
		{
			makeMove();
		}
		else if(stateGlobal==EnemyState.follow)
		{
			makeAttackMove();
		}
	}

	void makeAttackMove()
	{
		if(charObj.transform.position.x>this.transform.position.x&&movingLeft)
		{
			movingLeft=false;
			this.transform.Rotate(new Vector3(0,1,0),180);
		}
		else if(charObj.transform.position.x<this.transform.position.x&&!movingLeft)
		{
			movingLeft=true;
			this.transform.Rotate(new Vector3(0,1,0),180);
		}

		if(!movingLeft)
		{
			if(this.transform.position.x<routineEndPos.x)
			{
				this.transform.Translate(new Vector3(1,0) * Time.deltaTime*speedAttack);
			}
		}
		else
		{
			if(this.transform.position.x>routineStartPos.x)
			{
				this.transform.Translate(new Vector3(1,0) * Time.deltaTime*speedAttack);
			}
		}
	}

	void makeMove()
	{
		if(!movingLeft)
		{
			if(this.transform.position.x<routineEndPos.x)
			{
				this.transform.Translate(new Vector3(1,0) * Time.deltaTime*speedRoutine);
			}
			else
			{
				movingLeft=true;
				this.transform.Rotate(new Vector3(0,1,0),180);
			}
		}
		else
		{
			if(this.transform.position.x>routineStartPos.x)
			{
				this.transform.Translate(new Vector3(1,0) * Time.deltaTime*speedRoutine);
			}
			else
			{
				movingLeft=false;
				this.transform.Rotate(new Vector3(0,1,0),180);
			}
		}
	}
}
