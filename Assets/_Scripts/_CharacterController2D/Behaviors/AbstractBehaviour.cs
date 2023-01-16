#if UNITY_EDITOR 
using UnityEditor;
#endif
 
using UnityEngine;
using System.Collections;

[System.Serializable]
public enum InputMode{
	KeyCodeDown, // fires once on the frame that a specified key is pressed
	KeyCodeUp,   // fires once on the frame that a specified key is release
	KeyCodeHold, // fires while the key is pressed
	Axis, 		 // analog reading 
}

[System.Serializable]
public enum InputType{
    KeyCode,
	InputManager,
	AxisInputManager
}

public abstract class AbstractBehaviour: MonoBehaviour {

	[Space]
	public InputType inputType;
	public InputMode inputMode;
    [Space]

	[HideInInspector]
    public KeyCode _key;

    [HideInInspector]
    public KeyCode _key2;
    [Space]

    
    [HideInInspector]
	public string axisNameInputManager;
    [Space]

	[HideInInspector]
	public CommandRequestVerbs[] inputsButtons;


    public MonoBehaviour[] behaviorsToDisable;
    [Space]


    protected bool isRequestingBehavior;

	protected Rigidbody2D body2d;

    protected State inputState;

	protected virtual void Awake(){
		inputState = GetComponent<State> ();
		body2d = GetComponent<Rigidbody2D> ();
	}
 
    protected bool IsRequestingBehavior()
    {
        isRequestingBehavior = false;
        switch (inputType)
        {
            case InputType.KeyCode:

	            switch (inputMode)
	            {
		            case InputMode.KeyCodeDown:
			            if (Input.GetKeyDown(_key) )
			            {
				            isRequestingBehavior = true;
			            }
			            else if(Input.GetKeyDown(_key2) )
			            {
				            isRequestingBehavior = true;
			            }
			            else if (Input.GetKeyUp(_key))
			            {
				            isRequestingBehavior = false;
			            }
			            break; 
		            case InputMode.KeyCodeUp: 
			            if (Input.GetKeyUp(_key) )
			            {
				            isRequestingBehavior = true;
			            }
			            else if(Input.GetKeyUp(_key2) )
			            {
				            isRequestingBehavior = true;
			            }
			            else if (Input.GetKeyUp(_key))
			            {
				            isRequestingBehavior = false;
			            }
			            break;
	            }
	            break;
            
            case InputType.AxisInputManager:
                isRequestingBehavior = Input.GetAxis(axisNameInputManager) != 0;
                break;
            case InputType.InputManager:
                isRequestingBehavior = inputState.GetButtonValue(inputsButtons[0]);
                break;

            default:
                break;
        }
        return isRequestingBehavior;
    }
	protected virtual void SetRelatedBehaviorsToActive(bool value){

		foreach (MonoBehaviour aBehavior in behaviorsToDisable) {
			aBehavior.enabled = value;
		}
	}


	protected void DisableRelatedBehaviors(){

		foreach (MonoBehaviour aBehavior in behaviorsToDisable) {
			aBehavior.enabled = false;
		}
	}

	protected void EnableRelatedBehaviors(){

		foreach (MonoBehaviour aBehavior in behaviorsToDisable) {
			aBehavior.enabled = true;
		}
	}
}


#if UNITY_EDITOR
[CustomEditor(typeof(AbstractBehaviour))]
class AbstractBehaviorEditor : Editor{

	AbstractBehaviour ab;

	CommandRequestVerbs v;

	public void OnEnable()
	{
		ab = target as AbstractBehaviour;
	}

	public override void OnInspectorGUI()
	{
		//DrawDefaultInspector();

		serializedObject.Update();

		//ab.inputType = (InputType)EditorGUILayout.EnumPopup("Input Type",ab.inputType);

		InputType inputType = ab.inputType;

		switch (inputType)
		{
		case InputType.KeyCode:
			ab._key = (KeyCode)EditorGUILayout.EnumPopup("KeyCode",ab._key);
            ab._key2 = (KeyCode)EditorGUILayout.EnumPopup("KeyCode2", ab._key2);
            break;
		case InputType.AxisInputManager:
			ab.axisNameInputManager = EditorGUILayout.TextField("AxisName" ,ab.axisNameInputManager);	
			break;
		case InputType.InputManager:
			v = (CommandRequestVerbs) EditorGUILayout.EnumMaskField("Verbs" ,v);	
			break;

		default:
			break;
		}

    }
}
#endif

