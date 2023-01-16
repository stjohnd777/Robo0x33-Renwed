using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAnimationBridge : MonoBehaviour {

	private State state;
	private Animator animator;
    private IHealth health;
    private Walk walkBehavior;

    void Start () {
		state = GetComponent<State> ();
		animator = GetComponent<Animator> ();
        walkBehavior = GetComponent<Walk>(); ;
    }
	
	// Update is called once per frame
	void Update () {

        animator.speed = walkBehavior.Running ? walkBehavior.HorizontalSpeedMultiplier : 1;

        animator.SetBool ("IsGrounded", state.IsGrounded);
		animator.SetFloat ("HInput", state.HInput);
		animator.SetFloat ("AbsHInput", state.AbsHInput);

		animator.SetFloat ("AbsHVelocity", state.AbsHVelocity);
		animator.SetFloat ("HVelocity", state.HVelocity);
		animator.SetFloat ("VVelocity", state.VVelocity);
		animator.SetBool ("IsSquat", state.IsCrouching);
		animator.SetBool ("IsSliding", state.IsSlide);

		animator.SetBool ("IsSlashing", state.IsSlashing);
		animator.SetBool ("IsStabbing", state.IsStabbing);
		animator.SetBool ("IsShooting", state.IsShooting);

		animator.SetBool ("IsFire1", state.IsFire1);
		animator.SetBool ("IsFire2", state.IsFire2);
		animator.SetBool ("IsFire3", state.IsFire3);


		animator.SetBool ("IsDefending", state.IsRequestingDefence);
		animator.SetBool ("IsThrowing", state.IsThowing);
		animator.SetFloat ("Health", state.health);
	}
}
