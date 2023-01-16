using UnityEngine;
using System.Collections;

[System.Serializable]
public class Anim
{
    public string tag;
    public AudioClip sfx;
}



public class CharacterController2DMe: MonoBehaviour, IFreezeUnFreeze, IDie, IRespawn, IFacingRight {

	[Header("Movement Dynamics")]
	[Tooltip("Speed Horizontal.")]
	public float HorizontalMovement_Speed = 100f;
	[Tooltip("Jump Force.")]
	public float jumpForce = 600f;
	public bool useVelocityOverForce = false;
	public float jumpSpeed = 50;
	public GameObject dustEffectPrefab;
	public Transform spwanEffectLocation;
	public float DelayBetweenJumps = .2f;
	[Header("Delay Between Jumps")]
	public float jumpDelay = .5f;
	[HideInInspector]
	public bool playerCanMove = true;
    [Space(10)]


    public Anim[] animations;

	[Header("Sound Effects")]
	public AudioClip idelSFX;
	public AudioClip walkSFX;
	public AudioClip runSFX;
	public AudioClip jumpSFX;
	public AudioClip fire1SFX;
	public AudioClip fire2SFX;
	public AudioClip roleSFX;
	public AudioClip deathSFX;
	public AudioClip hurtSFX;
	public AudioClip madSFX;
	public AudioClip happySFX;
	[Space(10)]

	[Header("Input Reads")]
	public float AbsHInput;
    public float HorizontalInputAxis ;

    public float AbsVInput;
    public float VerticalInputAxis ;

    public float FourthInputAxis;
    public float FifthInputAxis;

	public bool Fire1_Input ;
	public bool Fire2_Input ;
	public bool Fire3_Input ;

	public bool Mele1_Input ;
	public bool Mele2_Input ;
	public bool Mele3_Input ;

	public bool Jump_Input ;
	public bool Crouch_Input;
	public bool Roll_Input;
	public bool Throw_Input;

    public float Trigger_Input;

    public float D_UP_Input;
    public float D_LEFT_Input;
    public float D_RIGHT_Input;
    public float D_DOWN_Input;


    [Space(10)]
	
	[Header("Derived State")]
	public bool CanMove = true;
	public bool IsGrounded ;
	public bool CanDoubleJump = false;
	public bool IsRight = true;
	public float HSpeed;
	public float VSpeed;
	[Space(10)]

	[Header("Animation State")]
	public bool isIdle;
	public bool isWalking;
	public bool isRunning;
	public bool isCrouch;
	public bool isRolling;
	public bool isJumping;
	public bool isHurt;
	public bool isDie;
	[Space(10)]

	[Header("State")]
	public string OldState;
	public string NewState;

	private Rigidbody2D body2D;
	private Animator    animator;
	private AudioSource _audio;

	private int _playerLayer;
	private int _platformLayer;

	private GroundProbeCircleOverlaps groundProbe;

	GameObject _fire;
	Animator   _fireAnim;

	void Awake () {
		body2D = GetComponent<Rigidbody2D>();

		groundProbe = GetComponent<GroundProbeCircleOverlaps> ();

		animator = GetComponent<Animator>();

		_audio = GetComponent<AudioSource> ();

		_playerLayer = this.gameObject.layer;

		// determine the platform's specified layer
		_platformLayer = LayerMask.NameToLayer("Platform");

		_fire = GameObject.FindGameObjectWithTag ("FireAnim");

		if (_fire) {
			_fireAnim = _fire.GetComponent<Animator> ();
		}
	}


	void RestState(){
		isIdle = false;
		isWalking= false;
		isRunning= false;
		isCrouch= false;
		isRolling= false;
		isJumping= false;
		isHurt= false;
		isDie= false;
	}

	public bool IsFacingRight(){
		return IsRight;
	}
		
	void ReadInputs(){
		HorizontalInputAxis = Input.GetAxis("Horizontal");
		VerticalInputAxis =  Input.GetAxis("Vertical");

        //FourthInputAxis = Input.GetAxis("4th axis");
        //FifthInputAxis = Input.GetAxis("5th axis");

        FourthInputAxis = Input.GetAxis("AimX");
        FifthInputAxis = Input.GetAxis("AimY");

        Jump_Input = Input.GetButton("Jump");
		Crouch_Input = Input.GetButton("Crouch");
		Roll_Input  = Input.GetButton("Roll");
		Fire1_Input = Input.GetButton("Fire1");
		Fire2_Input = Input.GetButton("Fire2");
		Fire3_Input = Input.GetButton("Fire3");
		Mele1_Input = Input.GetButton("Mele1");
		Mele2_Input = Input.GetButton("Mele2");
		Mele3_Input = Input.GetButton("Mele3");
		Throw_Input = Input.GetButton ("Throw");


        Trigger_Input = Input.GetAxis("Trigger");

        D_UP_Input = Input.GetAxis("D_UP");
        D_LEFT_Input = Input.GetAxis("D_LEFT");
        D_RIGHT_Input = Input.GetAxis("D_RIGHT");
        D_DOWN_Input = Input.GetAxis("D_DOWN");

    }

