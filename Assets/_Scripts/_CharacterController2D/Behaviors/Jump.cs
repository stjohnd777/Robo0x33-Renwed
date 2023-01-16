using UnityEngine;
using System.Collections;
#if UNITY_EDITOR 
using UnityEditor;
#endif
public class Jump : AbstractBehaviour
{


	public float JumpSpeed = 200f;

	public bool useforce = false;

	public float jumpForce;

	public int JumpCount = 0;

	public int MaxJumps = 2;

	public float DelayBetweenJumps = .1f;

	public bool IsJumpEnabled = true;

	public GameObject dustEffectPrefab;

	public Transform spwanEffect;

	void Update () {
	
		if (!IsJumpEnabled) {
			return;
		}

		bool jumpRequested = IsRequestingBehavior();
        //inputState.GetButtonValue (inputsButtons [0]);

        float jumpButtonHoldTime = inputState.GetButtonHoldTime (inputsButtons[0]);

		if (inputState.IsGrounded) {
			JumpCount = 0;
		}

		if( inputState.IsGrounded ||  CanDoubleJump()  ){
 
			if (jumpRequested) {
				OnJump ();
			}
		}

	}

	public bool CanJump(){
		bool ret = false;
		if (inputState.IsGrounded ||  CanDoubleJump ()) {
			ret = true;
		}
		return ret;
	}

	protected bool CanDoubleJump(){
		return JumpCount < MaxJumps;
	}

	protected virtual void OnJump(){ 
		if (!inputState.IsGrounded) {
			if ( dustEffectPrefab){
				GameObject clone = Instantiate (dustEffectPrefab);
				if (spwanEffect) {
					clone.transform.position = spwanEffect.position;
				} else {
					clone.transform.position = transform.position;
				}
			}
		}
		JumpCount++;
		StartCoroutine (DisableJump(DelayBetweenJumps));
		if (useforce) {
			body2d.velocity = new Vector2 (body2d.velocity.x, 0f );   
			// add a force in the up direction
			body2d.AddForce (new Vector2 (0, jumpForce));
 
		} else {
			body2d.velocity = new Vector2 (body2d.velocity.x, JumpSpeed);
		}
	}

	IEnumerator DisableJump(float disableTime){
		IsJumpEnabled = false;
		yield return new WaitForSeconds (disableTime);
		IsJumpEnabled = true;
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(Jump))]
class JumpEditor : AbstractBehaviorEditor
{
    Jump jump;

    public void OnEnable()
    {
        base.OnEnable();
        jump = target as Jump;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawDefaultInspector();

        serializedObject.Update();
    }
}
#endif
