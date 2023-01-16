using UnityEngine;
using System.Collections;


public class IsInventory : MonoBehaviour {


	public bool IsActive;

	public string InventoryKey;

	public int ScoreValue = 0;

	public bool taken = false;

	public GameObject explosion;

	public AudioClip sfx;

	// if the player touches the coin, it has not already been taken, and the player can move (not dead or victory)
	// then take the coin
	void OnTriggerEnter2D (Collider2D other)
	{
		if ((other.tag == "Player" ) && (!taken) && (other.gameObject.GetComponent<CharacterController2D>().playerCanMove))
		{

			// mark as taken so doesn't get taken multiple times
			taken=true;

			// if explosion prefab is provide, then instantiate it
			if (explosion)
			{
				Instantiate(explosion,transform.position,transform.rotation);
			}

			// do the player collect coin thing
			other.gameObject.GetComponent<CharacterController2D>().AddInventory(InventoryKey,1,sfx);
			 
			// add to score
			// TODO:

			// destroy the coin
			DestroyObject(this.gameObject);
		}
	}


}
