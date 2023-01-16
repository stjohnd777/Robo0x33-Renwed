using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public class Throw : AbstractBehaviour {


	public bool IsRequestingThrow;

	private bool lastRead = false;

	public GameObject spawnObjectAt;

	public GameObject objectThrown;

	public Vector3 forceThrow;

	public int inventory = 3;

    private State state;

    private void Awake()  
	{
		base.Awake();

        if (state == null)
        {
            state = GetComponent<State>();
        }
    }

    float delay = 1.0f;

	bool canThrow = true;

	void Update () {

		if (!canThrow) {
			return;
		}

        //IsRequestingThrow = inputState.GetButtonValue (inputsButtons [0]);
        //IsRequestingThrow = Input.GetAxis("Throw") != 0;
        IsRequestingThrow = IsRequestingBehavior();


        if (IsRequestingThrow && inventory > 0 ) {
			
			canThrow = false;
			inventory--;
   
			SetRelatedBehaviorsToActive (false);

			IFacingRight isRight = GetComponentInParent<IFacingRight> ();
			float dirX = 1;
			if (!isRight.IsFacingRight ()) {
				dirX = -1;
			}
			GameObject aGernade = sillymutts.GameObjectUtility.Instantiate (objectThrown, spawnObjectAt.transform.position);
			aGernade.GetComponent<Rigidbody2D> ().AddForce (new Vector3(dirX*forceThrow.x,forceThrow.y,forceThrow.z));


            if (state != null)
            {
                state.IsThowing = true;
            }

            StartCoroutine (StartDelay(delay));
 
		} else {

			SetRelatedBehaviorsToActive (true);

            if (state != null)
            {
                state.IsThowing = true;
            }
        }
 
	}

	IEnumerator StartDelay(float deley){

	    yield return new WaitForSeconds(delay);

		canThrow = true;

	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(Throw))]
class ThrowEditor : AbstractBehaviorEditor
{
    Throw slashAttack;

    public void OnEnable()
    {
        base.OnEnable();
        slashAttack = target as Throw;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawDefaultInspector();

        serializedObject.Update();
    }
}
#endif
