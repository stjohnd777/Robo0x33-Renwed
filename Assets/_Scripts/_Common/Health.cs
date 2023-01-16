using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

using sillymutts;

public class Health : MonoBehaviour, IHealth {

	[TextArea]
	[Tooltip("Doesn't do anything. Just comments shown in inspector")]
	public string Notes = "When this script is attached to a GameObject, that game object is given mortality. " +
		"Having mortality, the GameObject can play a sound effect, display a partical effect, and trigger an animation when the GameObject is injured, dies, or Heals";

	[Header("Delegate Sound")]
	[Tooltip("If true use aufio source on SoundManager.")]
	public bool IsUseSoundManager = false;
	[Space(10)]

	[Header("Hit Point : Health")]
	[Tooltip("Initial Health Point.")]
	public int health;
	[Tooltip("Max Health Point.")]
	public int maxHealth;
	[Tooltip("Injured Immunitity interval Gaurd.")]
	public float hurtImmunityInterval = 0;
    public bool IsAlive = true;
	[Space(10)]

	[Header("Death: SFX, PE, Anim, Destroy")]
	[Tooltip("Death Sound Effext.")]
	public AudioClip deathSFX;
	[Tooltip("Death Partical Effect.")]
	public GameObject onDeathParticalEffect;
	[Tooltip("Death Animation Trigger.")]
	public string onDeathAnimationTrigger = "Die";
    public bool cleanUpOnDeath = true;
//	[Tooltip("Destroy Wait Time.")]
//	public int waitTime = 1;
	[Space(10)]


	[Header("Injured: SFX, PE, Anim")]
	[Tooltip("Injured Sound Effext.")]
	public AudioClip injuredSFX;
	[Tooltip("Injured Partical Effect.")]
	public GameObject onInjuredParticalEffect;
	[Tooltip("Injured Animation Trigger.")]
	public string onInjuredAnimationTrigger = "Hurt";
	[Space(10)]

	[Header("Heal: SFX, PE, Anim")]
	[Tooltip("Heal Sound Effext.")]
	public AudioClip healthSFX;
	[Tooltip("Heal Partical Effect.")]
	public GameObject onHealParticalEffect;
	[Tooltip("Heal Animation Trigger.")]
	public string onHealAnimationTrigger = "Heal";
	[Space(10)]

	[Header("Recover")]
	[Tooltip("Recover from Injured or Stunned Sound Effext.")]
	public AudioClip recoverSFX;
	[Tooltip("Recover from Injured or Stunned Partical Effect.")]
	public GameObject onRecoverdParticalEffect;
	[Tooltip("Recover from Injured Animation Trigger.")]
	public string onRecoverAnimationTrigger = "Recover";
	[Space(10)]

	[Header("Stunned Recover Time : possible")]
	[Tooltip("Stunned time.")]
	public float recoverTime = 1f;   // how long to wait at a waypoint
	[Space(10)]

	Animator _animator;

	AudioSource _audioSource;
 
	HealthBar _healthBar;

	void Awake(){
		_audioSource	= GetComponent<AudioSource>();
		_animator 		= GetComponent<Animator>();
	    _healthBar      = GetComponentInChildren<HealthBar> ();
	 
	}
			
	void Start () {
		maxHealth =   health;
	}

	[HideInInspector]
	public bool OnetimeDeathGaurd = true;
	// Update is called once per frame
	void Update () {

        if (_animator)
        {
            _animator.SetInteger("health", health);
        }

        if( health <= 0)
        {
            if (OnetimeDeathGaurd)
            {
                Death();
            }
        }
    }


	public void Death(){
 

        IsAlive = false;

        OnetimeDeathGaurd = false;

		GetComponent<Rigidbody2D> ().velocity = new Vector2(0, 0);

        GiveDamage giveDamage = GetComponent<GiveDamage>();
        if (giveDamage != null)
        {
            giveDamage.enabled = false;
        }

        AutoShooter2D autoShooter2D = GetComponent<AutoShooter2D>();
        if (autoShooter2D != null)
        {
            autoShooter2D.enabled = false;
        }

        ActionWaypointMovement actionWaypointMovement = GetComponent<ActionWaypointMovement>();
		if ( actionWaypointMovement !=null){
			actionWaypointMovement.IsSuppended = true;
            actionWaypointMovement.enabled = false;

        }

        NpcState npcState = GetComponent<NpcState>();
        if  ( npcState != null)
        {
            npcState.enabled = false;
        }


        if (deathSFX!=null)
		{
			PlaySound (deathSFX);
		}

		if (onDeathParticalEffect!=null) {
			Instantiate (onDeathParticalEffect, transform.position, Quaternion.identity);
		}

        IHasValue hasValue = GetComponent<IHasValue>();
        if (hasValue!=null)
        {
            if (!hasValue.IsTaken())
            {
				GameManager.getInstance().AddPoints(hasValue.Take());

                FadingTextFactory.CreateFloatingTextScore("+" + hasValue.GetValue(), GameObject.FindGameObjectWithTag("Player").transform);
            }
        }

	
		if(_animator!= null && onDeathAnimationTrigger != null){
			_animator.SetTrigger(onDeathAnimationTrigger);
        }

		/*
		 *  TODO: Consider creating a health script just for the player(s)
		 * chacter for problem like below, albeit the code will have duplication
		 * perhapse some OO could help with a common class
		 * 
		*/
		if (this.gameObject.tag == "Player") {

			IDie canDie = GetComponent<IDie> ();
			if (canDie != null) {
				canDie.Die ();
			} else {
				StartCoroutine (ResetPlayer ());
			}
		} else {
			// is there is a death animation on this game object 
			// wait to clean up object unitl anuimation finishes
			// if there is no animation on this game object then 
			// clean up immediately, I am thinking that using an event on the last key frame
			// is better, however this move code to the animation clip and might be hard to 
			// find where and how is responsible for what!
			AnimatorClipInfo[] cinfo = null;
			if(_animator){
			 cinfo = _animator.GetCurrentAnimatorClipInfo(0);
			}
			float length = cinfo != null ? cinfo[0].clip.length :  0f;
            if (cleanUpOnDeath)
            {

                StartCoroutine(CleanUpAfterAnimation(length));
            }
		}

	}

