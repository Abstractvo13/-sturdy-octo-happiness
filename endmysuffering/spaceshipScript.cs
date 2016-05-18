using UnityEngine;
using System.Collections;

public class spaceshipScript : MonoBehaviour {


	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.forward);
		if (transform.position.z > 1000)
			Destroy (this.gameObject);
	}
}
