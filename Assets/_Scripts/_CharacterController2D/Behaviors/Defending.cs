using UnityEngine;
using System.Collections;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public class Defending : AbstractBehaviour
{

    public bool flag;
    public int i = 1;

    public GameObject theShield;

	public int MaximunShieldPoint;

	public int AvaliableShieldPoint;

	public int ShieldPointIdelDrainPerSecound;

	public float ShieldPointReChargePerSecWhileDown = .01f;

	public bool IsGrantImunityWhileActive = false;

	public float ShieldAbsorbsPercentageDamageDealthToChacter = .5f;

	public float ShieldPointsDrainedByDamageDeflected = .4f;



	public bool IsDefending;

	public virtual void OnDefend(bool value){

		if( value){

			theShield.SetActive(true);
			// raise shield, activate the 
			// shield collider 
		}else {
			theShield.SetActive(false);
			// lower shield, de-activate the 
			// shield collider 
		}
			
	}

	void Update(){

		if (IsRequestingBehavior()) {
			IsDefending = true;
		} else {
			IsDefending = false;
		}
		OnDefend(IsDefending);
		SetRelatedBehaviorsToActive (!IsDefending);
	}
}


#if UNITY_EDITOR
[CustomEditor(typeof(Defending))]
class DefendingEditor : AbstractBehaviorEditor
{
    Defending defend;

    public void OnEnable()
    {
        base.OnEnable();
        defend = target as Defending;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawDefaultInspector();

        serializedObject.Update();
    }
}
#endif