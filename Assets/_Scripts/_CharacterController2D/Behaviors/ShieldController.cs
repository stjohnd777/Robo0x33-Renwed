#if UNITY_EDITOR 
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System;


[SerializeField]
public enum HostTakeDamageBehavior
{
    NoDamage, PercentDamage, SelectiveDamage
}

[SerializeField]
public enum ShieldTakeDamageBehavior
{
    NoDamage, PercentDamage, SelectiveDamage
}
public class ShieldController : AbstractBehaviour, IHealth
{
    [Space]
    [Tooltip("The GameOject hosting the shield")]
	public GameObject theHostObject;
	[Tooltip("The Shield Object, Typically a shpere or circle")]
	public GameObject theShieldObject;
    [Space]


    [HideInInspector]
	public float MaximunShieldPoint;
    [HideInInspector]
    public float CurrentShieldPoint ;
    [HideInInspector]
    public float IdleDrainPerSec = 10;
    [HideInInspector]
    public float IdleChargePerSec = .01f;
    [HideInInspector]
    public int  RechargeDelayInterval = 5;
 
     [HideInInspector]
	 public HostTakeDamageBehavior hostTakeDamageBehavior = HostTakeDamageBehavior.NoDamage;
         [HideInInspector]
         public float ShieldDelegatesPercentageDamageToHost = .5f;

     [HideInInspector]
     public  ShieldTakeDamageBehavior shieldTakeDamageBehavior = ShieldTakeDamageBehavior.NoDamage;
         [HideInInspector]
         public float ShieldPointsReducedPercentDamageRecieved = .4f;

     [HideInInspector]
	 public bool ShieldGivesDamage = true;
         [HideInInspector]
         public int  shieldGivesDamageAmount = 1000;


	protected bool IsRequestingShieldUp;
    protected bool IsShieldUp = false;
	private IHealth hostHealth;

	override protected void Awake()
	{
		base.Awake();
		hostHealth = theHostObject.GetComponent<IHealth>();
	}


	// Unfornately the GiveDamage script does finds the IHealth on this Shield script
	// but GetComonentInParent can find the IHealth component on the parrent. The
	// and parents ApplyDamage is not the correct OO model.
	//
	// The collider between the object carring the GiveDamage Script and the Shield object if correct 
	// by looking at the tag in the debugger, this collision between the objects is correctly identified.
	// One would thing that Getting the Component would return this Script
	// since it does implement the IHealth. The GetComponent call return null, ending the story.
	public void ApplyDamage(int damage){
		ApplyDamageToShield( damage);
		ApplyDamageToPlayer (damage);
	}

	public int GetWeightedDamagePassedToPlayer( int damage){
		int weightedDamage =  (int)  ( (float) damage *  ShieldDelegatesPercentageDamageToHost );
		return weightedDamage;
	}

	public int GetWeightedDamagePassedToShield( int damage){
		int weightedDamageToShield = (int) (damage * ShieldPointsReducedPercentDamageRecieved);
		return weightedDamageToShield;
	}

	public void ApplyDamageToShield(int damage){
		int weightedDamageToShield = GetWeightedDamagePassedToShield(damage);
		FadingTextFactory.CreateFloatingTextDamage("Shield Damaged " + weightedDamageToShield, transform);
		CurrentShieldPoint =  CurrentShieldPoint-weightedDamageToShield;
		if (CurrentShieldPoint < 0) {
			CurrentShieldPoint = 0;
		}
	}

	public void ApplyDamageToPlayer(int damage){
	   damage = GetWeightedDamagePassedToPlayer(  damage);
	   hostHealth.ApplyDamage (damage);
	}

	public void ApplyHeal(int heal){
		CurrentShieldPoint = CurrentShieldPoint+ heal;
	}

	public int GetHealth(){
		return (int)CurrentShieldPoint;
	}

	public float GetHealthPercent(){
		return (int)CurrentShieldPoint;
	}

 

	public void SetImmunityForSec(float sec){
		
	}

 
		
