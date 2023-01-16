using UnityEngine;
using System.Collections;


public interface VisionBehavior {

	void SeeSomeThing();

	void SeeYou();

	void LostYou();

}


//public class ShootAtYouWhenISeeYou : MonoBehaviour, VisionBehavior{
//
//	public GameObject agent;
//
//	public float delay = .1f;
//
//	AutoShooter2D shooter;
//
//	ActionWaypointMovement patrolBehavior;
//
//	Animator animator;
//	public AudioClip  _seeYou;
//	public string     seeYouAnimationTrigger;
//
//	public void SeeSomeThing(){
//	}
//
//	public bool seeYou = false;
//
//	public void SeeYou(){
//		_SeeYou ();
//	}
//
//	IEnumerator _SeeYou(){
//		Debug.Log("SEE YOU I shoot at you!");
//		patrolBehavior.IsSuppended = true;
//		SoundManager.getInstance().PlaySound(_seeYou);
//		if(animator!=null){
//			animator.SetTrigger("Shoot");
//		}
//		shooter.DoFire();
//		yield return new WaitForSeconds (delay);
//		seeYou = false;
//		patrolBehavior.IsSuppended = false;
//	}
//
//	public void LostYou(){
//
//	}
//}
//
//public class MoveTowardsYouWhenISeeYou : MonoBehaviour, VisionBehavior{
//
//	public GameObject agent;
//
//	public float delay = .1f;
//
//	public void SeeSomeThing(){
//	}
//
//	public void SeeYou(){
//
//	}
//
//	public void LostYou(){
//
//	}
//}

public class Vision : MonoBehaviour {

	public GameObject agent;

	public float delay = .1f;

	public Collider2D visionCollider2D;

	public LayerMask  layerInVision;

	public string[]   tagsInVision;

	public string[]   tagsBlockingVision;

	public AudioClip  _seeSomething;
	public string     seeSeeSomethingTrigger;

	public AudioClip  _seeYou;
	public string     seeYouAnimationTrigger;

	public AudioClip  _lostYou;
	public string     seeLostYouTrigger;

	AutoShooter2D shooter;

	ActionWaypointMovement patrolBehavior;

	Animator animator;

	void Awake(){
		shooter = agent.GetComponent<AutoShooter2D>();
		patrolBehavior = agent.GetComponent<ActionWaypointMovement>();
		animator = agent.GetComponent<Animator>();
	}

	bool IsVisable(GameObject target)
	{
		
		Vector3 agentPos  = agent.transform.position; 
		Vector3 targetPos = target.transform.position; 
		Vector3 direction = targetPos - agentPos;

		float length = direction.magnitude; 
		direction.Normalize(); 
		//Ray ray = new Ray (agentPos, direction);

		/*
		Physics2D.RaycastAll
		public static RaycastHit2D[] RaycastAll(
			Vector2 origin, 
			Vector2 direction, 
			float distance = Mathf.Infinity, 
			int layerMask = DefaultRaycastLayers, 
			float minDepth = -Mathf.Infinity, 
			float maxDepth = Mathf.Infinity); 
		Parameters
			origin 		: The point in 2D space where the ray originates.
			direction	: Vector representing the direction of the ray.
			distance	: Maximum distance over which to cast the ray.
			layerMask   : Filter to detect Colliders only on certain layers.
			minDepth    : Only include objects with a Z coordinate (depth) greater than or equal to this value.
			maxDepth    : Only include objects with a Z coordinate (depth) less than or equal to this value.
		*/

		//Cast the created ray with length distance b/t agent and target in vision and retrieve all the hits: 
		RaycastHit2D[] hits = Physics2D.RaycastAll(agentPos,direction, length);

		// Check for any wall between the visor and target. 
		// If none, we can proceed to call our functions or develop our 
		// behaviors that are to be triggered: 

		foreach( RaycastHit2D hit in hits)
		{
			GameObject hitObj = hit.collider.gameObject;    
			string tag = hitObj.tag;    

			if (IsBlockingTagList (tag)) 
			{
				return false; 
			}
		}
		// target is visible 
	    return true;
	}

	public bool seeSomeThing = false;
	void OnTriggerEnter2D (Collider2D collider) {
		string tag = collider.gameObject.tag;
		bool interested = IsVisionTagList(tag);
		if ( interested && !seeSomeThing   ){
			if (IsVisable (collider.gameObject)) {
				seeSomeThing = true;

				// execute behavior
				StartCoroutine (SeeSomething ());
			}
		}
	}
	IEnumerator SeeSomething(){
		Debug.Log("I THINK I SAW SOMETHING ");
		SoundManager.getInstance().PlaySoundOnce(_seeSomething);
		yield return new WaitForSeconds (delay);
		seeSomeThing = true;
	}



	public bool seeYou = false;
	void OnTriggerStay2D (Collider2D collider) {
		string tag = collider.gameObject.tag;
		bool interested = IsVisionTagList(tag);
		if ( interested && !seeYou  ){
			if (IsVisable (collider.gameObject)) {
				seeSomeThing = false;
				seeYou = true;
				StartCoroutine (SeeYou ());
			}
		}
	}
	IEnumerator SeeYou(){
		
		Debug.Log("SEE YOU");

		if ( patrolBehavior)
			patrolBehavior.IsSuppended = true;

		SoundManager.getInstance().PlaySoundOnce(_seeYou);

		if(animator!=null){
			animator.SetBool("IsShoot",true);
			//animator.SetTrigger("Shoot");
		}
		shooter.DoFire();
	

		yield return new WaitForSeconds (delay);

		NpcState npcState = GetComponent<NpcState> ();
		if (npcState) {
			npcState.IsShooting = false;
		} 
		seeYou = false;
		if ( patrolBehavior)
        	patrolBehavior.IsSuppended = false;
		 
    }

	public bool lostYou = false;
	void OnTriggerExit2D (Collider2D collider) {
		string tag = collider.gameObject.tag;
		bool interested = IsVisionTagList(tag);
		if ( interested && lostYou  ){
			if (IsVisable (collider.gameObject)) {
				lostYou = true;
				StartCoroutine (LostYou ());
			}
		}
	}
	IEnumerator LostYou(){
		Debug.Log("I LOST YOU ");
		SoundManager.getInstance().PlaySoundOnce(_lostYou);
		yield return new WaitForSeconds (.1f);
		lostYou = false;
		seeYou = false;
		seeSomeThing = false;
		if ( patrolBehavior)
			patrolBehavior.IsSuppended = false;

	}



	public bool IsVisionLayerList(string tag){
		bool ret = true;

		return ret;
	}

	public bool IsVisionTagList(string tag){
		bool ret = false;
		foreach( string prejudiceTag in tagsInVision){
			if ( tag == prejudiceTag){
				ret = true;
				return ret;
			}
		}
		return ret;
	}

	public bool IsBlockingTagList(string tag){
		bool ret = false;
		foreach( string blockingTag in tagsBlockingVision){
			if ( tag == blockingTag){
				ret = true;
				return ret;
			}
		}
		return ret;
	}
}
