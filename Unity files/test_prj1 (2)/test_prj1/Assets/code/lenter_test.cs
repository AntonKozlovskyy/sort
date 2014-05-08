using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class lenter_test : MonoBehaviour {

	// Use this for initialization
	public int sphereCount;
	public float sphereSpeed;
	public Vector3 sphereSize;
	public float movementRadius;
	public float toCharDistance;
	
	Vector3[] sphereMovementPoint;
	Vector3 sphereStartPos;
	float sphereToCharCount;

	List<GameObject> lightSphere= new List<GameObject>();

	enum lenterState{Idle,ToChar,Empty,Off};

	lenterState currentState;

	Transform charObject;

	void Start () 
	{
		sphereToCharCount=0;
		charObject=GameObject.Find("char").transform.FindChild("hands");
		currentState=lenterState.Idle;
		//lightSphere=new GameObject[sphereCount];
		sphereMovementPoint=new Vector3[sphereCount];
		sphereStartPos=this.gameObject.transform.FindChild("start").position;
		for(int i=0;i<sphereCount;i++)
		{
			lightSphere.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
			lightSphere[i].GetComponent<SphereCollider>().enabled=false;
			lightSphere[i].transform.position=sphereStartPos;
			lightSphere[i].transform.localScale=sphereSize;
			sphereMovementPoint[i]=new Vector3(Random.Range(-1*movementRadius,movementRadius),Random.Range(-1*movementRadius,movementRadius),0);
			sphereMovementPoint[i]+=sphereStartPos;
			Debug.Log("RandomPoint "+sphereMovementPoint[i]);

		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(currentState!=lenterState.Off)
		{
			if(Vector3.Distance(charObject.transform.position,this.transform.position)<toCharDistance&&currentState==lenterState.Idle)
			{
				currentState=lenterState.ToChar;
			}

			if(currentState==lenterState.Idle) moveSphereIdle();
			else if(currentState==lenterState.ToChar) moveSphereToChar();
			else if(currentState==lenterState.Empty) lightFade();
		}
	}

	void lightFade()
	{
		if(this.GetComponent<Light>().intensity>0)
		{
			this.GetComponent<Light>().intensity-=Time.deltaTime*10;
		}
		else
		{
			currentState=lenterState.Off;
		}
	}

	void moveSphereToChar()
	{
		if(lightSphere.Count>0)
		{
			for(int i=0;i<lightSphere.Count;i++)
			{
				if(lightSphere[i].transform.position!=charObject.transform.position)
				{
					float distance=Vector3.Distance(lightSphere[i].transform.position, charObject.transform.position);
					if(distance>0)
					{
						lightSphere[i].transform.position = Vector3.Lerp(lightSphere[i].transform.position, charObject.transform.position, Time.deltaTime* sphereSpeed/distance*2);
					}
					//lightSphere[i].transform.Translate(charObject.transform.position*Time.deltaTime*sphereSpeed*10);
				}
				else
				{
					Destroy(lightSphere[i]);
					lightSphere.RemoveAt(i);
					//~lightSphere[i]();
				}
			}
		}
		else currentState=lenterState.Empty;
	}

	void moveSphereIdle()
	{
		for(int i=0;i<sphereCount;i++)
		{
			if(lightSphere[i].transform.position!=sphereMovementPoint[i])
			{
				float distance=Vector3.Distance(lightSphere[i].transform.position, sphereMovementPoint[i]);
				lightSphere[i].transform.position=Vector3.Lerp(lightSphere[i].transform.position,sphereMovementPoint[i],Time.deltaTime* sphereSpeed/distance);
				//sphereLerpCurPos[i]+=sphereSpeed;
			}
			else
			{
				sphereMovementPoint[i]=new Vector3(Random.Range(-1*movementRadius,movementRadius),Random.Range(-1*movementRadius,movementRadius),0);
				sphereMovementPoint[i]+=sphereStartPos;
				//sphereLerpCurPos[i]=0;
			}
		}
	}
}
