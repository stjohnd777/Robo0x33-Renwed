using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour {

	public string tagsIsWall = "Wall";

	public bool isOnWall = false;

	public LayerMask  whatIsWall;
	void Awake(){

	}
	void OnTriggerEnter2D(Collider2D collision)
	{
		if ( collision.gameObject.tag == tagsIsWall){
			isOnWall = true;
		}

//		if ( (collision.gameObject.layer && whatIsWall) !=0 ) {
//			isOnWall = true;
//		}
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		if ( collision.gameObject.tag == tagsIsWall){
			isOnWall = true;
		}

//		if ( (collision.gameObject.layer && whatIsWall) !=0 ) {
//			isOnWall = true;
//		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if ( collision.gameObject.tag == tagsIsWall){
			isOnWall = false;
		}
//		if ( (collision.gameObject.layer && whatIsWall) !=0) {
//			isOnWall = false;
//		}
	}
}
