using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnLocaton : MonoBehaviour {

	public Transform spawn;

	//public static Vector3 respawn;
 
	// Attack player
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player") {

			//respawn = spawn.position;

			// can not use reference to position need clone as we=hne the scene reloads 
			// the reference will be invalid if the refernce lives on a game object in the scene that
			// has been destroyed
			GameManager.playerSpawnLocation = new Vector3(spawn.position.x,spawn.position.y,spawn.position.z); 
		}
	}

}
