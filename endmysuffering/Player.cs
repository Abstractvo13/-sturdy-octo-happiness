using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	float speed = 5;

	void Awake(){
		if(!GetComponent<NetworkView>().isMine){

			enabled=false;	
		}

		if(gameObject.tag == "Untagged"){
			GameObject target1 = GameObject.FindGameObjectWithTag("Player1");
			GameObject target2 = GameObject.FindGameObjectWithTag("Player2");
			GameObject target3 = GameObject.FindGameObjectWithTag("Player3");
			GameObject target4 = GameObject.FindGameObjectWithTag("Player4");

			if(target1 == null)
				gameObject.tag = "Player1";
			else if(target1 == null)
				gameObject.tag = "Player2";
			else if(target1 == null)
				gameObject.tag = "Player3";
			else if(target1 == null)
				gameObject.tag = "Player4";
		}
	}

	void Start(){

	}

	void Update(){

		if(GetComponent<NetworkView>().isMine){
			InputMovement();
		}	
	}


	void InputMovement()
	{
		if (Input.GetKey(KeyCode.W))
			GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + Vector3.forward * speed * Time.deltaTime);

		if (Input.GetKey(KeyCode.S))
			GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position - Vector3.forward * speed * Time.deltaTime);

		if (Input.GetKey(KeyCode.D))
			GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + Vector3.right * speed * Time.deltaTime);

		if (Input.GetKey(KeyCode.A))
			GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position - Vector3.right * speed * Time.deltaTime);

	}


	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting){


			Vector3 pos = transform.position;		
			stream.Serialize(ref pos);

		}else{

			Vector3 posReceive = Vector3.zero;
			stream.Serialize(ref posReceive); 
			transform.position = Vector3.Lerp(transform.position, posReceive,1);	
		} 
	}
}
