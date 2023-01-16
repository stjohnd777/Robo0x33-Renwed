using UnityEngine;
using System.Collections;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public class Crouch : AbstractBehaviour
{

	public float scale = .5f;

	public bool IsCrouching;

	public float CenterOffsetY = 0f;

	private BoxCollider2D collisionBox;

	private Vector2 originalCenter;

    private State state;

    protected override void Awake(){

		base.Awake ();

		collisionBox = GetComponent<BoxCollider2D> ();

        state = GetComponent<State>();
    }

	public virtual void OnCrouch(bool value){
		IsCrouching = value;
	}

	void Update(){

        bool IsRequesting = IsRequestingBehavior();

        if (IsRequesting) {

            IsCrouching = true;

            if (state != null)
            {
                state.IsCrouching = true;
            }

        } else {

			IsCrouching = false;

            if (state != null)
            {
                state.IsCrouching = false;
            }
        }
		SetRelatedBehaviorsToActive (!IsCrouching);
	}

}

#if UNITY_EDITOR
[CustomEditor(typeof(Crouch))]
class CrouchEditor : AbstractBehaviorEditor
{
    Crouch crouch;

    public void OnEnable()
    {
        base.OnEnable();
        crouch = target as Crouch;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawDefaultInspector();

        serializedObject.Update();
    }
}
#endif