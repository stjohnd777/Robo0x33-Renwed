using UnityEngine;
using System.Collections;
using sillymutts;
/*
 * GameObject hosting this script will give damage to other ObjectObjects
 * with the specified tags.
 * 
 * that come into pyhsics collison or trigger
 * 
 */
public class GiveDamage : MonoBehaviour {


	public bool disable = false;

	public string[] disableTagList = {"HiddingSpot"};

	[Tooltip("If true use aufio source on SoundManager.")]
	public bool IsUseSoundManager = false;

	[Header("Game Object to Damage")]
	[Tooltip("The List of tags that will be dealt damage when contract occures")]
	public string[] prejudiceTagList;

	[Tooltip("The all object in these layers will be dealt damage ")]
	public LayerMask prejudiceLayerList;

	[Tooltip("Damage to dive to game objecs in the prejudice list ")]
	public int damage = 1;


	[Header("Damage Effect Relative to the GameObejct hosting this script")]
	[Tooltip("Sound to play when a give damage event occures")]
	public AudioClip damageSFX;
	[Tooltip("Partical Effect to display when a give damage event occures")]
	public GameObject damageParticalEffect;
	[Tooltip("Animation to play on the GamageGiver when a give damage event occures")]
	public string damageGiverAnimation;
	[Tooltip("Use trigger to initiate give damage event")]
	public bool IsOnTrigger = true;


	[Header("Activate Deal Damage on:")]
	[Tooltip("Use collision to initiate give damage event")]
	public bool IsOnCollision2D = false;
	[Tooltip("Remove the damage giver whne damage event, for example a bullet")]
	public bool   remove = true;
	[Tooltip("Time to wait before reoving the damage giver whne damage event, for example let the animation finish")]
	public int    waitToRemove = 1;


	AudioSource _audioSource;
	Animator _animator;
	//ActionWaypointMovement actionWaypointMovement ;

	void Awake() {

		_audioSource = GetComponent<AudioSource> ();

		_animator = GetComponent<Animator>();

		//actionWaypointMovement = GetComponent<ActionWaypointMovement>();

	}
 
	// Update is called once per frame
	void Update () {
	
	}

	public bool IsInPrejudiceTagList(string tag){
		bool ret = false;
		foreach( string prejudiceTag in prejudiceTagList){
			if ( tag == prejudiceTag){
				ret = true;
				return ret;
			}
		}
		return ret;
	}

	public bool IsInDisableTagList(string tag){
		bool ret = false;
		foreach( string atag in disableTagList){
			if ( tag == atag){
				ret = true;
				return ret;
			}
		}
		return ret;
	}

	bool IsInPrejudiceLayerList(int  layerInt){
		bool ret = false;
		if ( (layerInt & prejudiceLayerList) != 0) {
			ret = true;
		}
		return ret;
	}

	// if Player hits the stun point of the enemy, then call Stunned on the enemy
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (IsInDisableTagList (collision.gameObject.tag)) {
			disable = true;
			return;
		} else {
			disable = false;
		}
			
		if ( (IsInPrejudiceLayerList( collision.gameObject.layer )|| IsInPrejudiceTagList (collision.gameObject.tag)) && IsOnCollision2D && !disable) {//if (collision.gameObject.tag == "Player")
			DelegateDamage (collision.gameObject);
			if (remove) {
				Invoke ("CleanUpAndRemove", waitToRemove);
			}
		} else {
			//Invoke ("CleanUpAndRemove", waitToRemove);
		}
			
  
		if (IsInPrejudiceLayerList( collision.gameObject.layer ) && IsOnCollision2D ) 
		{
			DelegateDamage(collision.gameObject);
		}
	 
		
	}

	// Attack player
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (IsInDisableTagList (collision.gameObject.tag)) {
			disable = true;
			return;
		} else {
			disable = false;
		}

		GameObject go = collision.gameObject;
		int otherLayer = collision.gameObject.layer;
		string otherTag = collision.gameObject.tag;

		if ( (IsInPrejudiceLayerList( otherLayer )|| IsInPrejudiceTagList (otherTag) ) && IsOnTrigger  && !disable) {
			DelegateDamage (collision.gameObject);
			if (remove) {
				Invoke ("CleanUpAndRemove", waitToRemove);
			}
		} else {
			//Invoke ("CleanUpAndRemove", waitToRemove);
		}
	}

 
	void  DelegateDamage(GameObject gameObjectRecievingDamange){

		// first find the health component on the game object
		// in the collision, most cases this will be not null
		// however if the collision is on an object with not health
		// try the parent object
		IHealth health = gameObjectRecievingDamange.GetComponent<IHealth>();
		if (health == null){
		 health = gameObjectRecievingDamange.GetComponentInParent<IHealth>();
		}

		if ( health !=null){
			if (health.GetHealth() > 0) {
				health.ApplyDamage (damage);
				ApplyDamageGiversEffects (gameObjectRecievingDamange);
			}
		}
	}


	void ApplyDamageGiversEffects(GameObject go)
	{
		//FloatingTextController.CreateFloatingText("-"+damage,go.transform);

		// attack sound
 		PlaySound(damageSFX);

		if (damageParticalEffect)
		{
			Instantiate(damageParticalEffect,transform.position,transform.rotation);
		}

		if ( damageGiverAnimation != null && _animator!=null){
			_animator.SetTrigger(damageGiverAnimation);
		}
 
	}

	// destroy the gameobject
	void CleanUpAndRemove( )
	{
		GameObjectUtility.Destroy(gameObject);
	}

	void PlaySound(AudioClip sfx){

		if ( sfx!= null){
			if ( IsUseSoundManager ){
				SoundManager.getInstance().PlaySoundOnce(sfx);
			} else {
				if (_audioSource != null){
					_audioSource.PlayOneShot(sfx);
				}
			}
		}
	}

}
 