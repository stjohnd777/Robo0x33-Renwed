using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	//public bool IsForce = false;
	public float HorizontalSpeed = 5;

	public CommandRequestVerbs[] input;

	private Rigidbody2D rb;
	private State inputState;
 
	void Awake () {
		rb = GetComponent<Rigidbody2D> ();
		inputState = GetComponent<State> ();
	}
	
 
	void Update () {
	
		float hInputValue = Input.GetAxis ("Horizontal");
		hInputValue = Mathf.Abs (hInputValue);
		float velocityX = HorizontalSpeed * hInputValue;
		bool right = inputState.GetButtonValue (input [0]);
		bool left = inputState.GetButtonValue (input [1]);

		if (right || left) {
			velocityX *= (left ? -1 : 1);
		}

		Vector2 newVelocity = new Vector2 (velocityX, rb.velocity.y);
		rb.velocity = newVelocity;

//		if (IsForce) {
//			Vector2 force = new Vector2 (hspeed*hInputValue, 0);
//			rb.AddForce (force);
//
//		} else {
//			Vector2 newVelocity = new Vector2 (hspeed*hInputValue, rb.velocity.y);
//			rb.velocity = newVelocity;
//		}

	}
}