	/**
	 * Will respond by raising the shield if possible. If the shield is raised then
	 * method will return true, otheerwise false. The public IsShieldUp property
	 * is set for code quering if shield is up.
	 * 
	 * TODO : Create a Shield interface when the shield mechanic is more mature

	 */
    protected bool HandleRequestShieldUp(bool IsRequestingShieldUp){
		
		if( CanActivateShield() ) {
            //hostHealth.SetIsImmunityFromDamage(true);
			ActivateShield ();
			ApplyIdleDrain ();
			IsShieldUp = true;
		} else {
			DeActivateShield ();
			ApplyIdleCharge ();
        }
		return IsShieldUp;
			
	}

	bool CanActivateShield(){
		return (IsRequestingShieldUp && CurrentShieldPoint > 0); 
	}

    void ActivateShield(){
		IsShieldUp = true;
		theShieldObject.SetActive(true);
	}

	void DeActivateShield(){
		IsShieldUp = false;
		theShieldObject.SetActive(false);
	}

    public bool GetIsShieldUp(){
		return IsShieldUp;
	}

	int ApplyIdleDrain(){
        if (IsShieldUp)
        {
            CurrentShieldPoint = CurrentShieldPoint - Time.deltaTime * IdleDrainPerSec;
            if (CurrentShieldPoint < 0)
            {
                CurrentShieldPoint = 0;
            }
        }
		return (int)CurrentShieldPoint;
	}

    bool ShieldIsCoolingDown = true;

    int ApplyIdleCharge(){
		
        if (ShieldIsCoolingDown)
        {
            return (int)CurrentShieldPoint; ;
        }
		if (ShieldIsCoolingDown) {
			CurrentShieldPoint = 0;

		} else {
			CurrentShieldPoint = CurrentShieldPoint + Time.deltaTime * IdleChargePerSec;
			if (CurrentShieldPoint > MaximunShieldPoint) {
				CurrentShieldPoint = MaximunShieldPoint;
			}
		}
		return (int)CurrentShieldPoint;
	}

	IEnumerator CoolDown( int secCoolDown){
		ShieldIsCoolingDown = true;
		yield return new WaitForSeconds(secCoolDown);
		CurrentShieldPoint = 1;
		ShieldIsCoolingDown = false;
	}
	protected bool isKeyDown  = false;
	protected float keyDownTime = 0;
	protected float inputAxisReading;

	void Update(){
 
		if (CurrentShieldPoint == 0){ // && ShieldIsCoolingDown == false ) {
            DeActivateShield();
			ShieldIsCoolingDown = true;
			StartCoroutine (CoolDown(RechargeDelayInterval));
		}
		// Abstraction Need in BaseClass Input Read Result
		// in the Base class to swith between the diffent 
		// modes on setting the input for the game mechanic

		// use raw key that is explicitly tied to
		// shield mechanic
		if (InputType.KeyCode ==  inputType){
			
			//isKeyDown = Input.GetKeyDown(_key);
			//inputAxisReading = Input.GetAxis(axisNameInputManager);
			//isKeyDown = Input.GetKey(_key);
		    //isKeyDown = CrossPlatformInputManager.GetButtonDown("Defending");
			//Debug.Log("Q:" +isKeyDown);

			if ( Input.GetKeyDown(_key)){
				IsRequestingShieldUp = true;
			} else if (Input.GetKeyUp(_key)){
				keyDownTime = 0;
				IsRequestingShieldUp = false;
			}

			if (IsRequestingShieldUp) {
				keyDownTime = keyDownTime + Time.deltaTime;
			}
			//bool isKeyUp = Input.GetKeyUp(_key);

		} // use the input manager this is autamitally bound to
		// the unity input manager in setting
		else if (InputType.InputManager ==  inputType) {
			
			if (inputState.IsRequestingDefence) {
				IsRequestingShieldUp = true;

			} else {
				IsRequestingShieldUp = false;
			}
		}
		else if (InputType.AxisInputManager ==  inputType){
		}

		SetRelatedBehaviorsToActive (!IsRequestingShieldUp);
		HandleRequestShieldUp(IsRequestingShieldUp);
	}



    public void SetIsImmunityFromDamage(bool isImune)
    {
        throw new NotImplementedException();
    }

    public bool GetHasImmunity()
    {
        throw new NotImplementedException();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ShieldController))]
class ShieldEditor : AbstractBehaviorEditor //Editor
{
    ShieldController shieldController;

