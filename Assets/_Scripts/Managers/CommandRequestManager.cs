using UnityEngine;
using System.Collections;

/*
 * Anything the player can request on character
 */
public enum CommandRequestVerbs {
	RequestMoveRight,
	RequestMoveLeft,
    RequestWalk,
    
	RequestRun,
	RequestJump,
	RequestSprint,
	
	RequestCrouch, 
	RequestSlide,
	RequestRoll,
	
	RequestSneek,
	
	RequestInteract,
	
	RequestSlash,
	RequestStab,
	RequestProjectile,
	
	RequestFire1,
	RequestFire2,
	RequestFire3,
	
	RequestMele1,
	RequestMele2,
	RequestMele3,
	
	RequestThrow,
	
	RequestDefend,
	
	RequestDashGround,
	RequestDashAir,
	RequestPowerJump
}
/*
 * Condition such as KeyDown, 
 * KeyUp/Down + Hold  (like cook the grenade or charge the jump)
 */
public enum CommandRequestCondition {
	GreaterThan,
	LessThan,

	KeyUp,
	KeyDown,

	KeyUpWithHoldTime,
	KeyDownWithHoldTime
}

/*
 * In short this simply read an axis defined and managed by
 * the unity input manager.
 *
 * The value condition
 */ 
[System.Serializable]
public class CommandRequestState {

	[Header ("Input")]
	[Tooltip ("Axis Name (ie. horizontal, ... fire1) defined in Unity-InputManager") ] 
	public string axisNameUnityInputManager;

	[Tooltip ("Analog Activation Threshold  > or < is Activated") ]
	public float offValue = 0f;

	[Tooltip ("Condition on Input") ]
	public CommandRequestCondition condition;
	public float holdTimeKeyDown = 0;

	[Tooltip("Verbs ie. Jump, Defend, Crouch")]
	public CommandRequestVerbs verb;

	[Tooltip ("Description") ] 
	public string description;

	public bool value {
		get{
			float val = Input.GetAxis(axisNameUnityInputManager);
			switch(condition) {
			case CommandRequestCondition.GreaterThan:
				return val > offValue;
			case CommandRequestCondition.LessThan:
				return val < offValue;
			}
			return false;
		}
	}

	bool IsRequestingCommand(){
		return this.value;
	}
}

/*
 * Continuously queries over the interested InputAxis
 * and update the Input state
 */
public class CommandRequestManager : MonoBehaviour {

	// all the commands you are listening for
	[Header ("What Input activates the Verb")]
	public CommandRequestState[] inputs;
 
	[Header ("Character State Script")]
	public State state;

	void Update () {
		foreach( var input in inputs){
			state.SetButtonValue (input.verb,input.value);
		}
	}
}
