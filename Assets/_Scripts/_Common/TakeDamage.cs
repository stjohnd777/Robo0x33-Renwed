using UnityEngine;
using System.Collections;

/*
 * host of this script takes damage, delegates to health script
*/
public class TakeDamage : MonoBehaviour, ITakeDamage {

	public string[] takesDamageFormTags;

	Health health;

	void Awake() {
		 health = GetComponentInParent<Health>();
	}


	// Use this for initialization
	void Start () {
	
	}

	bool IsHostileTag(string tag){
		bool ret = false;
		foreach( string prejudiceTag in takesDamageFormTags){
			if ( tag.Equals( prejudiceTag)){
				ret = true;
				return ret;
			}
		}
		return ret;
	}

	// if Player hits the stun point of the enemy, then call Stunned on the enemy
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (IsHostileTag( collision.gameObject.tag ) )
		{
			GiveDamage _GiveDamage = collision.gameObject.GetComponent<GiveDamage>();
			if ( health != null && _GiveDamage!=null){
				health.ApplyDamage(_GiveDamage.damage);
			}
		 
		}

	}

	// Attack player
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (IsHostileTag( collision.gameObject.tag ) )
		{
			GiveDamage _GiveDamage = collision.gameObject.GetComponent<GiveDamage>();
			int damage = _GiveDamage.damage;
			ApplyDamage(damage);
		}
	}

	// just delegate to the IHealth
	public void ApplyDamage (int damage) {
		if ( health != null){
			//FloatingTextController.CreateFloatingText("-"+damage,transform);
			health.ApplyDamage(damage);
		}
	}


}