	public void OnEnable() 
    {
		base.OnEnable();
        shieldController = target as ShieldController;
    }

    public override void OnInspectorGUI()
    {
		base.OnInspectorGUI();

		DrawDefaultInspector();

        serializedObject.Update();


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Strength, Drain, Charge, Cool Down");
        EditorGUI.indentLevel++;
        shieldController.MaximunShieldPoint =  EditorGUILayout.FloatField("Max Shield Point", shieldController.MaximunShieldPoint);
        shieldController.CurrentShieldPoint = EditorGUILayout.FloatField("Current Shield Points", shieldController.CurrentShieldPoint);
        shieldController.IdleDrainPerSec =  EditorGUILayout.FloatField("Idle Drain points/sec", shieldController.IdleDrainPerSec);
        shieldController.IdleChargePerSec =  EditorGUILayout.FloatField("Idel Charge points/sec", shieldController.IdleChargePerSec);
        shieldController.RechargeDelayInterval =  EditorGUILayout.IntField("Cool Down Sec", shieldController.RechargeDelayInterval);
        EditorGUI.indentLevel--;
        EditorGUILayout.Space();

        shieldController.hostTakeDamageBehavior =  (HostTakeDamageBehavior) EditorGUILayout.EnumPopup("Host Damage Behavior", shieldController.hostTakeDamageBehavior);
        if (shieldController.hostTakeDamageBehavior == HostTakeDamageBehavior.NoDamage)
        {
            //EditorGUILayout.LabelField("Host Does Not Take Damage");
            EditorGUI.indentLevel++;
            EditorGUILayout.HelpBox("No configuration Needed", MessageType.Info);
            EditorGUI.indentLevel--;
        }
        else if (shieldController.hostTakeDamageBehavior == HostTakeDamageBehavior.PercentDamage)
        {
            //EditorGUILayout.LabelField("Host Does Take Damage % Damage");
            EditorGUI.indentLevel++;
            shieldController.ShieldDelegatesPercentageDamageToHost = EditorGUILayout.FloatField("% Damage Delegated to Host", shieldController.ShieldDelegatesPercentageDamageToHost);
            EditorGUI.indentLevel--;
        }
        else if (shieldController.hostTakeDamageBehavior == HostTakeDamageBehavior.SelectiveDamage)
        {

        }

        shieldController.shieldTakeDamageBehavior =  (ShieldTakeDamageBehavior)EditorGUILayout.EnumPopup("Shield Damage Behavior", shieldController.shieldTakeDamageBehavior);
        if (shieldController.shieldTakeDamageBehavior == ShieldTakeDamageBehavior.NoDamage)
        {
            //EditorGUILayout.LabelField("Shield Does Take Damage");
            EditorGUI.indentLevel++;
            EditorGUILayout.HelpBox("No Configuration Needed", MessageType.Info);
            EditorGUI.indentLevel--;
        } else if (shieldController.shieldTakeDamageBehavior == ShieldTakeDamageBehavior.PercentDamage)
        {
            //EditorGUILayout.LabelField("Shield Does Take Damage % Damage");
            EditorGUI.indentLevel++;
            shieldController.ShieldPointsReducedPercentDamageRecieved = EditorGUILayout.FloatField("% Damage Passed to Shield", shieldController.ShieldPointsReducedPercentDamageRecieved);
            EditorGUI.indentLevel--;
        } else if (shieldController.shieldTakeDamageBehavior == ShieldTakeDamageBehavior.SelectiveDamage)
        {

        }

        //shieldController.ShieldGivesDamage =  EditorGUILayout.Toggle ("Shield Gives Damage", shieldController.ShieldGivesDamage);

        shieldController.ShieldGivesDamage = EditorGUILayout.ToggleLeft("Shield Gives Damage", shieldController.ShieldGivesDamage);
        if (shieldController.ShieldGivesDamage)
        {
            EditorGUI.indentLevel++;
            shieldController.shieldGivesDamageAmount = EditorGUILayout.IntField("Shield Damage ", shieldController.shieldGivesDamageAmount);
            EditorGUI.indentLevel--;
        }

        // add a custom button to the Inspector component
        if (GUILayout.Button("TestBtn"))
        {
            Debug.Log("TestBtn");
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif