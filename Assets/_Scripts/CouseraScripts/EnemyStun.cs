using UnityEngine;
using System.Collections;

public class EnemyStun : MonoBehaviour {

	// if Player hits the stun point of the enemy, then call Stunned on the enemy
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			// tell the enemy to be stunned
			//this.GetComponentInParent<Enemy>().Stunned();
			this.GetComponentInParent<IStunnable>().Stunned();

			// make the player bounce of the player

			other.gameObject.GetComponent<CharacterController2D>().EnemyBounce();

			//other.gameObject.GetComponent<Rigidbody2D>().AddForce (new Vector2 (0, 10000));
		}
	}
}
