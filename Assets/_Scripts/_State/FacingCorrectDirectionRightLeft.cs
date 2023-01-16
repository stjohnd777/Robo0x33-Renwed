using UnityEngine;
using System.Collections;

[System.Serializable]
public enum FlipType {
	IsHorzontalInput, IsHorizontalVelocity, IsWaypointAction
}
public class FacingCorrectDirectionRightLeft : MonoBehaviour , IFacingRight{


	public GameObject target;

	public bool isFacingRight ;

	public FlipType type = FlipType.IsHorzontalInput;
 
	Rigidbody2D body2D;

	ActionWaypointMovement waypoint;

	void Awake () {
	
		body2D = GetComponent<Rigidbody2D> ();

		waypoint = GetComponent<ActionWaypointMovement> ();
	}
	
 
	float move = 0;

	void Update () {

		switch (type) {
			case  FlipType.IsHorzontalInput:
			    move = Input.GetAxis ("Horizontal");
				break;
		case  FlipType.IsHorizontalVelocity:
			move = body2D.velocity.x;
			break;
		case  FlipType.IsWaypointAction:
			isFacingRight = waypoint.IsFacingRight ();
			break;
		}

		if (move == 0) {
			return;
		}
			
		if ( !IsFaceCorrectDirection() ){
			FlipX ();
		}
	}

	public bool IsFacingRight(){
		return isFacingRight;
	}
	
     bool IsFaceCorrectDirection(){
		return  (move > 0 && isFacingRight) || (move < 0 && !isFacingRight);
	}
	
    void FlipX(){
		isFacingRight = !isFacingRight;
		target.transform.localScale = new Vector3 (-target.transform.localScale.x,target.transform.localScale.y,target.transform.localScale.z);
	}
}
