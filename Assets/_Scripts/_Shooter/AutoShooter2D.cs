using UnityEngine;
using System.Collections;

using sillymutts;
public class AutoShooter2D : MonoBehaviour, IShooter {

	// Shooter is Actively Shooting
	public bool active = true;

    public Transform locationSpwan;

	// Reference to projectile prefab to shoot
	public GameObject projectile;

	// Reference to the shooter. maybe just hosting this script is enough?
	public GameObject shooter;

	// shoot power
	public float power = 100.0f;

	public Vector2 directionVector = new Vector2 (0, 1);

	// damage
	public int damage = 100;
	
	// Reference to AudioClip to play
	public AudioClip shootSFX;

	public bool IsRandomShooter = false;

	// min and max time range for firing 
	public Vector2 delayRange = new Vector2(1, 2);

	float delay = 2.0f;

	float _ShootDirectionX = 1;
	float _ShootDirectionY = 1;

	//Animator anim;

	NpcState npcState;

	void Start () {
		ResetDelay ();
		if (IsRandomShooter) {
			StartCoroutine (Fire ());
		}
		npcState = GetComponent<NpcState> ();
	}

	int GetDirectionX(){
		
		if ( this.shooter.transform.localScale.x < 0){
			return -1;
		}else {
			return 1;
		}
	}

	int GetDirectionY(){

		if ( this.shooter.transform.localScale.y < 0){
			return -1;
		}else {
			return 1;
		}
	}

	public void DoFire() {
		// if projectile is specified
		if (projectile && active) {
			if (npcState) {
				//anim.SetBool ("IsShooting", true);
				npcState.IsShooting = true;
			}
			 
			GameObject newProjectile = GameObjectUtility.Instantiate (projectile, locationSpwan.position + transform.forward);

            newProjectile.SetActive(true);

			GiveDamage projectilesGiveDamage = newProjectile.GetComponent<GiveDamage> ();

			projectilesGiveDamage.damage = this.damage;

			// if the projectile does not have a rigidbody component, add one
			if (!newProjectile.GetComponent<Rigidbody2D> ()) {
				newProjectile.AddComponent<Rigidbody2D> ();
			}

			_ShootDirectionX = GetDirectionX ();

            _ShootDirectionY = GetDirectionY ();

			Vector2 vx = new Vector2 (_ShootDirectionX * power, 0);

			Vector2 vy = new Vector2 (0, _ShootDirectionX * power);

			Vector2 v = Vector2.zero;

			if (directionVector.x != 0 && directionVector.y == 0) {
				v = vx;
			} else if (directionVector.x == 0 && directionVector.y != 0) {
				v = vy;
			} else {
				v = vx + vy;
			}

			newProjectile.GetComponent<Rigidbody2D> ().AddForce (v);


			// play sound effect if set
			if (shootSFX) {
				if (newProjectile.GetComponent<AudioSource> ()) { // the projectile has an AudioSource component
					// play the sound clip through the AudioSource component on the gameobject.
					// note: The audio will travel with the gameobject.
					newProjectile.GetComponent<AudioSource> ().PlayOneShot (shootSFX);
				} else {
					// dynamically create a new gameObject with an AudioSource
					// this automatically destroys itself once the audio is done
					SoundManager.instance.PlaySoundOnce (shootSFX, newProjectile.transform.position);
				}
			}
		}  
		//StartCoroutine (StopFire ());
	}

	IEnumerator StopFire () {
		yield return new WaitForSeconds (5);
		if (npcState) {
			npcState.IsShooting = false;
		}
	}

	public void SetActive(bool isActive){
		active = isActive;
	}

	public bool IsActive(){
		return active;
	}

	public void DisableFor(int sec){
		SetActive(false);
		StartCoroutine(Disable(sec));
	}

	IEnumerator Disable (int sec ) {
		yield return new WaitForSeconds (sec);
		SetActive(true);
	}

	// Update is called once per frame
	IEnumerator Fire () {
		yield return new WaitForSeconds (delay);

		DoFire();

		StartCoroutine (Fire ());
	}
		
	void ResetDelay(){
		delay = Random.Range (delayRange.x, delayRange.y);
	}

}
