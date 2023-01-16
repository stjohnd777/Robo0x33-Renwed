using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using sillymutts;

#if UNITY_EDITOR 
using UnityEditor;
#endif

public class FireProjectile : AbstractBehaviour
{

    [Tooltip("delay between fring projectiles.")]
    public float shootDelay = .5f;

    [Tooltip("projectile prefab.")]
    public GameObject projectilePrefab;

    [Tooltip("the end of the gun barrel.")]
    public Transform spawnLocation;

    public float power = 10.0f;

    // Reference to AudioClip to play
    public AudioClip shootSFX;

    public bool IsCoolingDown;

    State state;

    void Awake () {

        base.Awake();

        state = GetComponent<State>();
	}

    float timeElapsedSinceFire = 0;
	void Update () {

        timeElapsedSinceFire += Time.deltaTime;

        if (projectilePrefab)
        {

            bool fireRequested = IsRequestingBehavior();

            bool canFire = (timeElapsedSinceFire > shootDelay);

            if (! canFire)
            {
                return;
            }

            state.IsShooting = false;

            if (fireRequested && canFire)
            {
                Fire(spawnLocation.position);

                state.IsShooting = true;
            }  

            


            timeElapsedSinceFire += Time.deltaTime;
        }
	}

    public void Fire(Vector2 orgin)
    {

        timeElapsedSinceFire = 0;

        GameObject projectile = sillymutts.GameObjectUtility.Instantiate(projectilePrefab, orgin);

        float shootDirection = 1f;
        if( inputState.direction == Directions.Left)
        {
            shootDirection = -1;
        }

        // if the projectile does not have a rigidbody component, add one
        if (!projectile.GetComponent<Rigidbody2D>())
        {
            projectile.AddComponent<Rigidbody2D>();
        }
        Vector2 v = new Vector2(shootDirection * power, 0);
        projectile.GetComponent<Rigidbody2D>().AddForce(v);

        if (shootSFX)
        {
            if (projectile.GetComponent<AudioSource>())
            {
                // the projectile has an AudioSource component
                // play the sound clip through the AudioSource component on the gameobject.
                // note: The audio will travel with the gameobject.
                projectile.GetComponent<AudioSource>().PlayOneShot(shootSFX);
            }
            else
            {
                // dynamically create a new gameObject with an AudioSource
                // this automatically destroys itself once the audio is done
                AudioSource.PlayClipAtPoint(shootSFX, projectile.transform.position);
            }
        }

    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(FireProjectile))]
class FireProjectileEditor : AbstractBehaviorEditor
{
    FireProjectile fireProjectile;

    public void OnEnable()
    {
        base.OnEnable();
        fireProjectile = target as FireProjectile;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawDefaultInspector();

        serializedObject.Update();
    }
}
#endif