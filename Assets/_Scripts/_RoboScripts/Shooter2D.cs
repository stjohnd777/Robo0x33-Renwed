using UnityEngine;
using System.Collections;

using sillymutts;
using UnityEngine.Serialization;

public class Shooter2D : MonoBehaviour {


    public string InputManagerButtonName = "Fire1";

	// Reference to projectile prefab to shoot
	public GameObject projectile;
	// Reference to AudioClip to play
	public AudioClip shootSFX;
	
	
	// Reference to the shooter
	public GameObject shooter;

	public Transform locationSpawn;

	public float power = 10.0f;
	


    public bool IsHandlingSpawnBullet = false;

	//CharacterController2D _CharacterController2D;
	IFacingRight _IFacingRight;

	float _ShootDirectionX = 1;

	void Awake(){

		_IFacingRight = shooter.gameObject.GetComponent<IFacingRight>();

		if (!locationSpawn) {
			locationSpawn = shooter.transform;
		}
	
	}

    // Update is called once per frame
    void Update()
    {
        if (IsHandlingSpawnBullet)
        {
            // Detect if fire button is pressed
            // mabye implement a cooldown between shooting with a co-rutine and canShoot variable or Time.deltaTime
            if (Input.GetButtonDown(InputManagerButtonName))
            {
                Shoot();
            }
        }
    }


    public void Shoot(){
 
		// if projectile is specified
		if (projectile)
		{
			// Instantiante projectile at the camera + 1 meter forward 
			GameObject newProjectile = GameObjectUtility.Instantiate(projectile, locationSpawn.position + transform.forward);

			// if the projectile does not have a rigidbody component, add one
			if (!newProjectile.GetComponent<Rigidbody2D>()) 
			{
				newProjectile.AddComponent<Rigidbody2D>();
			}
				
			// Apply force to the newProjectile's Rigidbody component if it has one

			// Get the direction correct, is the player facing right?
			_ShootDirectionX =1 ;
			if ( _IFacingRight !=null) {
				if (_IFacingRight.IsFacingRight()){
					_ShootDirectionX =1 ;
				} else { 
					_ShootDirectionX = -1;
				}
			}

			// potential to place in the directional shooting ablity there but fings the general unity verctor conditon based
			// on a user supplied input, arrow keys or player to mouse vector, of gane pad right stick analog
			Vector2 v = new Vector2(_ShootDirectionX * power,0);
			newProjectile.GetComponent<Rigidbody2D>().AddForce(v);

			// play sound effect if set
			if (shootSFX)
			{
				if (newProjectile.GetComponent<AudioSource> ()) { 
					// the projectile has an AudioSource component
					// play the sound clip through the AudioSource component on the gameobject.
					// note: The audio will travel with the gameobject.
					newProjectile.GetComponent<AudioSource> ().PlayOneShot (shootSFX);
				} else {
					// dynamically create a new gameObject with an AudioSource
					// this automatically destroys itself once the audio is done
					AudioSource.PlayClipAtPoint (shootSFX, newProjectile.transform.position);
				}
			}
		}
	}
 

}