	IEnumerator ResetPlayer(){

		yield return new WaitForSeconds(1f);
		if (GameManager.getInstance()){ // if the gameManager is available, tell it to reset the game
			GameManager.getInstance().PlayerHasDied();
		} else {// otherwise, just reload the current level
			string currentSceneName = SceneManager.GetActiveScene().name;
			SceneManager.LoadScene (currentSceneName);
 
		}
	}

    public void CleanUpAfterAnimation2()
    {
        //NpcState npcState = GetComponent<NpcState>();
        //if (npcState != null)
        //{
        //    npcState.enabled = false;
        //}

        //ActionWaypointMovement actionWaypointMovement = GetComponent<ActionWaypointMovement>();
        //if (actionWaypointMovement != null)
        //{
        //    actionWaypointMovement.IsSuppended = true;
        //}

        StartCoroutine(CleanUpAfterAnimation(0));
    }


    IEnumerator CleanUpAfterAnimation(float length){

		yield return new WaitForSeconds(length);

        GameObjectUtility.Destroy(gameObject);

	}

	private bool hasImmunity = false;

    public void SetImmunityForSec(float sec)
    {
        StartCoroutine(TempImmunity(sec));
    }

    public bool GetHasImmunity()
    {
        return hasImmunity;
    }


    IEnumerator TempImmunity(float delay){
		hasImmunity = true;
		yield return new WaitForSeconds (delay);
		hasImmunity = false;
	}

    public void SetIsImmunityFromDamage(bool isImune)
    {
        hasImmunity = isImune;
    }

    public void ApplyDamage(int damage){

        if (!IsAlive)
        {
            FadingTextFactory.CreateFloatingTextDamage("Dead", transform);
            return;
        }

		if (hasImmunity) {
            //FloatingTextController.CreateFloatingText("Immune", transform);
            return;
		} else {
            // need to prevent a damange hit every frame, once per interval
			StartCoroutine (TempImmunity(hurtImmunityInterval));
		}

 		health = health -damage;
        FadingTextFactory.CreateFloatingTextDamage("-" + damage, transform);

        // perhapse this is incorrect, stunnable has it own path
        IStunnable stunnable = GetComponent<IStunnable>();
		if (stunnable != null){
			stunnable.Stunned();
		}

		// if this game object is a shooter disable shooting for 
		// one second
		IShooter shooter = GetComponent<IShooter>();
		if ( shooter != null){
			shooter.DisableFor(1);
		}

		if (health > 0) {
			
			if (injuredSFX) {
				PlaySound (injuredSFX);
			}

			if (onInjuredParticalEffect != null) {
				Instantiate (onDeathParticalEffect, transform.position, Quaternion.identity);
			}

			// if you are moving pause movement
			ActionWaypointMovement actionWaypointMovement = GetComponent<ActionWaypointMovement> ();
			if (actionWaypointMovement != null) {
				actionWaypointMovement.IsSuppended = true;
			}

			if (_animator != null && onInjuredAnimationTrigger != null) {
				_animator.SetTrigger (onInjuredAnimationTrigger);
				StartCoroutine (Recover ());
			}

 		 
		}
        else {
			
			if(OnetimeDeathGaurd){
				Death();
			}
		}
	}

	public int GetHealth(){
		return health;
	}

	public float GetHealthPercent(){
		return ( (float)(health) ) /  maxHealth ;
	}
			
 
	IEnumerator Recover()
	{
		yield return new WaitForSeconds(recoverTime); 
		ActionWaypointMovement actionWaypointMovement = GetComponent<ActionWaypointMovement>();
		if ( actionWaypointMovement !=null){
			actionWaypointMovement.IsSuppended = false;
		}
		if ( _animator != null &&  onRecoverAnimationTrigger != null){
			_animator.SetTrigger(onRecoverAnimationTrigger);
		}

	}

	public void ApplyHeal(int heal){
		health = health - heal;

		if (healthSFX)
		{
			PlaySound (healthSFX);
		}

		if (onHealParticalEffect!=null) {
			Instantiate (onDeathParticalEffect, transform.position, Quaternion.identity);
		}

		if(_animator!= null && onHealAnimationTrigger!=null){
			_animator.SetTrigger(onHealAnimationTrigger);
		}
	}


	// base class 
	void PlaySound(AudioClip sfx){

		if ( sfx!= null){
			if ( IsUseSoundManager ){
				SoundManager.getInstance().PlaySoundOnce(sfx);
			} else {
				if (_audioSource != null){
					_audioSource.PlayOneShot(sfx);
				}
			}
		}
	}


}
