#define WORKAROUND_UNITY_BUG

using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using sillymutts;
using UnityEngine.Serialization;

// using System;
// using System.Net.Configuration;
// using Microsoft.Win32;
// using UnityEditor.U2D.IK;
// using UnityEngine.Serialization;

/*
 * movement
 * 	left
 * 	right
 * 	jump
 * 		double jump
 * 		tripple jump
 * Dash
 * 		ground dash
 * 		air dash
 * freeze movement
 * bounce off enemy head
 * 
 * moving platform contact become child of platform 
 * 
 * apply damage
 * death
 * respawn
 * pause game
 * 
 * what is ground
 * 
 * 	picking up coins
 * 		play sound pickup
 * 
 * death sound
 * fall sound
 * jump sound
 * invoke victory condition
 * victory sound
 * 
 * animator control
 * 
*/
public class CharacterController2D : MonoBehaviour,
    IVictory,
    IHealth, IFacingRight, IsGrounded, IDie, IRespawn,
    IFreezeUnFreeze,
    ICollectPickUp{
    [Tooltip("Player Health")] public int playerHealth = 1;

    public int maxHealth = 3;
    
    [Space(10)] 
    [Header("Animations")] public string deathAmination = "Death"; 
    public string victoryAmination = "Victory";

    [Space(10)]
    
    [Header("Determines What Layers are Ground")] public LayerMask whatIsGround;
    
    
    [Tooltip("Player Ground Check Collider")]
    public Transform groundCheck;
    [Tooltip("Player Ground Check Collider Circle Radius.")] public float radius = 20;

    [Space(10)] [Header("Speed Movement Horizontal.")] [Range(0.0f, 60.0f)]
    // create a slider in the editor and set limits on moveSpeed
    public float moveSpeed = 3f;

    [Space(10)] [Tooltip("Jump Force")] public float jumpForce = 600f;
    public GameObject dustEffectPrefab;
    public Transform  spwanEffectLocation;

    [Space(10)] [Header("One Way Platforms")] [Tooltip("Determines what layers are considered one-way ...")]
    public LayerMask whatIsOnewayPlatformUp;

    [Tooltip("Layers are One-Way Down.")]
    public LayerMask whatIsOnewayPlatformDown;

    [Tooltip("Layers are One-Way Right.")]
    public LayerMask whatIsOnewayPlatformRight;

    [Tooltip("Layers are considered One-Way Left.")]
    public LayerMask whatIsOnewayPlatformLeft;

    [Space(10)]
    
    [Header("Sounds")]
    public AudioClip coinSFX;

    public AudioClip deathSFX;
    public AudioClip hurtSFX;
    public AudioClip fallSFX;
    public AudioClip jumpSFX;
    public AudioClip victorySFX;
    public AudioClip dashingGroundSFX;
    public AudioClip canNotDashGroundSFX;
    public AudioClip powerJumpSFX;
    public AudioClip canNotPowerFX;
    public AudioClip airDashSFX;
    public AudioClip canNotAirDashFX;

    [Space(10)] 
    Transform _transform;
    Rigidbody2D body2D;
    Animator animator;
    AudioSource _audio;

    // hold player motion in this time step


    // player tracking
    [Header("Character State")]
    public bool isGrounded = false;
    public bool isFacingRight = true;
    public bool isRunning = false;
    
    public float horizontalAxisInput;
    public float xVelocityPlayer;
    public float yVelocityPlayer;
    
    public bool isOnWall = false;
    
    [Header("Player Input State")]
    public bool isJumpButtonDown = false;
    public bool isFire1ButtonDown = false;
    public bool isFire2ButtonDown = false;
    public bool isShoot = false;
    public bool isMele = false;
    public bool playerCanMove = true;

    [Header("Double Jump")]
    public bool isDoubleJumpEnabled = true;
    public bool canDoubleJump = false;
    [Header("Power Jump")]
    public bool isPowerJumpEnabled = false;
    public bool isPowerJumping = false;
    public bool isCoolingDownPowerJump = false;
    [Header("Ground Dashing")]
    public bool isGroundDashingEnabled = true;
    public bool isCoolingDownGroundDash = false;
    public bool isGroundDashing = false;
    [Header("Air Dashing")]
    public bool isAirDashingEnabled = true;
    public bool isAirDashing = false;
    public bool isAirDashCoolingDown = false;
    
    
    [Space(20)]

    // store the layer the player is on (setup in Awake)
    int playerLayerNumber;

    // number of layer that Platforms are on (setup in Awake)
    int oneWayplatformLayerNumberUp;
    int oneWayplatformLayerNumberDown;
    int oneWayplatformLayerNumberRight;
    int oneWayplatformLayerNumberLeft;

    WallDetector[] detectors;

    void Awake(){
        detectors = GetComponentsInChildren<WallDetector>();
        //groundProbe = GetComponent<GroundProbe> (); 
        maxHealth = playerHealth;
        // get a reference to the components we are going to be changing and store a reference for efficiency purposes
        _transform = GetComponent<Transform>();

        body2D = GetComponent<Rigidbody2D>();
        if (body2D == null){
            // if Rigidbody is missing
            Debug.LogError("Rigidbody2D component missing from this gameobject");
        }

        animator = GetComponent<Animator>();
        if (animator == null){
            // if Animator is missing
            Debug.LogError("Animator component missing from this gameobject");
        }

        _audio = GetComponent<AudioSource>();
        if (_audio == null){
            // if AudioSource is missing
            Debug.LogWarning("AudioSource component missing from this gameobject. Adding one.");
            // let's just add the AudioSource component dynamically
            _audio = gameObject.AddComponent<AudioSource>();
        }

        // determine the player's specified layer
        playerLayerNumber = this.gameObject.layer;

        // TODO : revisit
        // determine the platform's specified layer
        oneWayplatformLayerNumberUp = LayerMask.NameToLayer("OneWayPlatform");
        oneWayplatformLayerNumberDown = LayerMask.NameToLayer("OneWayPlatformDown");
        oneWayplatformLayerNumberRight = LayerMask.NameToLayer("OneWayPlatformRight");
        oneWayplatformLayerNumberLeft = LayerMask.NameToLayer("OneWayPlatformLeft");
    }

    public bool IsOnGround(){
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, whatIsGround);
        return isGrounded;
    }

    [Tooltip("WIP Wall Jumping")]
    public bool isWallJumpEnabled = false;

    /*
     * Update: Update is called once per frame.
     * It is the main workhorse function for frame updates.
     */
    private void Update(){
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, whatIsGround);
        
        
        
        //isGrounded = groundProbe.IsOnGround ();
        
        /*if (detectors != null && detectors.Length == 2){
            isOnWall = detectors[0].isOnWall || detectors[1].isOnWall;
            if (wallJumpEnabled){
                foreach (WallDetector detector in detectors){
                    if (detector.isOnWall){
                        isOnWall = true;
                    }
                    else{
                        isOnWall = false;
                    }
                }
            }
        }*/

        // exit update if player cannot move or game is paused
        if (!playerCanMove || (Time.timeScale == 0f)){
            return;
        }

        // determine horizontal velocity change based on the horizontal input
        horizontalAxisInput = CrossPlatformInputManager.GetAxisRaw("Horizontal");

        animator.SetFloat("HInput", horizontalAxisInput);
        animator.SetFloat("AbsHInput", Mathf.Abs(horizontalAxisInput));
        animator.SetFloat("HVelocity", body2D.velocity.x);
        animator.SetFloat("VVelocity", body2D.velocity.y);
        //animator.SetBool("IsFire1", CrossPlatformInputManager.GetButtonDown("Fire1"));

        isFire1ButtonDown = CrossPlatformInputManager.GetButtonDown("Fire1")  ?  true : false;
        
        // if (CrossPlatformInputManager.GetButtonDown("Fire1")){
        //     isFire1ButtonDown = true;
        // }
        // else if (CrossPlatformInputManager.GetButtonUp("Fire1")){
        //     isFire1ButtonDown = false;
        // }

        animator.SetBool("IsFire1", isFire1ButtonDown);
        animator.SetBool("IsFire2", CrossPlatformInputManager.GetButtonDown("Fire2"));
        animator.SetBool("IsFire3", CrossPlatformInputManager.GetButtonDown("Fire3"));
        animator.SetBool("IsMele1", CrossPlatformInputManager.GetButtonDown("Mele1"));
        animator.SetBool("IsMele2", CrossPlatformInputManager.GetButtonDown("Mele2"));
        animator.SetBool("IsMele3", CrossPlatformInputManager.GetButtonDown("Mele3"));

        // Determine if running based on the horizontal movement
        isRunning = horizontalAxisInput != 0;
        animator.SetBool("IsRunning", isRunning);

        animator.SetFloat("AbsHVelocity", Mathf.Abs(horizontalAxisInput));


        // allow double jump
        if (isGrounded){
            canDoubleJump = true;
        }

        // Set the grounded animation states
        animator.SetBool("IsGrounded", isGrounded); //ok

        xVelocityPlayer = body2D.velocity.x;
        // get the current vertical velocity from the rigidbody component
        yVelocityPlayer = body2D.velocity.y;

        isJumpButtonDown = CrossPlatformInputManager.GetButtonDown("Jump") ;
        if (isGrounded && isJumpButtonDown) {// If grounded AND jump button pressed, then allow the player to jump
             DoJump();
        } else if ( (canDoubleJump || isOnWall) && isJumpButtonDown){  // If canJump and button down
            DoJump();
            canDoubleJump = false;
        }

        isFire1ButtonDown = CrossPlatformInputManager.GetButtonDown("Fire1");
        if (isFire1ButtonDown) {
            isShoot = !isShoot;
            animator.SetBool("IsShoot", isShoot);
            GetComponent<Shooter2D>().Shoot();
            animator.SetBool("IsShooting", isShoot);
            //animator.SetTrigger ("Smack");
            StartCoroutine(StopShooting());
        }

        isFire2ButtonDown = CrossPlatformInputManager.GetButtonDown("Fire2");
        if (isFire2ButtonDown) {
            isMele = !isMele;
            animator.SetBool("IsMele", isMele);
            animator.SetTrigger("Kick");
        }

        // TODO: WTF 
        // If the player stops jumping mid jump and player is not yet falling
        // then set the vertical velocity to 0 (he will start to fall from gravity)
        if (CrossPlatformInputManager.GetButtonUp("Jump") && yVelocityPlayer > 0f){
            yVelocityPlayer = 0f;
        }

        // Change the actual velocity on the rigidbody
        body2D.velocity = new Vector2(horizontalAxisInput * moveSpeed, yVelocityPlayer);

        // if moving up then don't collide with platform layer
        // this allows the player to jump up through things on the platform layer
        // NOTE: requires the platforms to be on a layer named "Platform"
        Physics2D.IgnoreLayerCollision(playerLayerNumber, oneWayplatformLayerNumberUp, (yVelocityPlayer > 0.0f));
        Physics2D.IgnoreLayerCollision(playerLayerNumber, oneWayplatformLayerNumberRight, (xVelocityPlayer > 0.0f));
        Physics2D.IgnoreLayerCollision(playerLayerNumber, oneWayplatformLayerNumberLeft, (xVelocityPlayer < 0.0f));

        // Ground Dashing 
        if (isGrounded && (Input.GetKeyDown(keyGroundDash) || isGroundDashing)){
            GroundDashRequested();
        }

        // Air Dashing 
        if ( !isGrounded &&  Input.GetKeyDown(keyAirDash) && !isAirDashing)   {
            AirDashRequested();
        }

        // Power Jump
        if (isGrounded && (Input.GetKeyDown(keyPowerJump) || isPowerJumping)){
            PowerJumpRequested();
        }
    }

    // make the player jump
    void DoJump() {
        if (!isGrounded){
            if (dustEffectPrefab){
                GameObject clone = Instantiate(dustEffectPrefab);
                if (spwanEffectLocation){
                    clone.transform.position = spwanEffectLocation.position;
                }
                else{
                    clone.transform.position = transform.position;
                }
            }
        }
        
        // reset current vertical motion to 0 prior to jump
        yVelocityPlayer = 0f;
        // add a force in the up direction
        body2D.AddForce(new Vector2(0.0f, jumpForce));
        // play the jump sound
        PlaySound(jumpSFX);
    }

    /*
     * LateUpdate: LateUpdate is called once per frame, after Update has finished.
     * Any calculations that are performed in Update will have completed when LateUpdate begins.
     * A common use for LateUpdate would be a following third-person camera. If you make your
     * character move and turn inside Update, you can perform all camera movement and rotation
     * calculations in LateUpdate. This will ensure that the character has moved completely
     * before the camera tracks its position.
     */
    // Checking to see if the sprite should be flipped
    // this is done in LateUpdate since the Animator may override the localScale
    // this code will flip the player even if the animator is controlling scale
    private void LateUpdate(){

        if (horizontalAxisInput != 0){
            isFacingRight = horizontalAxisInput > 0 ? true : false;
        }

        // get the current scale
        var localScale = _transform.localScale;
        // check to see if scale x is right for the player
        // if not, multiple by -1 which is an easy way to flip a sprite
        
        if (((isFacingRight) && (localScale.x < 0)) || ((!isFacingRight) && (localScale.x > 0))){
            localScale.x *= -1;
        }
        // update the scale
        _transform.localScale = localScale;
    }

    // if the player collides with a MovingPlatform, then make it a child of that platform
    // so it will go for a ride on the MovingPlatform
    void OnCollisionEnter2D(Collision2D other){
        if (  other.gameObject.CompareTag("MovingPlatform") ) { 
            this.transform.parent = other.transform;
        }
    }

    // if the player exits a collision with a moving platform, then un-child it
    void OnCollisionExit2D(Collision2D other){
        if (other.gameObject.CompareTag("MovingPlatform")){
            this.transform.parent = null;
        }
    }

    // do what needs to be done to freeze the player
    public void FreezeMotion(){
        body2D.velocity = Vector3.zero;
        playerCanMove = false;
        body2D.isKinematic = true;
    }

    // do what needs to be done to unfreeze the player
    public void UnFreezeMotion(){
        playerCanMove = true;
        body2D.isKinematic = false;
    }

    // play sound through the audio source on the game object
    void PlaySound(AudioClip clip){
        _audio.PlayOneShot(clip);
    }

    // Temporary Immunity /////////////////////////////
    private bool _hasImmunity = false;

    public void SetImmunityForSec(float sec){
        StartCoroutine(TempImmunity(sec));
    }

    public bool GetHasImmunity(){
        return _hasImmunity;
    }

    private IEnumerator TempImmunity(float delay){
        _hasImmunity = true;
        yield return new WaitForSeconds(delay);
        _hasImmunity = false;
    }
    
    // Apply Damage ////////////////////////////////
    public void ApplyDamage(int damage){
        var shieldController = GetComponentInChildren<ShieldController>();
        if (shieldController != null && shieldController.GetIsShieldUp()){
            shieldController.ApplyDamageToShield(damage);
            damage = shieldController.GetWeightedDamagePassedToPlayer(damage);
        }
        if (_hasImmunity){
            FadingTextFactory.CreateFloatingTextDamage("*", transform);
            return;
        }
        FadingTextFactory.CreateFloatingTextDamage("-" + damage, transform);
        if (playerCanMove){
            playerHealth -= damage;
            if (playerHealth <= 0){
                // player is now dead, so start dying
                PlaySound(deathSFX);
                StartCoroutine(KillPlayer());
            }
            else{
                PlaySound(hurtSFX);
            }
        }
    }

    public void ApplyHeal(int heal){
        if (isDead){
            return;
        }
        playerHealth += heal;
    }

    public int GetHealth(){
        return playerHealth;
    }

    public float GetHealthPercent(){
        return ((float) (playerHealth)) / ((float) (maxHealth));
    }

    // public function to kill the player when they have a fall death
    // instantly kill player
    public void InstantDeath(){
        if (playerCanMove){
            isDead = true;
            playerHealth = 0;
            PlaySound(fallSFX);
            StartCoroutine(KillPlayer());
        }
    }

    public void Die(){
        StartCoroutine(KillPlayer());
    }

    bool isDead = false;

    // coroutine to kill the player
    private IEnumerator KillPlayer(){
        isDead = true;
        if (playerCanMove){
            // freeze the player
            FreezeMotion();
            // play the death animation
            animator.SetTrigger(deathAmination);
//			AnimatorClipInfo deathClipInfo = animator.GetCurrentAnimatorClipInfo(0);
//			AnimationClip deathClip = deathClipInfo.clip;
//			float lenghtSec = deathClip.length;
            // After waiting tell the GameManager to reset the game
            yield return new WaitForSeconds(2.0f);
            if (GameManager.getInstance()){
                // if the gameManager is available, tell it to reset the game
                GameManager.getInstance().PlayerHasDied();
            }
            else{
                // otherwise, just reload the current level
                GameObjectUtility.ClearPools();
                GameManager.LoadScene(Application.loadedLevelName);
            }
        }
    }

    // public function on victory over the level
    public void Victory(){
        //PlaySound(victorySFX);
        FreezeMotion();
        animator.SetTrigger(victoryAmination);
        //if (GameManager.gm) {// do the game manager level compete stuff, if it is available
        //	GameManager.gm.LevelCompete();
        //}
    }

    // public function to respawn the player at the appropriate location
    // reset health
    // set play to alive 
    // un freeze

    public void Respawn(Vector3 spawnloc){
        playerHealth = maxHealth;
        isDead = false;
        UnFreezeMotion();
        playerHealth = maxHealth;
        _transform.parent = null;
        _transform.position = spawnloc;
        animator.SetTrigger("ReSpawn");
    }

    // coroutine to turn off animation shoot 
    IEnumerator StopShooting(){
        yield return new WaitForSeconds(0.5f);
        isShoot = false;
        animator.SetBool("IsShoot", isShoot);
    }

    public void CollectCoin(int amount){
        PlaySound(coinSFX);
        if (GameManager.getInstance()){
            GameManager.getInstance().AddPoints(amount);
        }
    }

    public void CollectPickUp(string type, int amount, AudioClip sfx){
        PlaySound(sfx);
        if (GameManager.getInstance()){
            // add the points through the game manager, if it is available
            GameManager.getInstance().AddPoints(amount);
        }
    }

    public void AddInventory(string type, int amount, AudioClip sfx){
        PlaySound(sfx);
        if (GameManager.getInstance()){
            // add the points through the game manager, if it is available
            GameManager.getInstance().AddInventory(type, amount);
        }
    }

    // make the player jump
    public void EnemyBounce(){
        DoJump();
    }

    public bool IsFacingRight(){
        return isFacingRight;
    }

    public void SetIsImmunityFromDamage(bool isImune){
        _hasImmunity = isImune;
    }

    //
    // PowerJumpSection Section
    // 
    [Space(10)] [Header("Power Jump Section")]
    [Tooltip("Power Jump Key")] public KeyCode keyPowerJump = KeyCode.P;
    [Tooltip("POWER JUMP FORCE X")] public float powerJumpForce = 3000.0f;
    [Tooltip("POWER JUMP DURATION")] public float powerJumpDuraton = .2f;
    [Tooltip("POWER JUMP COOLDOWN")] public float powerJumpCoolDownDuraton = .1f;
    public float powerJumpForceY = 0.0f;
    public float powerJumpForceX = 0.0f;


    private bool CanPowerJump(){
        return isPowerJumpEnabled && isGrounded && !isCoolingDownPowerJump;
    }

    private void PowerJumpRequested(){
        if (CanPowerJump()){
            isPowerJumping = true;
            PlaySound(powerJumpSFX);
            powerJumpForceX = 0.0f;
            powerJumpForceY = jumpForce;
            body2D.AddForce(new Vector2(0.0f, powerJumpForce));
            StartCoroutine(PowerJumpCounter());
        }
        else{
            PlaySound(canNotPowerFX);
        }
    }

    private IEnumerator PowerJumpCounter(){
        yield return new WaitForSeconds(powerJumpDuraton);
        isPowerJumping = false;
        isCoolingDownPowerJump = true;
        StartCoroutine(PowerJumpCoolDown());
    }

    private IEnumerator PowerJumpCoolDown(){
        yield return new WaitForSeconds(dashCoolDownDuraton);
        isCoolingDownPowerJump = false;
    }

    //
    // Air Dashing Section
    // 
    [Space(10)] [Header("Air Dash Section")]
    public bool isAirDashEnabled = false;
    [Tooltip("Air Dashing Key")] public KeyCode keyAirDash = KeyCode.O;
    [Tooltip("Air Dashing Force")] public float airDashForce = 2000.0f;
    [Tooltip("Air Dashing Duration")] public float airDashDuraton = .1f;


    private bool CanAirDash(){
        return isAirDashEnabled && !isAirDashCoolingDown && !isGrounded;
    }

    private void AirDashRequested(){
        if (CanAirDash()){
            isAirDashing = true;
            isAirDashCoolingDown = true;
            PlaySound(airDashSFX);
            var y = 0;
            var x = IsFacingRight() ? airDashForce : -airDashForce;
            body2D.AddForce(new Vector2(x, y));
            StartCoroutine(AirDashCounter());
        }
        else{
            isAirDashing = false;
            //PlaySound(canNotAirDashFX);
        }
    }
    private IEnumerator AirDashCounter(){
        yield return new WaitForSeconds(airDashDuraton);
        isAirDashing = false;
        isAirDashCoolingDown = false;
    }

    //
    // Ground Dashing Section
    // 
    [Space(10)] [Header("Ground Dash Section")] [Tooltip("Ground Dash Key")]
    public KeyCode keyGroundDash = KeyCode.I;
    [Tooltip("Ground DASH Force")] public float groundDashForce = 100.0f;
    [Tooltip("Ground DASH DURATION")] public float dashDuraton = .2f;
    [Tooltip("Ground DASH COOLDOWN")] public float dashCoolDownDuraton = .1f;
    [Tooltip("Ground Dash Enabled")] public bool isGroundDashEnabled = false;


    private bool CanGroundDash(){
        return isGroundDashEnabled && isGrounded && !isCoolingDownGroundDash;
    }

    private void GroundDashRequested(){
        if (CanGroundDash()){
            isGroundDashing = true;
            PlaySound(dashingGroundSFX);
            var dir = IsFacingRight() ? 1 : -1;
            body2D.AddForce(new Vector2(dir * groundDashForce, 0));
            StartCoroutine(GroundDashCounter());
        }
        else{
            isGroundDashing = false;
            //PlaySound(canNotDashGroundSFX);
        }
    }

    private IEnumerator GroundDashCounter(){
        yield return new WaitForSeconds(dashDuraton);
        isGroundDashing = false;
        isCoolingDownGroundDash = true;
        StartCoroutine(GroundDashCoolDown());
    }

    private  IEnumerator GroundDashCoolDown(){
        yield return new WaitForSeconds(dashCoolDownDuraton);
        isCoolingDownGroundDash = false;
    }
}