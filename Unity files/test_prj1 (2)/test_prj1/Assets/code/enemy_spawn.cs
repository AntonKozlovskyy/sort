using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemy_spawn : MonoBehaviour {


	public GameObject enemyObject;
	public int enemyCount;
	// Use this for initialization

	int spawnsCount;

	List<GameObject> enemyList= new List<GameObject>();

	void Start () {
		spawnsCount=0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Random.Range(0,100)==10&&spawnsCount<enemyCount) spawnEnemy();
	}

	void spawnEnemy()
	{
		Debug.Log("spawning");
		enemyList.Add((GameObject)Instantiate(enemyObject,this.transform.position,this.transform.rotation));
		spawnsCount++;
	}
}
