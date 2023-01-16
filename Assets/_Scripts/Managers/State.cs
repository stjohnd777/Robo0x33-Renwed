using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public class ButtonState {
    // Button is pressed or not
    public bool isActive;
    public float holdTime = 0;
}

public enum Directions {
    Right = 1,
    Left = -1
}


public class State : MonoBehaviour {

    public Directions direction = Directions.Right;

    // not inputs but state
	// are you on the ground? what is your (h,v) velocity
	[Header("Physics 2D State")]
    public bool  IsGrounded;
    public float HVelocity;
    public float VVelocity;

    [Header("Physics Dervived")]
    public float AbsHVelocity;
	public float AbsVVelocity;

 
    [Header("Input State")]
    public float  HInput;
    
    [Header("Input State Derived")]
   
	public float  AbsHInput;
	public bool  IsRight;
    public float holdRight;
    public bool  IsLeft;
    public float holdLeft;
    public bool  IsSprint;
    public float holdSprint;
    public bool   IsCrouching;
	public float  holdCrouching;
	public bool   IsSlashing;
	public float  holdSlashing;
	public bool   IsStabbing;
	public float  holdStabbing;
	public bool   IsShooting;
	public float  holdShooting;
	public bool   IsThowing;
	public float  holdThrowing;
	public bool   IsRequestingDefence;
	public float  holdDefending;
	public bool   IsSlide;
	public float  holdSlide;
	public bool   IsRoll;
	public float  holdRoll;
	public bool   IsSneek;
	public float  holdSneek;
	public bool   IsMele1;
	public float  holdMele1;
	public bool   IsMele2;
	public float  holdMele2;
	public bool   IsMele3;
	public float  holdMele3;
	public bool   IsFire1;
	public float  holdFire1;
	public bool   IsFire2;
	public float  holdFire2;
	public bool   IsFire3;
	public float  holdFire3;

	[Header("Health State")]
	public float health;


    private Dictionary<CommandRequestVerbs, ButtonState> buttonStates = new Dictionary<CommandRequestVerbs, ButtonState>();

    private Rigidbody2D body2d;
    private IsGrounded  groundProbe;
    private IHealth     healthScript;

    void Awake(){
	    healthScript = GetComponent<IHealth>();
        body2d = GetComponent<Rigidbody2D>();
        groundProbe = GetComponent<IsGrounded>();
    }

    void Update() {
        HInput = Input.GetAxis("Horizontal");
        AbsHInput = Mathf.Abs(HInput);

        // TODO place check that script exist and throw error if not
        health = healthScript.GetHealth();

        HVelocity       = body2d.velocity.x;
        AbsHVelocity    = Mathf.Abs(HVelocity);
        VVelocity       = body2d.velocity.y;
        AbsVVelocity    = Mathf.Abs(VVelocity);
        IsGrounded      = groundProbe.IsOnGround();

        IsRight = GetButtonValue(CommandRequestVerbs.RequestMoveRight);
        holdCrouching = GetButtonHoldTime(CommandRequestVerbs.RequestMoveRight);
        if (IsRight) {
            direction = Directions.Right;
        }

        IsLeft = GetButtonValue(CommandRequestVerbs.RequestMoveLeft);
        holdCrouching = GetButtonHoldTime(CommandRequestVerbs.RequestMoveLeft);
        if (IsLeft) {
            direction = Directions.Left;
        }

        //IsSprint = GetButtonValue(CommandRequestVerbs.RequestMoveLeft); holdSprint = GetButtonHoldTime(CommandRequestVerbs.RequestMoveLeft);

        //IsCrouching = GetButtonValue(CommandRequestVerbs.RequestCrouch); holdCrouching = GetButtonHoldTime(CommandRequestVerbs.RequestCrouch);
        //IsSlide = GetButtonValue(CommandRequestVerbs.RequestSlide); holdSlide = GetButtonHoldTime(CommandRequestVerbs.RequestSlide);
        //IsRoll = GetButtonValue(CommandRequestVerbs.RequestRoll); holdRoll = GetButtonHoldTime(CommandRequestVerbs.RequestRoll);
        //IsSneek = GetButtonValue(CommandRequestVerbs.RequestSneek); holdSneek = GetButtonHoldTime(CommandRequestVerbs.RequestSneek);
        //IsSlashing = GetButtonValue(CommandRequestVerbs.RequestSlash); holdSlashing = GetButtonHoldTime(CommandRequestVerbs.RequestSlash);
        //IsStabbing = GetButtonValue(CommandRequestVerbs.RequestStab); holdStabbing = GetButtonHoldTime(CommandRequestVerbs.RequestStab);
        //IsShooting = GetButtonValue(CommandRequestVerbs.RequestProjectile); holdShooting = GetButtonHoldTime(CommandRequestVerbs.RequestProjectile);
        //IsMele1 = GetButtonValue(CommandRequestVerbs.RequestMele1); holdMele1 = GetButtonHoldTime(CommandRequestVerbs.RequestMele1);
        //IsMele2 = GetButtonValue(CommandRequestVerbs.RequestMele2); holdMele2 = GetButtonHoldTime(CommandRequestVerbs.RequestMele2);
        //IsMele3 = GetButtonValue(CommandRequestVerbs.RequestMele1); holdMele3 = GetButtonHoldTime(CommandRequestVerbs.RequestMele3);
        //IsThowing = GetButtonValue(CommandRequestVerbs.RequestThrow); holdThrowing = GetButtonHoldTime(CommandRequestVerbs.RequestThrow);
        //IsRequestingDefence = GetButtonValue(CommandRequestVerbs.RequestDefend); holdDefending = GetButtonHoldTime(CommandRequestVerbs.RequestDefend);

        //IsFire1 = GetButtonValue(CommandRequestVerbs.RequestFire1); holdDefending = GetButtonHoldTime(CommandRequestVerbs.RequestFire1);
        //IsFire2 = GetButtonValue(CommandRequestVerbs.RequestFire2); holdDefending = GetButtonHoldTime(CommandRequestVerbs.RequestFire2);
        //IsFire3 = GetButtonValue(CommandRequestVerbs.RequestFire3); holdDefending = GetButtonHoldTime(CommandRequestVerbs.RequestFire3);
    }

    public void SetButtonValue(CommandRequestVerbs key, bool isActive) {

        if (!buttonStates.ContainsKey(key)) {
            buttonStates.Add(key, new ButtonState());
        }

        // get the stored state
        var storedButtonData = buttonStates[key];

        if (storedButtonData.isActive && isActive == false) {
            // if the state changed from on to off then the button has been released
            storedButtonData.holdTime = 0;
        }else if (storedButtonData.isActive && isActive == true){
            // button state is on and new state is on then keep counting
            // hold time
            storedButtonData.holdTime += Time.deltaTime;
        }
        // update the state value with the new state
        storedButtonData.isActive = isActive;
    }

    public bool GetButtonValue(CommandRequestVerbs key) {
        bool ret = false;
        if (buttonStates.ContainsKey(key))
        {
            ret = buttonStates[key].isActive;
        }
        return ret;
    }
    
    public bool IsButtonActivated(CommandRequestVerbs key) {
	    return GetButtonValue(key);
    }

    public float GetButtonHoldTime(CommandRequestVerbs key) {
        float ret = 0;
        if (buttonStates.ContainsKey(key))
        {
            ret = buttonStates[key].holdTime;
        }
        return ret;
    }
}
