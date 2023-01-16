﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[System.Serializable]
//public enum AnimationType {
//	Idel, Floating
//}
//
//[System.Serializable]
//public class AnimParam {
//	public bool 		  fireTrigger;
//	public string 		  AnimationName;
//}
// 

public class AgentState : MonoBehaviour {

	[Tooltip("GameObject this script is managing state for")]
	public GameObject target;

	[Header("Agent State")]
	public bool  IsGrounded;
	public float SpeedH;
	public float SpeedV;
	public bool  IsShooting;
	public int   ShootModifier;
	public int   Mele;
	public bool  IsThrowing;
	public float MeleIsInRange;
	public bool IsHurt;
	public bool IsDead;

	[Tooltip("If true sets Animation Triggers")]
	public bool IsDirty;

	public string defaultAnimationState = "Idel";

	[Tooltip("Animation Triggers")]
	public AnimParam[] p;



	private Animator npcAnimationController;

	private GroundProbeCircleOverlaps groundProbe;

	private Rigidbody2D body;


	void Awake(){

		npcAnimationController = GetComponent<Animator>();

		body = target.GetComponent<Rigidbody2D>();

		groundProbe = GetComponent<GroundProbeCircleOverlaps>();

		//wayPoints = GetComponent<ActionWaypointMovement> ();

		StartCoroutine(FireTrigger());
	}
	 


	void Update () 
	{
 
  		SpeedH = body.velocity.x;
		npcAnimationController.SetFloat ("HVelocity", SpeedH);

		//npcAnimationController.SetFloat("AbsHVelocity",Mathf.Abs(SpeedH));

		SpeedV =  body.velocity.y;
		npcAnimationController.SetFloat ("VVelocity",SpeedV);

		IsGrounded = groundProbe.IsOnGround();

		npcAnimationController.SetBool ("IsGrounded", IsGrounded);

		npcAnimationController.SetBool ("IsShooting", IsShooting);

		npcAnimationController.SetBool ("IsThrowing", IsThrowing);


		if( IsDirty ){
			StartCoroutine(FireTrigger());
		}
	}

	private IEnumerator FireTrigger(){

		foreach( AnimParam anP in p){
			if(  anP.fireTrigger){
				npcAnimationController.SetTrigger(anP.AnimationName);
				anP.fireTrigger = false;
			}
		}

		yield return new WaitForSeconds(.01f);

		IsDirty = false;

		StartCoroutine(ReturnToDefaultAnimationStateInDelay(1));
	}

	private IEnumerator ReturnToDefaultAnimationStateInDelay(float delay){

		yield return new WaitForSeconds(delay );

		npcAnimationController.SetTrigger (defaultAnimationState);
	}


	public void TriggerAnimation(string trigger, bool recover = true, float delayOnRecover = 1) {
		npcAnimationController.SetTrigger(trigger);
		if (recover) {
			StartCoroutine (ReturnToDefaultAnimationStateInDelay (delayOnRecover));
		}
	}

	public void ShootAnimation(int type){

	}

	public void MeleAnimation(int type){

	}
	public void HurtAnimation(){

	}
	public void DieAnimation(){

	}

}
