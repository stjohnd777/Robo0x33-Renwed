using UnityEngine;
using System.Collections;

public class ActionWaypointMovement : MonoBehaviour, IFacingRight {

	[Tooltip("Stop Movement is true")]
	public bool IsSuppended = false;

	[Tooltip("The GameObject moving under waypoint.")]
	public GameObject target; 

	[Tooltip("The waypoint.")]
	public GameObject[] waypoints; // array of all the waypoints

	[Range(0.0f, 30.0f)] 
	public float moveSpeed = 5f; 

	[Tooltip("Time to wait at waypoint.")]
	public float waitAtWaypointTime = 1f; 

	[Tooltip("Loop through the waypoint")]
	public bool IsLooping = true; // should it loop through the waypoints

	[Tooltip("Manage Aninimation ")]
	public bool IsManageAnimations = true;
 
	public bool IsFlipX = true; // should it loop through the waypoints

	public bool IsFlipY = false; // should it loop through the waypoints
	// private variables

	public bool isMoving = true;

	public bool isGrounded = true;

	Transform targetTransform;
	int waypointIndex = 0;		// used as index for My_Waypoints
	float timeToStartMovingAgain;

	//Animator _animator;
	private GroundProbeCircleOverlaps groundProb;


	void Awake(){

	}

	// Use this for initialization
	void Start () {

		groundProb = GetComponent<GroundProbeCircleOverlaps>();
		targetTransform = target.transform;
		//_animator 		= GetComponent<Animator>();
		timeToStartMovingAgain = 0f;
		isMoving = true;

//		if (_animator!=null){
//			_animator.SetBool("IsGrounded",true);
//			_animator.SetBool("IsMoving",false);
//		}
	}

	// game loop
	void Update () {

		if (groundProb) {
			isGrounded = groundProb.IsGrounded;
		}

		if (IsSuppended) {
			isMoving = false;
			return;
		} else {
			isMoving = true;
		}
		if (IsNotInWaitState ()) {
			Movement ();
			//_animator.SetBool("IsMoving",true);
			isMoving = true;
		} else {
			//_animator.SetBool("IsMoving",false);
			isMoving = false;
		}
	}

	bool IsNotInWaitState(){
		return Time.time >= timeToStartMovingAgain;
	}

	bool HasWaypoints(){
		return (waypoints.Length != 0);
	}
		
	public float tolerance = 5;
	void Movement() {


		if ( HasWaypoints() && (isMoving)) {

			// move towards waypoint
			Vector3 currentWaypointPosition = waypoints[waypointIndex].transform.position;
			Vector3 currentPositionTarget = targetTransform.position;
		
			float step = moveSpeed * Time.deltaTime;
			targetTransform.position = Vector3.MoveTowards(currentPositionTarget, currentWaypointPosition,step);

			float  _vx = currentWaypointPosition.x - currentPositionTarget.x ;
            float  _vy = currentWaypointPosition.y - currentPositionTarget.y;

            if (IsFlipX)
            {
                FlipX(_vx);
            }
            if (IsFlipY)
            {
                FlipY(_vy);
            }


			float distanceBetweenTargetAndWaypoint = Vector3.Distance(currentWaypointPosition, targetTransform.position);
			if(distanceBetweenTargetAndWaypoint <= tolerance) {
				waypointIndex++;
				timeToStartMovingAgain = Time.time + waitAtWaypointTime;
			}

			// reset waypoint back to 0 for looping, otherwise flag not moving for not looping
			if(waypointIndex == waypoints.Length) {
				if (IsLooping){
					waypointIndex = 0;
				}else {
					isMoving = false;
				}
			}
		}  
	}

	// flip the enemy to face torward the direction he is moving in
	void FlipX(float _vx) {

		// get the current scale
		Vector3 localScale = targetTransform.localScale;

		if ((_vx>0f)&&(localScale.x<0f))
			localScale.x*=-1;
		else if ((_vx<0f)&&(localScale.x>0f))
			localScale.x*=-1;

		// update the scale
		targetTransform.localScale = localScale;
	}

    // flip the enemy to face torward the direction he is moving in
    void FlipY(float _vy)
    {

        // get the current scale
        Vector3 localScale = targetTransform.localScale;

        if ((_vy > 0f) && (localScale.y < 0f))
            localScale.y *= -1;
        else if ((_vy < 0f) && (localScale.y > 0f))
            localScale.y *= -1;

        // update the scale
        targetTransform.localScale = localScale;
    }


    public bool IsFacingRight(){
		return (targetTransform.localScale.x > 0f) ? true : false;
	}
	 

}
