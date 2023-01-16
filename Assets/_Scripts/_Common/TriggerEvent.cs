using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
	[Header("Deals Damage")] private bool dealsDamage = true;
	[Header("Apply Damage")] public int damage = 1;
	[Header("Is DOT")] public bool isDOT;
	[Header("DOT Interval in Sec")] private float perDamageSecond = .5f;
	
	[Header("Deals Healing")] private bool dealsHealing = false;
	[Header("Apply Heal")] public int heal = 0;
	[Header("DOT Interval in Sec")] private float perHealSecond = .5f;

	[Tooltip("Tags that trigger event")]
	public string[] prejudiceTagList;

	public bool disable = false;

	[Tooltip("This trigger is Enables")]
	public bool IsOnTrigger = true;
	
	[Header("Activate Deal Damage on:")]
	[Tooltip("Use collision to initiate give damage event")]
	public bool IsOnCollision2D = false;

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

	void OnCollisionEnter2D(Collision2D collision)
	{
		if ( IsInPrejudiceTagList (collision.gameObject.tag) && IsOnCollision2D && !disable) {
			
		}
	}
	
	void OnTriggerEnter2D(Collider2D collision)
	{
 		GameObject go = collision.gameObject;
		int otherLayer = collision.gameObject.layer;
		string otherTag = collision.gameObject.tag;
		if ( IsInPrejudiceTagList (otherTag)  && IsOnTrigger  && !disable)
		{
			ITakeDamage giveDamage = go.GetComponent<ITakeDamage>();
			giveDamage.ApplyDamage(damage);
		} 
	}
}
