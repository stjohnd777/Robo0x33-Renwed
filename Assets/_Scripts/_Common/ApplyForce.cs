using UnityEngine;
using System.Collections;

public class ApplyForce : MonoBehaviour {


    public Vector2 force;

    public string[] tags;

	public float speedCapVertical = 500f; 

	public float speedCapHorizontal = 500f; 

	public bool IsOnTrigger = true;

	public bool IsOnCollision = true;


	void OnTriggerEnter2D(Collider2D c2d){

	}
	void OnTriggerStay2D(Collider2D collider2D){
		if (IsOnTrigger) {
			
			if (IsInPrejudiceTagList (collider2D.gameObject.tag)) {
				Rigidbody2D body = collider2D.gameObject.GetComponent<Rigidbody2D> ();
				if (body) {
					if (body.velocity.y < speedCapVertical) {
 
						Vector2 oldVelocity = body.velocity;
						Vector2 newVelocity = new Vector2 (oldVelocity.x + force.x, oldVelocity.y + force.y);
						body.velocity = newVelocity;
					}
				}
				
			}
		}
	}
	void OnTriggerExit2D(Collider2D c2d){

	}


	void OnCollisionEnter2D(Collision2D c2d){
	
	}

	void Update(){

	}
	void OnCollisionStay2D(Collision2D collision2D){

		if (IsOnCollision) {
			if (IsInPrejudiceTagList (collision2D.gameObject.tag)) {
				Rigidbody2D body = collision2D.gameObject.GetComponent<Rigidbody2D> ();
				if (body) {

					if (body.velocity.y < speedCapVertical) {
						body.AddForce (force);
					}
						
//					if (body.velocity.y > speedCapVertical) {
//						Vector2 oldVelocity = body.velocity;
//						Vector2 newVelocity = new Vector2 (oldVelocity.x, (oldVelocity.y > speedCapVertical) ?  speedCapVertical :  oldVelocity.y );
//						body.velocity = newVelocity;
//					}

				}

			}
		}
	}
	void OnCollisionExist2D(Collision2D c2d){

	}

	private bool IsInPrejudiceTagList(string tag){
		bool ret = false;
		foreach( string prejudiceTag in tags){
			if ( tag == prejudiceTag){
				ret = true;
				return ret;
			}
		}
		return ret;
	}
    
}
