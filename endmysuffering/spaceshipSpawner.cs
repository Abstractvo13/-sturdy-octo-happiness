using UnityEngine;
using System.Collections;

public class spaceshipSpawner : MonoBehaviour {

	public GameObject ship;
	float timer=0;
	int SpawnTime;
	// Use this for initialization
	void Start () {
		SpawnTime = Random.Range (10, 15);
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > SpawnTime) 
		{
			Instantiate (ship, new Vector3(Random.Range(25.0f,270.0f),transform.position.y,transform.position.z), Quaternion.identity);
			SpawnTime = Random.Range (10, 15);
			timer = 0;
		}
	}
}