	void SetParamsOnAnimator(){

		IsGrounded = groundProbe.IsOnGround ();
		animator.SetBool ("IsGrounded", IsGrounded);

		AbsHInput = Mathf.Abs (HorizontalInputAxis);
		animator.SetFloat("AbsHInput", Mathf.Abs(AbsHInput));

		HSpeed = body2D.velocity.x;
		animator.SetFloat ("HVelocity", HSpeed);
		VSpeed = body2D.velocity.y;
		animator.SetFloat ("VVelocity", VSpeed);

        // If the state changes on this parameter then set the 
        // parameter on the animator, since continuiouly setting 
        // the parameter causes the animation to reset
		if (animator.GetBool ("IsCrouching") != Crouch_Input) {
			animator.SetBool ("IsCrouching", Crouch_Input);
		}
        // same as above
		if (animator.GetBool ("IsRolling") != Roll_Input) {
			animator.SetBool ("IsRolling", Roll_Input);
		}
        // same as above
		if (animator.GetBool ("IsFire1") != Fire1_Input) {
			animator.SetBool ("IsFire1", Fire1_Input);
			if (_fireAnim) {
				_fireAnim.SetBool ("IsFire", Fire1_Input);
			}
		}
 
		if (animator.GetBool ("IsThrow") != Throw_Input) {
			animator.SetBool ("IsThrow", Throw_Input);
			if (Throw_Input) {
				ThrowGernade throwit = GetComponentInChildren<ThrowGernade> ();
				if (throwit) {
					throwit.Throw ();
				}
			}
		}

		animator.SetBool("IsFire2",Fire2_Input);
		animator.SetBool("IsFire3",Fire2_Input);
		animator.SetBool("IsMele1",Mele1_Input);
		animator.SetBool("IsMele2",Mele2_Input);
		animator.SetBool("IsMele3",Mele3_Input);

        if (isDie)
        {
            animator.SetTrigger("Die");
        }


    }

    void SyncSoundAndAnim( )
    {
        AnimatorStateInfo animStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        RestState();
        if (animStateInfo.IsTag("Idel"))
        {
            isIdle = true;
            NewState = "Idel";
            if (OldState != NewState)
            {
                OldState = "Idel";
                _audio.Stop();
                PlaySound(idelSFX);
            }
        }
        if (animStateInfo.IsTag("Walk"))
        {
            isWalking = true;
            NewState = "Walk";
            if (OldState != NewState)
            {
                OldState = "Walk";
                _audio.Stop();
                PlaySound(walkSFX);
            }
        }
        if (animStateInfo.IsTag("Run"))
        {
            isRunning = true;
            NewState = "Run";
            if (OldState != NewState)
            {
                OldState = "Run";
                _audio.Stop();
                PlaySound(runSFX);
            }
        }
        if (animStateInfo.IsTag("Roll"))
        {
            isRolling = true;
            NewState = "Roll";
            if (OldState != NewState)
            {
                OldState = "Roll";
                _audio.Stop();
                PlaySound(roleSFX);
            }
        }
        if (animStateInfo.IsTag("Crouch"))
        {
            isCrouch = true;
        }
        if (animStateInfo.IsTag("Hurt"))
        {
            isHurt = true;
            NewState = "Hurt";
            if (OldState != NewState)
            {
                   OldState = "Hurt";
                _audio.Stop();
                PlaySound(hurtSFX);
            }

        }
        if (animStateInfo.IsTag("Die"))
        {
            //_audio.Stop ();
            isDie = true;
        }
        if (animStateInfo.IsTag("Jump"))
        {
            isJumping = true;
            NewState = "Jump";
            if (OldState != NewState)
            {
                OldState = "Jump";
                _audio.Stop();
                //PlaySound(jumpSFX);
            }
        }
    }


    void Update()
	{
        // read the inputs
		ReadInputs ();

        // set the HVelocity for movement
		body2D.velocity = new Vector2(HorizontalMovement_Speed * HorizontalInputAxis, body2D.velocity.y);

        // set the parameters on the animator
		SetParamsOnAnimator ();

        //FaceCorrectDirection(HorizontalInputAxis);

        SyncSoundAndAnim();

        // if you are on the ground you can jump and double jump
        if (IsGrounded){
			CanDoubleJump = true;
		}						
			

		if (Jump_Input ) {
			if (IsGrounded) {
				doJump();
				CanDoubleJump = true;
			} else {
				if (CanDoubleJump) {
					if ( playerCanMove ){
						CanDoubleJump = false;
						doJump();
					}
				}
			}
		}
 
		// if moving up then don't collide with platform layer
		// this allows the player to jump up through things on the platform layer
		// NOTE: requires the platforms to be on a layer named "Platform"
		//Physics2D.IgnoreLayerCollision(_playerLayer, _platformLayer, (body2D.velocity.y > 0.0f)); 


	} 

