using UnityEngine;
using System.Collections;

public enum AnimState {

    Idel = 0,

	Run = 1,
	Crouch = 2,

	CrouchSlash = 20,
	CrouchRun = 21,
	CrouchDefend = 22,
	CrouchHurt = 23,

	Defending = 3,
	DefendingRun = 30,

    Slash = 4,
	SlashAir = 40,

	Stab = 5,

    Hurt = -1,
    Die = -99,

}

 

public class PlayerManager : MonoBehaviour {

	private State state;
	private Walk walkBehavior;
	private Animator animator;
	
	void Awake () {
	
		state = GetComponent<State> ();
		walkBehavior = GetComponent<Walk> ();
		animator = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		
		animator.SetBool  ("IsGrounded", state.IsGrounded);
		animator.SetFloat ("HVelocity", state.HVelocity);
		animator.SetFloat ("VVelocity", state.VVelocity);

		if ( !state.IsGrounded){
			// in the air

			if (state.IsSlashing){
			
				ChangeAnimationState ( (int)AnimState.SlashAir);
			}
				

		} else {

			// on the ground

			if (state.AbsHVelocity > 0 ) {
				
				//moving, walking or running
			
				animator.speed = walkBehavior.Running ? walkBehavior.HorizontalSpeedMultiplier : 1;

				ChangeAnimationState ((int)AnimState.Run);

//				if (state.IsCrouching)  {
//					ChangeAnimationState ((int)AnimState.CrouchRun);
//				}

 
			} else {
				
				// not moving
				ChangeAnimationState ( (int)AnimState.Idel);

				if (state.IsCrouching && !state.IsSlashing)  {
					ChangeAnimationState ( (int)AnimState.Crouch);
				}

				if (state.IsCrouching && state.IsSlashing)  {
				    ChangeAnimationState ( (int)AnimState.CrouchSlash);
				}

				if (state.IsSlashing){
					ChangeAnimationState ( (int)AnimState.Slash);
				}

				if (state.IsStabbing){
					ChangeAnimationState ( (int)AnimState.Stab);
				}

				if (state.IsRequestingDefence ) {
					ChangeAnimationState ((int)AnimState.Defending);
				}
					
			}
				
		}
        
//		if (state.AbsHVelocity == 0) {
//			ChangeAnimationState ( (int)AnimState.Idel);
//		}
//
//		if (state.AbsHVelocity > 0 ) {
//			ChangeAnimationState ( (int)AnimState.Run );
//		}
//
//		if (state.GetButtonValue(CharacterVerbs.Crouch) ) {
//			ChangeAnimationState ((int)AnimState.Crouch);
//		}
//			
//		animator.speed = walkBehavior.Running ? walkBehavior.HorizontalSpeedMultiplier : 1;
//
//		//animator.SetBool ("IsFire1",state.GetButtonValue(CharacterVerbs.Fire1));
//		if (state.GetButtonValue(CharacterVerbs.Slash) ) {
//			ChangeAnimationState ((int)AnimState.Slash);
//		}
//
//		//animator.SetBool ("IsFire2",state.GetButtonValue(CharacterVerbs.Fire2));
//		if (state.GetButtonValue(CharacterVerbs.Stab) ) {
//			ChangeAnimationState ((int)AnimState.Stab);
//		}
//
//
//		if (state.GetButtonValue(CharacterVerbs.Projectile) ) {
//			ChangeAnimationState ((int)AnimState.Stab);
//		}
//		 
// 
//		if (state.IsDefending ) {
//			ChangeAnimationState ((int)AnimState.Defending);
//		}

        animator.SetFloat("Health", state.health);
        if (state.health <= 0)
        {
            ChangeAnimationState((int)AnimState.Die);
        }

        //		animator.SetBool ("IsMele1",inputState.GetButtonValue(Buttons.Mele1));
        //		animator.SetBool ("IsMele2",inputState.GetButtonValue(Buttons.Mele2));
        //		animator.SetBool ("IsMele3",inputState.GetButtonValue(Buttons.Mele3));



    }

	void ChangeAnimationState(int value){
		animator.SetInteger ("AnimState", value);
	}
}
