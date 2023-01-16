using UnityEngine;
using System.Collections;

/*
 * 
 *     ______
 *    | stun |
 *    ++++++++
 *    ++++++++
 *    | obj  |
 *    |      | 
 *    | ++++ |
 *    ++|floor|++
 *      ++++
 */
public class Stunnable : MonoBehaviour, IStunnable {

	[Tooltip("Players Layer.")]
	public string playerLayer = "Player";  

	[Tooltip("Move Stunned Enemy to this Layer.")]
	public string stunnedLayer = "StunnedEnemy";  // name of the layer to put enemy on when stunned


//	[Tooltip("Check gameObject for detecting stun.")]
//	public GameObject stunnedCheck; // what gameobject is the stunnedCheck

	[Tooltip("Stunned time.")]
	public float stunnedTime = 3f;   // how long to wait at a waypoint


	// SFXs
	[Tooltip("Stuned Sound.")]
	public AudioClip stunnedSFX;

	[Tooltip("Stunned Trigger Animation.")]
	public string stunnedTrigger = "Stunned";

	[Tooltip("UnStunned Trigger Animation.")]
	public string unStunnedTrigger = "Stunned";


	AudioSource _audio;
	Rigidbody2D _rigidbody;
	Animator _animator;
	ActionWaypointMovement wps;
	AutoShooter2D shooter;


	bool isStunned = false;
	int _enemyLayer;
	int _stunnedLayer;


	void Awake() {
 
		_rigidbody = GetComponent<Rigidbody2D> ();
 
		_animator = GetComponent<Animator>();
 
		_audio = GetComponent<AudioSource> ();
		if (_audio==null) { // if AudioSource is missing
			Debug.LogWarning("AudioSource component missing from this gameobject. Adding one.");
			// let's just add the AudioSource component dynamically
			_audio = gameObject.AddComponent<AudioSource>();
		}

		// determine the enemies specified layer
		_enemyLayer = this.gameObject.layer;

		// determine the stunned enemy layer number
		_stunnedLayer = LayerMask.NameToLayer(stunnedLayer);

		wps = GetComponent<ActionWaypointMovement> ();

		shooter = GetComponent<AutoShooter2D> ();

	}


//	void OnCollisionEnter2D(Collision2D other)
//	{
//		_rigidbody.AddForce(new Vector3(0,bounceForce,0));
//
//		Stunned();
//
//	}

 
	public void Stunned () {

		if (!isStunned) 
		{
			
			if (wps != null) {
				wps.IsSuppended = true;
			}

			if ( shooter != null){
				shooter.active = false;
			}

			isStunned = true;

			// provide the player with feedback that enemy is stunned
			playSound(stunnedSFX);

			// Trigger stuned animation
			_animator.SetTrigger(stunnedTrigger);

			// stop moving
			_rigidbody.velocity = new Vector2(0, 0);

			// switch layer to stunned layer so no collisions with the player while stunned
			this.gameObject.layer = _stunnedLayer;
			//stunnedCheck.layer    = _stunnedLayer;

			// make sure collision are off between the playerLayer and the stunnedLayer
			// which is where the enemy is placed while stunned
			Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayer), _stunnedLayer, true); 

			// start coroutine to stand up eventually
			StartCoroutine (_UnStun ());
		}
	}

	// Update is called once per frame
	public void UnStun () {

		isStunned = false;

		if (wps != null) {
			wps.IsSuppended = false;
		}
		//GetComponent<ActionWaypointMovement>().IsSuppended = false;

		if ( shooter != null){
			shooter.active = true;
		}

		// switch layer back to regular layer for regular collisions with the player
		this.gameObject.layer = _enemyLayer;
		//stunnedCheck.layer = _enemyLayer;



		// provide the player with feedback
		_animator.SetTrigger(unStunnedTrigger);
	}

	// coroutine to unstun the enemy and stand back up
	IEnumerator _UnStun()
	{
		yield return new WaitForSeconds(stunnedTime); 

		UnStun();
	}

	// play sound through the audiosource on the gameobject
	void playSound(AudioClip clip)
	{
		_audio.PlayOneShot(clip);
	}
}