	void LateUpdate(){
		if (HorizontalInputAxis> 0) {
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z) ;
		} else if ( HorizontalInputAxis < 0) {
			transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z) ;
		}
	}

	public void FaceCorrectDirection(float valueH){
		if ( (valueH <0 && IsRight) || (valueH >0 && !IsRight) ) {
			Flip ();
		}
	}

	void Flip(){
		IsRight = !IsRight;
		transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z) ;
	}
		
	void doJump(){

		if (!IsGrounded) {
			if ( dustEffectPrefab){
				GameObject clone = Instantiate (dustEffectPrefab);
				if (spwanEffectLocation) {
					clone.transform.position = spwanEffectLocation.position;
				} else {
					clone.transform.position = transform.position;
				}
			}
		}

		if (useVelocityOverForce) {
	 
  			body2D.velocity = Vector3.zero;
			body2D.velocity = new Vector2 (body2D.velocity.x, jumpSpeed);
			StartCoroutine (hold (jumpDelay));	 
			 
		} else {
			body2D.velocity = new Vector2 (body2D.velocity.x, 0);
			body2D.AddForce (new Vector2 (0, jumpForce));
			PlaySound (jumpSFX);
			StartCoroutine (hold (jumpDelay));
		}

	}

//	bool IsJumpEnabled = true;
//	IEnumerator DisableJump(float disableTime){
//		IsJumpEnabled = false;
//		yield return new WaitForSeconds (disableTime);
//		IsJumpEnabled = true;
//	}

	IEnumerator hold(float delay)
	{
		// suspend execution for .5 seconds
		playerCanMove = false;
		yield return new WaitForSecondsRealtime(delay);
		playerCanMove = true;
	}

	void PlaySound(AudioClip clip)
	{
		if( clip){
			_audio.PlayOneShot(clip);
		}
	}

    [Header("Parenting")]
	public bool IsChildingToObjectWithTags = true;
    [Tooltip("ParentTo")]
	public string[] TagList = { "MovingPlatform" };
     

    public bool IsInParentingList(string theTag){
		bool ret = false;
		foreach( string aTag in TagList){
			if ( theTag == aTag){
				ret = true;
				return ret;
			}
		}
		return ret;
	}









	// if the player collides with a MovingPlatform, then make it a child of that platform
	// so it will go for a ride on the MovingPlatform
	void OnCollisionEnter2D(Collision2D other)
	{
		if (IsChildingToObjectWithTags) {
			if (other.gameObject.tag == "MovingPlatform") {
				this.transform.parent = other.transform;
			}
		}
	}

	// if the player exits a collision with a moving platform, then unchild it
	void OnCollisionExit2D(Collision2D other)
	{
		if (IsChildingToObjectWithTags) {
			if (other.gameObject.tag == "MovingPlatform") {
				this.transform.parent = null;
			}
		}
	}













	public string RespawnAnimTrigger = "Idel";

	public AudioClip sfxRespawn;

	// reset health
	// put player at spwan location
	// set default animation
	// play repawned spound
	public void Respawn(Vector3 spawnloc) {

        StartCoroutine(_Respawn( spawnloc));
	}


    IEnumerator _Respawn(Vector3 spawnloc)
    {
        yield return new WaitForSeconds(2);
        Health health = GetComponent<Health>();
        health.health = health.maxHealth;
        health.OnetimeDeathGaurd = true;
        health.IsAlive = true;
        playerCanMove = true;
        transform.parent = null;
        transform.position = spawnloc;
        animator.SetTrigger(RespawnAnimTrigger);
        PlaySound(sfxRespawn);
    }


    public float waitOnDie = 2.0f;

    public void Die()
    {
        StartCoroutine(KillPlayer());
    }

    // coroutine to kill the player
    IEnumerator KillPlayer()
    {
        if (playerCanMove)
        {
            // freeze the player
            FreezeMotion();

            // play the death animation
            animator.SetTrigger("Die"); 

            // After waiting tell the GameManager to reset the game
            yield return new WaitForSeconds(waitOnDie);

			if (GameManager.getInstance())
            {// if the gameManager is available, tell it to reset the game
				GameManager.getInstance().PlayerHasDied();
            }
            else
            {
                // otherwise, just reload the current level
                sillymutts.GameObjectUtility.ClearPools();
                Application.LoadLevel(Application.loadedLevelName);
            }
        }
    }












    // do what needs to be done to freeze the player
    public void FreezeMotion()
    {
        body2D.velocity = Vector3.zero;
        playerCanMove = false;
        body2D.isKinematic = true;
    }

    // do what needs to be done to unfreeze the player
    public void UnFreezeMotion()
    {
        playerCanMove = true;
        body2D.isKinematic = false;
    }


	public void ZZZ(){
		// play the death animation
		animator.SetTrigger("Idle"); 
	}
}
