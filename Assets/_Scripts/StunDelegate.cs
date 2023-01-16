using UnityEngine;
using System.Collections;

/**
 * GameObject hosting this script are required to have a have a script component that implements 
 * the IStunnable interface. In addition a ridgebody2d with corresponding
 * collider not set to trigger.  
 */
public class StunDelegate: MonoBehaviour {

	[Tooltip("Collisions with GameObject tag will stun the host of this script")]
	public string tagStunner = "Player";

	[Tooltip("Force Up.")]
	public float bounceForce = 500f;

	[Tooltip("Partical Effect to display Stunned")]
	public ParticleSystem damageParticalEffect;

    [Tooltip("ApplyDamage")]
    public int headBumpDamage = 0;

    IHealth heath;
    private void Awake()
    {
        heath = GetComponentInParent<IHealth>();
    }

   
    void OnCollisionEnter2D(Collision2D other)
	{
		// is the other object in collision have the tag that will cause a stunn
		// for example the player jumping on the emenies head
		if (other.gameObject.tag == tagStunner)
		{
			// Is the game object hosting this script stannable
			IStunnable targetStunnable = GetComponentInParent<IStunnable>();

			if ( targetStunnable!=null){

					if ( damageParticalEffect ){
					
						GameObject.Instantiate(damageParticalEffect,this.transform.position,this.transform.rotation);

					}
					Rigidbody2D body = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

                    body.velocity = new Vector2( body.velocity.x, 0);

					body.AddForce(new Vector3(0,bounceForce,0));

					// apply stun to stunnable game object
					targetStunnable.Stunned();

                    if ( (headBumpDamage>0) && (heath!=null))
                    {
                        heath.ApplyDamage(headBumpDamage);
                    }
                   



            }
		}
	}



}
