using UnityEngine;
using System.Collections;
using System.IO;
using System;
public class PickUp : MonoBehaviour
{

	public enum PickupType {
		Coin, Key, HealthUpPoint, ShieldPoints, ExtraLive, PowerUpGun, SpeedUp, PowerUpGun1,PowerUpGun2,PowerUpGun3,PowerUpGun4
	}

	[Header ("Inventory Pickup")]
	public string targetTag = "Player";
	public bool IsInventory = false;
	public string InventoryKey;
    public int countInventory = 1;
	[Space (10)]

	[Header ("Non-Inventory Pickup")]
	[Tooltip ("Value Added to Score for Pickup")]
	public int    pickupValue = 1;
	public string type;
	[Space (10)]

	[Header ("PickUp Interaction Occures On")]
	public bool useTrigger = true;
	public bool useCollision = false;
	[Space (10)]

	[Header ("PickUp Effects")]
	public AudioClip sfx;
	public GameObject PickUParticalEffect;
	[Space (10)]

	public bool taken = false;

	AudioSource _audio;

	void Awake () {
		_audio = GetComponent<AudioSource> ();
		if (_audio==null) {
			_audio = gameObject.AddComponent<AudioSource>();
		}

	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (useTrigger) {
			if (other.gameObject.tag == targetTag && !taken) {
				HandlePickUp (other.gameObject);
			}
		}

	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (useCollision) {
			if (collision.gameObject.tag == targetTag && !taken) {
				HandlePickUp (collision.gameObject);
			}
		}
	}

 
	private void HandlePickUp (GameObject gameObject)
	{
		PlaySound (sfx);

		taken = true;

		if (PickUParticalEffect) {
			// try this
			GameObject go = Instantiate (PickUParticalEffect,GameManager.getInstance().transform); ;//, transform.position, transform.rotation,false);
			go.transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z);
			go.transform.rotation = new Quaternion(transform.rotation.x,transform.rotation.y,transform.rotation.z,transform.rotation.w);
		}

		if (IsInventory) {
			GameManager.getInstance().AddInventory (InventoryKey, countInventory, null);
		} else {
			GameManager.getInstance().CollectPickUp (type, pickupValue,null);
		}
		//GameObjectUtility.

		// choice to delay on destoy to let snd and partical
		// effect run it course, or let the global sound manager 
		// handle the sound and set the parent of the particall effect to
		// some persisten object on scene, like game manager
        
		Destroy (this.gameObject);
		 
	}

	private void HandleEffect(){

		PlaySound (sfx);

		if (PickUParticalEffect) {
			Instantiate (PickUParticalEffect, transform.position, transform.rotation);
		}

	}
 
	void PlaySound(AudioClip clip)
	{
		try {
		if (clip) {
            SoundManager.getInstance ().PlaySoundOnce (clip,.25f);
            
		}
		}catch(System.Exception e ){

		}
	}
}
