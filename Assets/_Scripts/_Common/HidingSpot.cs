using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour {

 
	int enemyLayerNumber ;
	int playerLayerNumber ;
	void OnTriggerEnter2D(Collider2D collider){

		//print(collider.gameObject.name);
		if( collider.gameObject.tag == "Player"){
			//Debug.Log("hide player");
			Physics2D.IgnoreLayerCollision(playerLayerNumber, enemyLayerNumber, true); 
			//Rigidbody2D rb = collider.gameObject.GetComponent<Rigidbody2D>();
			 
			 
		}
	}

	void Awake(){

		enemyLayerNumber = LayerMask.NameToLayer("Enemy");
		playerLayerNumber = LayerMask.NameToLayer("Player");
	}

	void OnTriggerStay2D(Collider2D collider){

		//print(collider.gameObject.name);
		if( collider.gameObject.tag == "Player"){
			//Debug.Log("hide player");
			Physics2D.IgnoreLayerCollision(playerLayerNumber, enemyLayerNumber, true); 
		}
	}

	void OnTriggerExit2D(Collider2D collider){
		if( collider.gameObject.tag == "Player"){
			//Debug.Log("un hide player");
			Physics2D.IgnoreLayerCollision(playerLayerNumber, enemyLayerNumber, false); 
		}
	}
}
